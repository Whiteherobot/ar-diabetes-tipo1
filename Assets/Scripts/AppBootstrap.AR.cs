using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    // Núcleo de la Experiencia AR (compartido por cada libro y por el escaneo genérico),
    // narración de audio y captura de foto.
    public partial class AppBootstrap
    {
        // Núcleo común de una pantalla de Experiencia AR (usada por cada libro y por el escaneo genérico).
        RectTransform BuildArScreenCore(string name, Func<ModelViewer> viewerGetter, UnityEngine.Events.UnityAction onClose)
        {
            var p = Panel(name);
            var bg = UIKit.Box(p, new Color(0, 0, 0, 0), 4, "ARbg"); UIKit.Stretch(bg.rectTransform);

            // Arrastrar en cualquier parte de la pantalla (menos los botones, que van encima)
            // rota la figura anclada — antes no se podía interactuar con el modelo real en AR.
            var dragCatcher = UIKit.Box(p, new Color(0, 0, 0, 0), 0, "DragCatcher");
            dragCatcher.raycastTarget = true;
            UIKit.Stretch(dragCatcher.rectTransform);
            var dr = dragCatcher.gameObject.AddComponent<DragRotate>();
            dr.OnDragX = dx => { arController?.AddYaw(-dx * 0.3f); modelViewer?.AddYaw(-dx * 0.3f); };

            var raw = new GameObject("Model3D", typeof(RectTransform)).AddComponent<RawImage>();
            raw.transform.SetParent(p, false); raw.raycastTarget = false;
            var mv = viewerGetter();
            if (mv != null) { raw.texture = mv.Texture; raw.color = Color.white; }
            else raw.color = new Color(1, 1, 1, 0);
            if (!Land) UIKit.Frac(raw.rectTransform, 0.06f, 0.21f, 0.94f, 0.61f);
            else UIKit.Frac(raw.rectTransform, 0.24f, 0.10f, 0.76f, 0.90f);

            var titleTxt = UIKit.Text(p, "", 50, Color.white);
            UIKit.Frac(titleTxt, 0.08f, 0.82f, 0.72f, 0.88f);
            titleTxt.alignment = TextAlignmentOptions.Left;

            const float S = 2.77f;
            var pill = UIKit.Box(p, new Color(0, 0, 0, 0.54f), Mathf.RoundToInt(16f * S), "Hint");
            pill.rectTransform.anchorMin = new Vector2(.07f, 1); pill.rectTransform.anchorMax = new Vector2(.93f, 1);
            pill.rectTransform.pivot = new Vector2(.5f, 1);
            pill.rectTransform.offsetMin = new Vector2(0, -(30f + 40f) * S); pill.rectTransform.offsetMax = new Vector2(0, -30f * S);
            var hint = UIKit.Text(pill.transform, "Mueve tu teléfono lentamente sobre la página", 15f * S, Color.white, TextAlignmentOptions.Center, FontStyles.Normal);
            UIKit.Stretch(hint.rectTransform, 9f * S);

            // Aviso mientras no hay cámara; se oculta cuando el AR real arranca (ARStateChanged)
            var buscando = UIKit.Node("Buscando", p);
            UIKit.Stretch(buscando);
            buscando.SetAsFirstSibling();
            bg.transform.SetAsFirstSibling();
            var surface = UIKit.Img(buscando, UIKit.Circle(), "Surface");
            surface.color = new Color(0.93f, 0.56f, 0.68f, 0.28f);
            var srt = surface.rectTransform;
            srt.anchorMin = srt.anchorMax = new Vector2(.5f, .34f);
            srt.sizeDelta = new Vector2(280f * S, 70f * S);
            var camIco = UIKit.Img(buscando, icCamera, "Camara"); camIco.color = UIKit.Hex("AEB9C4"); camIco.preserveAspect = true;
            var crt = camIco.rectTransform;
            crt.anchorMin = crt.anchorMax = new Vector2(.5f, .5f);
            crt.sizeDelta = new Vector2(36f * S, 36f * S); crt.anchoredPosition = new Vector2(0, 58f * S);
            var searching = UIKit.Text(buscando, "Buscando una superficie…", 16f * S, Color.white, TextAlignmentOptions.Center, FontStyles.Normal);
            var txt = searching.rectTransform;
            txt.anchorMin = txt.anchorMax = new Vector2(.5f, .5f);
            txt.sizeDelta = new Vector2(320f * S, 26f * S); txt.anchoredPosition = new Vector2(0, 16f * S);
            var permiso = UIKit.Text(buscando, "Permite el acceso a la cámara para ver tu entorno.", 15f * S, UIKit.Hex("AEB9C4"), TextAlignmentOptions.Center, FontStyles.Normal);
            UIKit.Interlinea(permiso, 1.5f);
            permiso.alignment = TextAlignmentOptions.Top;
            var prt = permiso.rectTransform;
            prt.anchorMin = prt.anchorMax = new Vector2(.5f, .5f);
            prt.sizeDelta = new Vector2(280f * S, 50f * S); prt.anchoredPosition = new Vector2(0, -21f * S);

            // Controles. El texto de info vive anclado en 3D junto al modelo (ARController.ToggleInfo);
            // si todavía no se escaneó ningún marcador, el botón cae a un toast con el mismo texto.
            ArBoton(p, icInfo, null, UIKit.Fisio, 58f, () =>
            {
                if (arController != null && arController.HasSpawnedModel) arController.ToggleInfo();
                else Toast(currentArInfoText);
            }, -2);
            ArBoton(p, icAudio, null, UIKit.Scan, 58f, () => PlayNarration(), -1);
            ArBoton(p, icRotate, null, UIKit.Blue, 58f, () => { modelViewer?.GirarPaso(25f); arController?.GirarPaso(25f); }, 0);
            ArBoton(p, null, "+", UIKit.Nutri, 58f, () => { modelViewer?.CambiarTamano(.1f); arController?.CambiarTamano(.1f); }, 1);
            ArBoton(p, null, "−", UIKit.Prog, 58f, () => { modelViewer?.CambiarTamano(-.1f); arController?.CambiarTamano(-.1f); }, 2);
            ArBoton(p, icClose, null, UIKit.Hex("E85091"), 66f, onClose, -1, true);
            ArBoton(p, icCamera, null, UIKit.Blue, 66f, () => StartCoroutine(FlashAndSave()), 1, true);

            var toastTxt = UIKit.Text(p, "", 36, Color.white);
            UIKit.Frac(toastTxt, 0.05f, 0.135f, 0.95f, 0.185f);

            p.gameObject.AddComponent<ArRefs>().Set(raw, bg, hint, titleTxt, toastTxt);
            return p;
        }

        // Botón circular con aro blanco. Lateral: fila bajo el 32 % de la pantalla;
        // inferior (abajo = true): posicion -1 o 1 a cada lado del centro.
        void ArBoton(RectTransform p, Sprite icono, string signo, Color color, float tamano, UnityEngine.Events.UnityAction click, int posicion, bool abajo = false)
        {
            const float S = 2.77f;
            var boton = CircleBtn(p, null, new Color(1, 1, 1, .72f), Color.white, click,
                abajo ? .5f : 1f, abajo ? 0f : .68f, tamano * S,
                abajo ? posicion * 47f * S : -44f * S,
                abajo ? 58f * S : -(29f + posicion * 70f) * S);
            var interior = UIKit.Img(boton.transform, UIKit.Circle(), "Interior"); interior.color = color;
            UIKit.Stretch(interior.rectTransform, 3f * S);
            if (icono != null)
            {
                var ico = UIKit.Img(boton.transform, icono, "Icon"); ico.preserveAspect = true;
                UIKit.Frac(ico, .3f, .3f, .7f, .7f);
            }
            else
            {
                var txt = UIKit.Text(boton.transform, signo, 29f * S, Color.white);
                UIKit.Stretch(txt.rectTransform);
            }
        }

        void ARStateChanged(bool ok)
        {
            if (ok)
            {
                if (arRaw != null) arRaw.enabled = false;
                if (arBg != null) arBg.color = new Color(0, 0, 0, 0); // transparente: se ve la cámara
                if (mainCam != null) mainCam.enabled = false;
                if (globalBg != null) globalBg.gameObject.SetActive(false); // deja pasar el feed real
                if (arHintText != null) arHintText.text = "Mueve tu teléfono lentamente sobre la página";
                MostrarBuscando(false);
                // AR real activo: arRaw (el RawImage del visor de respaldo) queda oculto, pero sin
                // esto su cámara offscreen (+ 2 luces, RenderTexture 800x900) seguía renderizando
                // cada frame en paralelo a la cámara AR real — el doble de trabajo de GPU sin nada
                // visible a cambio. Encontrada como causa de lentitud general en la experiencia AR
                // real (reportado 2026-07-17).
                modelViewer?.SetActive(false);
            }
            else
            {
                if (arRaw != null) arRaw.enabled = modelViewer != null;
                if (arBg != null) arBg.color = UIKit.Hex("17212B"); // fondo oscuro para el visor 3D
                MostrarBuscando(true);
                if (mainCam != null) mainCam.enabled = true;
                if (globalBg != null) globalBg.gameObject.SetActive(true);
                if (arHintText != null) arHintText.text = modelViewer != null
                    ? "Gira y cambia el tamaño del modelo con los botones"
                    : "AR no disponible en este dispositivo para el escaneo libre";
                modelViewer?.SetActive(true);
            }
        }

        void MostrarBuscando(bool ver)
        {
            var grupo = arBg != null ? arBg.transform.parent.Find("Buscando") : null;
            if (grupo != null) grupo.gameObject.SetActive(ver);
        }

        // Nota: usa Toast() (global, visible en cualquier pantalla) en vez de ShowARToast() porque
        // este método lo llaman tanto el botón "Escuchar" de Detalle como el de la Experiencia AR.
        void PlayNarration()
        {
            if (AppState.Muted) { Toast("Audio silenciado (activalo en Configuración)"); return; }
            var clips = isGenericScan ? null : books[currentBook].Narration;
            if (audioSrc == null || clips == null || currentTopic >= clips.Length || clips[currentTopic] == null)
            { Toast("Sin audio disponible"); return; }
            if (audioSrc.isPlaying) { audioSrc.Stop(); Toast("Audio detenido"); }
            else
            {
                audioSrc.clip = clips[currentTopic]; audioSrc.Play();
                var msgs = AppState.MarkAudioHeard(currentBook, currentTopic);
                if (msgs.Count > 0) ShowReward(string.Join("\n", msgs));
                else Toast("Reproduciendo narración…");
            }
        }

        IEnumerator FlashAndSave()
        {
            string dir = System.IO.Path.Combine(Application.persistentDataPath, "Fotos");
            System.IO.Directory.CreateDirectory(dir);
            // ScreenCapture.CaptureScreenshot antepone persistentDataPath incluso si ya le pasamos
            // una ruta absoluta (en este dispositivo terminaba duplicando la ruta y el archivo
            // nunca se guardaba donde la app después buscaba las fotos) — hay que darle una ruta
            // relativa para que la arme bien sola.
            string relFile = "Fotos/foto_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
            yield return new WaitForEndOfFrame();
            ScreenCapture.CaptureScreenshot(relFile);
            if (flashOverlay != null)
            {
                flashOverlay.gameObject.SetActive(true);
                yield return new WaitForSecondsRealtime(0.28f);
                flashOverlay.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(0.25f);
            ShowARToast("Foto guardada");
        }

        // ============================================================
        IEnumerator AutoCapture()
        {
            string dir = Environment.GetEnvironmentVariable("ARCAP_DIR");
            if (string.IsNullOrEmpty(dir)) dir = Application.persistentDataPath;
            string suf = Land ? "_land" : "_port";
            currentBook = 0; currentTopic = 1;
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < panels.Length; i++)
            {
                ShowOnly(panels[i], false);
                yield return new WaitForSeconds(0.8f); // dejar asentar animaciones de entrada
                yield return new WaitForEndOfFrame();
                ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(dir, "cap_" + panels[i].name + suf + ".png"));
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitForSeconds(0.4f);
            Application.Quit();
        }
    }
}
