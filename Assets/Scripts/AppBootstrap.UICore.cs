using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace ARDiabetes
{
    // Infraestructura de UI compartida por todas las pantallas: Canvas raíz, navegación entre
    // paneles (ShowOnly), barra inferior, franja de encabezado, avisos (Toast/Reward) y widgets
    // genéricos (ProgressBar, CircleBtn).
    public partial class AppBootstrap
    {
        // ============================================================
        void BuildCanvas()
        {
            var go = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas),
                                    typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = Land ? new Vector2(1920, 1080) : new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = Land ? 1f : 0f; // portrait: ancho; landscape: alto

            // Fondo global (color + orbes) agrupado para poder OCULTARLO durante el AR real:
            // el Canvas Overlay se compone SIEMPRE encima de lo que rendericen las cámaras de
            // escena, así que un fondo opaco aquí taparía el feed de la cámara por completo.
            globalBg = UIKit.Node("GlobalBg", canvas.transform);
            UIKit.Stretch(globalBg);

            var bg = UIKit.Img(globalBg, null, "Background");
            bg.color = UIKit.Sky;
            UIKit.Stretch(bg.rectTransform);

            // Orbes del diseño: fijos en las esquinas, detrás del contenido
            AddOrbe(globalBg, new Color(180f / 255f, 203f / 255f, 248f / 255f, .29f), new Vector2(1, 1), 15f, -5f, 290f);
            AddOrbe(globalBg, new Color(175f / 255f, 234f / 255f, 209f / 255f, .28f), new Vector2(0, 0), -40f, 170f, 260f);
            AddOrbe(globalBg, new Color(250f / 255f, 228f / 255f, 184f / 255f, .28f), new Vector2(1, 0), 20f, 0f, 180f);

            root = UIKit.Node("SafeRoot", canvas.transform);
            UIKit.Stretch(root);
            root.gameObject.AddComponent<SafeArea>();

            // Toast global (visible en cualquier pantalla)
            toast = UIKit.Text(canvas.transform, "", 40, UIKit.BlueDark);
            var trt = toast.rectTransform;
            trt.anchorMin = trt.anchorMax = new Vector2(0.5f, 0f); trt.pivot = new Vector2(0.5f, 0f);
            trt.anchoredPosition = new Vector2(0, 210); trt.sizeDelta = new Vector2(940, 90);

            // Flash blanco para el botón "Foto" (topmost, cubre toda la pantalla)
            var flashGo = new GameObject("Flash", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            flashGo.transform.SetParent(canvas.transform, false);
            flashOverlay = flashGo.GetComponent<Image>();
            flashOverlay.color = Color.white;
            flashOverlay.raycastTarget = false;
            UIKit.Stretch(flashOverlay.rectTransform);
            Anim.Destello(flashOverlay);
            flashOverlay.gameObject.SetActive(false);

            // Visor de fotos: overlay modal (no es un "panel" de ShowOnly, flota encima de todo).
            photoViewer = UIKit.Node("PhotoViewer", canvas.transform);
            UIKit.Stretch(photoViewer);
            var pvBg = UIKit.Box(photoViewer, new Color(0.04f, 0.07f, 0.11f, 0.94f), 0, "Bg");
            UIKit.Stretch(pvBg.rectTransform);
            photoViewerImg = new GameObject("Img", typeof(RectTransform)).AddComponent<RawImage>();
            photoViewerImg.transform.SetParent(photoViewer, false);
            photoViewerImg.raycastTarget = false;
            UIKit.Frac(photoViewerImg.rectTransform, 0.06f, 0.14f, 0.94f, 0.90f);
            var pvClose = UIKit.Button(photoViewer, "Cerrar", UIKit.Blue, Color.white, 34,
                () => photoViewer.gameObject.SetActive(false), 38);
            UIKit.Frac(R(pvClose), 0.30f, 0.035f, 0.70f, 0.10f);
            photoViewer.gameObject.SetActive(false);

            // Aviso de recompensa (estrellas/nivel/bonus): más llamativo que el toast genérico,
            // con ícono + color dorado + animación de "pop" para que se note de verdad.
            rewardBox = UIKit.Node("Reward", canvas.transform);
            rewardBox.anchorMin = rewardBox.anchorMax = new Vector2(0.5f, 1f);
            rewardBox.pivot = new Vector2(0.5f, 1f);
            rewardBox.anchoredPosition = new Vector2(0, -170);
            rewardBox.sizeDelta = new Vector2(860, 190);
            var rewardBg = UIKit.Box(rewardBox, UIKit.Hex("FFC93C"), 40, "Bg");
            UIKit.Stretch(rewardBg.rectTransform);
            UIKit.AddShadow(rewardBg, 40, 0.30f, -10, 10);
            var rewardStar = UIKit.Img(rewardBox, spStar, "Star"); rewardStar.preserveAspect = true;
            UIKit.Frac(rewardStar, 0.07f, 0.30f, 0.28f, 0.78f);
            Anim.Estrella(rewardStar);
            rewardText = UIKit.Text(rewardBox, "", 34, UIKit.Hex("7A4B00"), TextAlignmentOptions.Left);
            UIKit.Frac(rewardText, 0.32f, 0.10f, 0.94f, 0.90f);
            rewardBox.gameObject.AddComponent<CanvasGroup>();
            rewardBox.localScale = Vector3.zero;
            rewardBox.gameObject.SetActive(false);
        }

        // Muestra un aviso de recompensa llamativo (estrellas ganadas, bonus, nivel) con un "pop"
        // de escala + fade, muy distinto del toast genérico para que no pase desapercibido.
        void ShowReward(string msg)
        {
            if (rewardText == null || rewardBox == null) return;
            rewardText.text = msg;
            if (rewardCo != null) StopCoroutine(rewardCo);
            rewardCo = StartCoroutine(RewardPop());
        }

        IEnumerator RewardPop()
        {
            rewardBox.gameObject.SetActive(true);
            var cg = rewardBox.GetComponent<CanvasGroup>();
            float t = 0, dur = 0.32f;
            while (t < dur)
            {
                t += Time.unscaledDeltaTime;
                float k = Mathf.Clamp01(t / dur);
                float scale = k < 0.7f ? Mathf.Lerp(0.55f, 1.08f, k / 0.7f) : Mathf.Lerp(1.08f, 1f, (k - 0.7f) / 0.3f);
                rewardBox.localScale = Vector3.one * scale;
                cg.alpha = Mathf.Clamp01(k * 2.2f);
                yield return null;
            }
            rewardBox.localScale = Vector3.one; cg.alpha = 1f;
            yield return new WaitForSeconds(1.7f);
            t = 0; dur = 0.28f;
            while (t < dur)
            {
                t += Time.unscaledDeltaTime;
                cg.alpha = 1f - Mathf.Clamp01(t / dur);
                yield return null;
            }
            rewardBox.gameObject.SetActive(false);
        }

        RectTransform Panel(string name)
        {
            var rt = UIKit.Node(name, root);
            UIKit.Stretch(rt);
            rt.gameObject.AddComponent<CanvasGroup>();
            Anim.PantallaEntra(rt);
            return rt;
        }

        void AddOrbe(Transform parent, Color c, Vector2 esquina, float x, float y, float size)
        {
            const float S = 2.77f;
            var img = UIKit.Img(parent, UIKit.Circle(), "Orbe");
            img.color = c;
            var rt = img.rectTransform;
            rt.anchorMin = rt.anchorMax = esquina; rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(x * S, y * S); rt.sizeDelta = new Vector2(size * S, size * S);
        }

        RectTransform BuildRobotGroup(Transform parent, Sprite frente = null)
        {
            // RobotGroup de Figma (280x188): las piezas van en fracciones para escalar con el rect
            // El contenedor lo posiciona quien llama; adentro se conserva la proporción
            var box = UIKit.Node("RobotGroup", parent);
            var group = UIKit.Node("Trio", box);
            var fit = group.gameObject.AddComponent<AspectRatioFitter>();
            fit.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            fit.aspectRatio = 280f / 188f;

            // El personaje elegido va al frente; si era uno de los laterales, el verde toma su lugar.
            if (frente == null) frente = robotVerde;
            var spriteIzq = frente == robotCoral ? robotVerde : robotCoral;
            var spriteDer = frente == robotAzul ? robotVerde : robotAzul;

            var left = UIKit.Img(group, spriteIzq, "RobotBackCoral");
            left.preserveAspect = true;
            UIKit.Frac(left, .021f, .032f, .529f, .894f);
            var right = UIKit.Img(group, spriteDer, "RobotBackAzul");
            right.preserveAspect = true;
            UIKit.Frac(right, .479f, .032f, .986f, .894f);
            var front = UIKit.Img(group, frente, "RobotFront");
            front.preserveAspect = true;
            UIKit.Frac(front, .191f, -.064f, .809f, .989f);
            return box;
        }

        RectTransform BuildHeaderBar(RectTransform p, string title, Color color, Sprite icon,
                                     UnityEngine.Events.UnityAction back, Sprite mascota = null)
        {
            const float S = 2.77f;
            float h = 90f * S;
            var bar = UIKit.Box(p, color, 2, "HeaderBar");
            var brt = bar.rectTransform;
            brt.anchorMin = new Vector2(0, 1); brt.anchorMax = new Vector2(1, 1); brt.pivot = new Vector2(.5f, 1);
            brt.offsetMin = new Vector2(0, -h); brt.offsetMax = Vector2.zero;

            if (back != null)
            {
                var backButton = UIKit.Button(bar.transform, "", new Color(1, 1, 1, 0), Color.white, 16, back, 22, false);
                UIKit.TopLeft(R(backButton), 12f * S, 28f * S, 34f * S, 34f * S);
                var backIcon = UIKit.Img(backButton.transform, figBack, "Back");
                backIcon.color = Color.white;
                backIcon.preserveAspect = true;
                UIKit.Stretch(backIcon.rectTransform, 6f * S);
            }

            // Fila centrada: mascota + ícono + título, separados 7 px como en el diseño
            var label = UIKit.Text(bar.transform, title, 20f * S, Color.white, TextAlignmentOptions.Left);
            label.enableWordWrapping = false;
            UIKit.TamanoFijo(label, 20f * S);
            float anchoTexto = Mathf.Min(label.GetPreferredValues(title).x + 4f, 800f);
            float gap = 7f * S, total = anchoTexto;
            if (icon != null) total += 22f * S + gap;
            if (mascota != null) total += 38f * S + gap;
            float x = -total / 2f;

            if (mascota != null)
            {
                var disco = UIKit.Box(bar.transform, new Color(1, 1, 1, .25f), Mathf.RoundToInt(19f * S), "Mascota");
                Fila(disco.rectTransform, x, 38f * S, 42f * S);
                disco.gameObject.AddComponent<Mask>();
                var robot = UIKit.Img(disco.transform, mascota, "Robot"); robot.preserveAspect = true;
                UIKit.TopLeft(robot.rectTransform, 0, 4f * S, 38f * S, 46f * S);
                x += 38f * S + gap;
            }
            if (icon != null)
            {
                var titleIcon = UIKit.Img(bar.transform, icon, "Icon");
                titleIcon.color = Color.white;
                titleIcon.preserveAspect = true;
                Fila(titleIcon.rectTransform, x, 22f * S, 22f * S);
                x += 22f * S + gap;
            }
            Fila(label.rectTransform, x, anchoTexto, 40f * S);
            return bar.rectTransform;
        }

        void BuildBottomNav(RectTransform p, int active)
        {
            const float S = 2.77f;
            // Solo redondeada arriba: la parte inferior queda fuera de la pantalla
            float bajo = 30f * S, h = 82f * S;
            var bar = UIKit.Box(p, UIKit.Card, Mathf.RoundToInt(22f * S), "BottomNav");
            var brt = bar.rectTransform;
            brt.anchorMin = new Vector2(0, 0); brt.anchorMax = new Vector2(1, 0); brt.pivot = new Vector2(0.5f, 0);
            brt.offsetMin = new Vector2(0, -bajo); brt.offsetMax = new Vector2(0, h);
            UIKit.AddShadow(bar, Mathf.RoundToInt(22f * S), 0.14f, -6, 8);
            var items = UIKit.Node("Items", bar.transform);
            items.anchorMin = Vector2.zero; items.anchorMax = Vector2.one;
            items.offsetMin = new Vector2(12f * S, bajo + 8f * S); items.offsetMax = new Vector2(-12f * S, -8f * S);

            Sprite[] ic = { figHome, figChart, figSettings, figHelp };
            string[] lb = { "Inicio", "Progreso", "Config", "Ayuda" };
            System.Action[] act =
            {
                () => ShowOnly(pMenu),
                () => ShowOnly(pProgreso),
                () => ShowOnly(pConfig),
                () => ShowOnly(pAyuda)
            };
            Color apagado = UIKit.Hex("879BAE");
            for (int i = 0; i < 4; i++)
            {
                int idx = i;
                bool on = i == active;
                var b = UIKit.Button(items, "", on ? UIKit.Hex("F2F8FF") : new Color(0, 0, 0, 0), UIKit.Navy, Mathf.RoundToInt(15f * S), () => act[idx](), 20, true);
                var rt = R(b);
                rt.anchorMin = new Vector2(i / 4f, 0); rt.anchorMax = new Vector2((i + 1) / 4f, 1);
                rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
                var g = UIKit.Img(b.transform, ic[i], "Ico"); g.color = on ? UIKit.Blue : apagado; g.preserveAspect = true;
                var irt = g.rectTransform;
                irt.anchorMin = irt.anchorMax = new Vector2(.5f, .5f);
                irt.sizeDelta = new Vector2(21f * S, 21f * S); irt.anchoredPosition = new Vector2(0, 10f * S);
                var t = UIKit.Text(b.transform, lb[i], 12f * S, on ? UIKit.Blue : apagado);
                t.fontWeight = FontWeight.Bold;
                var trt = t.rectTransform;
                trt.anchorMin = trt.anchorMax = new Vector2(.5f, .5f);
                trt.sizeDelta = new Vector2(84f * S, 18f * S); trt.anchoredPosition = new Vector2(0, -13f * S);
            }
        }

        // Coloca un elemento en la fila central del encabezado; x se mide desde el centro hacia la derecha.
        static void Fila(RectTransform rt, float x, float w, float h)
        {
            rt.anchorMin = rt.anchorMax = new Vector2(.5f, .5f);
            rt.pivot = new Vector2(0, .5f);
            rt.anchoredPosition = new Vector2(x, 0);
            rt.sizeDelta = new Vector2(w, h);
        }

        // ============================================================
        void ShowOnly(RectTransform panel, bool animate = true)
        {
            if (panel != null && stubs.TryGetValue(panel, out var destinoPanel))
            {
                destino = destinoPanel;
                SceneManager.LoadScene(EscenasPorPanel[destinoPanel]);
                return;
            }
            shown = Array.IndexOf(panels, panel);
            if (shown < 0) shown = 0;
            if (audioSrc != null) audioSrc.Stop();
            foreach (var p in panels) if (p != null) p.gameObject.SetActive(false);
            if (panel != null) panel.gameObject.SetActive(true);

            if (panel == pMenu)
            {
                if (starLabel != null) starLabel.text = AppState.Stars.ToString();
                if (holaText != null) holaText.text = AppState.Greeting;
                if (nivelText != null) nivelText.text = "Nivel " + AppState.Level;
                if (nivelFill != null) UIKit.Frac(nivelFill, 0f, 0f, Mathf.Clamp01(AppState.LevelProgress), 1f);
            }
            else if (panel == pProgreso) RefreshProgreso();
            else if (panel == pInsignias) RefreshInsignias();
            else if (panel == pColeccion) RefreshColeccion();
            else if (panel == pConfig) RefreshConfig();
            else if (panel == pJuegos) RefreshJuegos();

            int quizB = Array.IndexOf(bookQuiz, panel);
            if (quizB >= 0) { quizBook = quizB; RefreshQuiz(); }

            // ¿A qué libro (0-2) o pantalla pertenece este panel? -1 si no es Detalle/AR.
            int book = Array.IndexOf(bookDetalle, panel);
            if (book < 0) book = Array.IndexOf(bookAR, panel);

            if (detallePanels.Contains(panel) && book >= 0) { currentBook = book; RefreshDetalle(); }

            bool isBookAr = book >= 0 && bookAR[book] == panel;
            bool isScanAr = panel == pScanAR;
            isGenericScan = isScanAr;

            if (isBookAr || isScanAr)
            {
                var refs = panel.GetComponent<ArRefs>();
                if (refs != null)
                {
                    arRaw = refs.Raw; arBg = refs.Bg; arHintText = refs.Hint;
                    arTitle = refs.Title; arToast = refs.Toast;
                }
                // Mientras ARCore arranca la sesión (puede tardar), mostrar feedback en vez del
                // texto de compilación ("Vista 3D...") que quedaba pegado y se sentía como traba.
                if (arHintText != null) arHintText.text = "Iniciando cámara…";

                if (isBookAr)
                {
                    currentBook = book;
                    modelViewer = book >= 0 && book < modelViewers.Length ? modelViewers[book] : null;
                    var names = new List<string>();
                    if (books[book].Markers != null)
                        foreach (var m in books[book].Markers) if (m != null) names.Add(m.name);
                    arController?.SetScope(names);
                    if (arTitle != null && books != null && currentTopic < books[book].TopicTitle.Length) arTitle.text = books[book].TopicTitle[currentTopic];
                    if (books != null) currentArInfoText = books[book].Desc(currentTopic);
                }
                else
                {
                    modelViewer = null; arController?.SetScope(null);
                    if (arTitle != null) arTitle.text = "Escanear página";
                    currentArInfoText = "Apunta la cámara a cualquier página de los libros para ver su modelo 3D.";
                }
                arController?.Activate(); // null en el preview del editor
            }
            else
            {
                modelViewer = null;
                if (arController != null)
                {
                    arController.Deactivate();
                    if (mainCam != null) mainCam.enabled = true;
                    if (arRaw != null) arRaw.enabled = false;
                    if (globalBg != null) globalBg.gameObject.SetActive(true);
                }
            }
            // Solo la cámara del visor 3D que se está mostrando debe renderizar: las otras 2 se
            // apagan para no gastar GPU en pantallas donde ni siquiera son visibles.
            for (int i = 0; i < modelViewers.Length; i++)
                modelViewers[i]?.SetActive(modelViewers[i] == modelViewer);
            if (animate && panel != null) panel.GetComponent<Anim>()?.Reproducir();
        }

        IEnumerator FadeIn(RectTransform panel)
        {
            var cg = panel.GetComponent<CanvasGroup>();
            float t = 0, dur = 0.26f;
            while (t < dur)
            {
                t += Time.unscaledDeltaTime;
                float k = Mathf.Clamp01(t / dur);
                float e = 1f - Mathf.Pow(1f - k, 3f); // ease-out
                if (cg != null) cg.alpha = e;
                panel.localScale = Vector3.one * Mathf.Lerp(0.97f, 1f, e);
                panel.anchoredPosition = new Vector2(0, Mathf.Lerp(34f, 0f, e));
                yield return null;
            }
            if (cg != null) cg.alpha = 1;
            panel.localScale = Vector3.one;
            panel.anchoredPosition = Vector2.zero;
        }

        Image ProgressBar(Transform parent, float value, float x0, float y0, float x1, float y1,
                          Color? color = null, float animDur = 0f)
        {
            var track = UIKit.Box(parent, UIKit.Hex("E2ECF5"), 16, "Track");
            UIKit.Frac(track.rectTransform, x0, y0, x1, y1);
            var fill = UIKit.Box(track.transform, color ?? UIKit.Nutri, 16, "Fill");
            UIKit.Frac(fill.rectTransform, 0f, 0f, Mathf.Clamp01(value), 1f);
            if (animDur > 0f) Anim.Llenar(fill.GetComponent<Image>(), animDur);
            return fill;
        }

        void SetProgress(Image fill, float value)
        {
            if (fill == null) return;
            var anim = fill.GetComponent<Anim>();
            float target = Mathf.Clamp01(value);
            if (anim == null) { UIKit.Frac(fill, 0f, 0f, target, 1f); return; }
            UIKit.Frac(fill, 0f, 0f, 0f, 1f);
            anim.FijarLlenado(target);
            anim.Reproducir();
        }

        // Botón circular con icono (para controles del visor AR y accesos de Progreso).
        Button CircleBtn(RectTransform parent, Sprite icon, Color bg, Color iconTint,
                         UnityEngine.Events.UnityAction onClick, float ax, float ay, float size, float offx, float offy)
        {
            var go = new GameObject("CircleBtn", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parent, false);
            var img = go.GetComponent<Image>(); img.sprite = UIKit.Circle(); img.color = bg;
            var b = go.AddComponent<Button>(); b.targetGraphic = img; b.transition = Selectable.Transition.None;
            go.AddComponent<PressEffect>();
            if (onClick != null) b.onClick.AddListener(onClick);
            var rt = (RectTransform)go.transform;
            rt.anchorMin = rt.anchorMax = new Vector2(ax, ay); rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(offx, offy); rt.sizeDelta = new Vector2(size, size);
            if (icon != null) { var g = UIKit.Img(go.transform, icon, "Icon"); g.color = iconTint; UIKit.Frac(g, 0.26f, 0.26f, 0.74f, 0.74f); }
            return b;
        }

        void HeaderBand(RectTransform p, Color accent, Sprite icon, string title, UnityEngine.Events.UnityAction onBack, bool showBack = true)
        {
            var band = UIKit.Box(p, accent, 0, "Band");
            // El panel vive dentro de SafeRoot (recortado por el notch/cámara "gota"), así que su
            // borde superior local (fracción 1) NO es el borde real de la pantalla — antes eso
            // dejaba una franja del fondo celeste asomando arriba de la banda de color, con pinta
            // de "cortada". Se calcula cuánto hay que sobrepasar en fracción local del panel para
            // que la banda llegue de verdad hasta el borde físico de la pantalla.
            float topBleed = 1.01f;
            if (Screen.height > 0)
            {
                float safeTop = Screen.safeArea.yMax / Screen.height;
                float safeBottom = Screen.safeArea.yMin / Screen.height;
                float panelFrac = safeTop - safeBottom;
                if (panelFrac > 0.01f) topBleed = 1f + (1f - safeTop) / panelFrac + 0.01f;
            }
            UIKit.Frac(band.rectTransform, 0f, Land ? 0.86f : 0.885f, 1f, topBleed);
            if (showBack)
            {
                var back = UIKit.Button(band.transform, "‹", new Color(1, 1, 1, 0.22f), Color.white, 26, onBack, 46);
                var brt = R(back); brt.anchorMin = brt.anchorMax = new Vector2(0, 0.5f); brt.pivot = new Vector2(0, 0.5f);
                brt.anchoredPosition = new Vector2(36, 0); brt.sizeDelta = new Vector2(96, 96);
            }
            if (icon != null)
            {
                var g = UIKit.Img(band.transform, icon, "Ico"); g.color = Color.white;
                var grt = R(g); grt.anchorMin = grt.anchorMax = new Vector2(0, 0.5f); grt.pivot = new Vector2(0, 0.5f);
                // Mismo sistema de coordenadas (píxeles fijos, anclado a la izquierda) que el botón de
                // volver: antes el ícono se posicionaba por fracción del ANCHO de la franja, así que en
                // landscape (franja mucho más ancha) quedaba muy separado de la flecha. Con píxeles fijos
                // el hueco entre flecha e ícono es siempre el mismo, sin importar la orientación.
                grt.anchoredPosition = new Vector2(showBack ? 148 : 36, 0); grt.sizeDelta = new Vector2(64, 64);
            }
            // Centrado en TODA la franja (no en el hueco libre a la derecha del ícono/flecha),
            // como en cualquier barra superior — antes quedaba corrido hacia la derecha.
            var t = UIKit.Text(band.transform, title, 50, Color.white);
            UIKit.Frac(t, 0.06f, 0.1f, 0.94f, 0.9f);
        }

        void ShowARToast(string msg)
        {
            if (arToast == null) return;
            arToast.text = msg;
            CancelInvoke(nameof(ClearARToast));
            Invoke(nameof(ClearARToast), 1.6f);
        }
        void ClearARToast() { if (arToast != null) arToast.text = ""; }

        void Toast(string msg)
        {
            if (toast == null) return;
            toast.text = msg;
            CancelInvoke(nameof(ClearToast));
            Invoke(nameof(ClearToast), 2.2f);
        }
        void ClearToast() { if (toast != null) toast.text = ""; }
    }
}
