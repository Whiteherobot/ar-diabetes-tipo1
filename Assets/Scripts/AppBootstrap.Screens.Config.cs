using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    // Pantallas 8) CONFIGURACIÓN 9) AYUDA.
    public partial class AppBootstrap
    {
        // ============================================================
        // Piezas comunes de Mi espacio y Ayuda
        // ============================================================

        // Portada de color con círculos, estrellas y robot; se desplaza con el contenido.
        void BuildPortada(RectTransform content, float w, Color color, string kicker, string titulo, string sub, Sprite robot, float maxTitulo, float estrellaY = 34f)
        {
            const float S = 2.77f;
            var hero = UIKit.Box(content, color, 2, "Portada");
            UIKit.Top(hero.rectTransform, 0, 0, w, 250f * S);
            hero.gameObject.AddComponent<Mask>();
            var c1 = UIKit.Img(hero.transform, UIKit.Circle(), "Circulo"); c1.color = new Color(1, 1, 1, .11f);
            UIKit.TopLeft(c1.rectTransform, w - 115f * S, -60f * S, 185f * S, 185f * S);
            var c2 = UIKit.Img(hero.transform, UIKit.Circle(), "Circulo"); c2.color = new Color(1, 1, 1, .11f);
            UIKit.TopLeft(c2.rectTransform, -52f * S, 188f * S, 130f * S, 130f * S);
            var chispa1 = UIKit.Img(hero.transform, figStar, "Estrella"); chispa1.color = UIKit.Hex("FFF2A9"); chispa1.preserveAspect = true;
            UIKit.TopLeft(chispa1.rectTransform, w - 166f * S, estrellaY * S, 22f * S, 22f * S);
            chispa1.rectTransform.localRotation = Quaternion.Euler(0, 0, 11f);
            var chispa2 = UIKit.Img(hero.transform, figStar, "Estrella"); chispa2.color = Color.white; chispa2.preserveAspect = true;
            UIKit.TopLeft(chispa2.rectTransform, w - 38f * S, 204f * S, 15f * S, 15f * S);
            chispa2.rectTransform.localRotation = Quaternion.Euler(0, 0, -14f);

            var k = UIKit.Text(hero.transform, kicker, 12f * S, Color.white, TextAlignmentOptions.Left);
            k.characterSpacing = 8f;
            UIKit.TopLeft(k.rectTransform, 23f * S, 45f * S, w - 46f * S, 18f * S);
            var t = UIKit.Text(hero.transform, titulo, 31f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.TamanoFijo(t, 31f * S);
            UIKit.Interlinea(t, 1.08f);
            float altoT = UIKit.Alto(t, titulo, maxTitulo);
            UIKit.TopLeft(t.rectTransform, 23f * S, 71f * S, maxTitulo, altoT + 4f);
            var sb = UIKit.Text(hero.transform, sub, 17f * S, Color.white, TextAlignmentOptions.Left, FontStyles.Normal);
            sb.fontWeight = FontWeight.Bold;
            UIKit.Interlinea(sb, 1.36f);
            UIKit.TopLeft(sb.rectTransform, 23f * S, 76f * S + altoT, 230f * S, 50f * S);

            var img = UIKit.Img(hero.transform, robot, "Robot"); img.preserveAspect = true;
            UIKit.TopLeft(img.rectTransform, w - 142f * S, 88f * S, 134f * S, 163f * S);
            UIKit.SombraImagen(img, .18f, 7f * S);
            Anim.RobotEntra(img);
        }

        // Tarjeta con ícono cuadrado, rótulo y título; el contenido empieza 90 px abajo.
        Image BuildTarjetaPref(RectTransform content, float y, float h, float w, Color color, Sprite icono, string rotulo, string titulo)
        {
            const float S = 2.77f;
            var card = UIKit.Box(content, Color.white, Mathf.RoundToInt(24f * S), "Pref");
            UIKit.Top(card.rectTransform, 0, y, w, h);
            UIKit.AddShadow(card, Mathf.RoundToInt(24f * S), .10f, -5, 5);
            var caja = UIKit.Box(card.transform, color, Mathf.RoundToInt(16f * S), "IconoCaja");
            UIKit.TopLeft(caja.rectTransform, 20f * S, 20f * S, 50f * S, 50f * S);
            var ico = UIKit.Img(caja.transform, icono, "Icono"); ico.color = Color.white; ico.preserveAspect = true;
            UIKit.Stretch(ico.rectTransform, 12.5f * S);
            var r = UIKit.Text(card.transform, rotulo, 11f * S, UIKit.Hex("59738C"), TextAlignmentOptions.Left);
            r.characterSpacing = 7f;
            UIKit.TopLeft(r.rectTransform, 82f * S, 22f * S, w - 100f * S, 17f * S);
            var t = UIKit.Text(card.transform, titulo, 22f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.TopLeft(t.rectTransform, 82f * S, 40f * S, w - 100f * S, 30f * S);
            return card;
        }

        TMP_Text TextoMedido(Transform parent, string texto, float x, float y, float ancho, float size, float interlinea, Color color)
        {
            var t = UIKit.Text(parent, texto, size, color, TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.TamanoFijo(t, size);
            UIKit.Interlinea(t, interlinea);
            UIKit.TopLeft(t.rectTransform, x, y, ancho, UIKit.Alto(t, texto, ancho) + 4f);
            return t;
        }

        // Selector de opciones en una barra; devuelve los botones para repintar el elegido.
        void BuildSegmentado(Transform parent, float y, float w, string[] etiquetas, int elegido, System.Action<int> alElegir)
        {
            const float S = 2.77f;
            var barra = UIKit.Box(parent, UIKit.Hex("EAF0F5"), Mathf.RoundToInt(17f * S), "Segmentado");
            UIKit.TopLeft(barra.rectTransform, 20f * S, y, w, 58f * S);
            float bw = (w - 10f * S - 6f * S * (etiquetas.Length - 1)) / etiquetas.Length;
            var fondos = new Image[etiquetas.Length]; var textos = new TMP_Text[etiquetas.Length];
            System.Action<int> pintar = sel =>
            {
                for (int i = 0; i < fondos.Length; i++)
                {
                    fondos[i].color = i == sel ? UIKit.Fisio : new Color(0, 0, 0, 0);
                    textos[i].color = i == sel ? Color.white : UIKit.Hex("526A82");
                }
            };
            for (int i = 0; i < etiquetas.Length; i++)
            {
                int idx = i;
                var b = UIKit.Button(barra.transform, "", Color.white, UIKit.Navy, Mathf.RoundToInt(13f * S), () => { pintar(idx); alElegir(idx); }, 10);
                UIKit.TopLeft(R(b), 5f * S + i * (bw + 6f * S), 5f * S, bw, 48f * S);
                fondos[i] = b.GetComponent<Image>();
                textos[i] = UIKit.Text(b.transform, etiquetas[i], 14f * S, Color.white);
                UIKit.Stretch(textos[i].rectTransform, 3f * S);
            }
            pintar(elegido);
        }

        // ============================================================
        // 8 - CONFIGURACIÓN
        // ============================================================
        RectTransform BuildConfig()
        {
            var p = Panel("8_Config");
            const float S = 2.77f;
            float w = Land ? 1000 : 1080, cardW = w - 40f * S, inW = cardW - 40f * S;
            var content = BuildAreaScroll(p, "Config", 82f * S, w);
            var viewport = (RectTransform)content.parent;
            viewport.offsetMax = Vector2.zero; // la portada llega hasta arriba: esta pantalla no tiene encabezado
            BuildPortada(content, w, UIKit.Blue, "MI ESPACIO", "¡Mi espacio!", "Personaliza tu aventura.", PersonajeElegido(), 230f * S);

            float y = 232f * S;
            var welcome = UIKit.Box(content, Color.white, Mathf.RoundToInt(24f * S), "SettingsWelcome");
            UIKit.Top(welcome.rectTransform, 0, y, cardW, 138f * S);
            UIKit.AddShadow(welcome, Mathf.RoundToInt(24f * S), .10f, -5, 5);
            var robotBg = UIKit.Box(welcome.transform, UIKit.Hex("EEEAFF"), Mathf.RoundToInt(21f * S), "RobotBg");
            UIKit.TopLeft(robotBg.rectTransform, 20f * S, 33f * S, 72f * S, 72f * S);
            robotBg.gameObject.AddComponent<Mask>();
            var welcomeRobot = UIKit.Img(robotBg.transform, PersonajeElegido(), "Robot"); welcomeRobot.preserveAspect = true;
            UIKit.TopLeft(welcomeRobot.rectTransform, 3.5f * S, 7f * S, 65f * S, 79f * S);
            var wk = UIKit.Text(welcome.transform, "TU AVENTURA, TUS ELECCIONES", 11f * S, UIKit.Hex("6952C7"), TextAlignmentOptions.Left);
            wk.characterSpacing = 7f;
            UIKit.TopLeft(wk.rectTransform, 107f * S, 20f * S, cardW - 127f * S, 17f * S);
            var wt = UIKit.Text(welcome.transform, "¡Todo a tu gusto!", 23f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.TopLeft(wt.rectTransform, 107f * S, 38f * S, cardW - 127f * S, 32f * S);
            var wb = UIKit.Text(welcome.transform, "Aquí puedes elegir cómo quieres aprender y explorar.", 16f * S, UIKit.Hex("536B84"), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Interlinea(wb, 1.4f);
            UIKit.TopLeft(wb.rectTransform, 107f * S, 72f * S, cardW - 127f * S, 48f * S);
            y += 138f * S + 25f * S;

            // Mi experiencia: tamaño de letra del diseño y el sonido, que ya existía en la app
            var exp = BuildTarjetaPref(content, y, 362f * S, cardW, UIKit.Fisio, figSettings, "PERSONALIZACIÓN", "Mi experiencia");
            var st = UIKit.Text(exp.transform, "Tamaño de letra", 18f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.TopLeft(st.rectTransform, 20f * S, 90f * S, inW, 25f * S);
            TextoMedido(exp.transform, "Elige cómo quieres leer los textos.", 20f * S, 117f * S, inW, 15f * S, 1.4f, UIKit.Hex("5A7189"));
            float[] escalas = { 1f, 1.15f, 1.3f };
            int escalaActual = System.Array.FindIndex(escalas, e => Mathf.Approximately(AppState.FontScale, e));
            BuildSegmentado(exp.transform, 152f * S, inW, new[] { "Normal", "Grande", "Muy grande" }, Mathf.Max(0, escalaActual),
                i => { AppState.FontScale = escalas[i]; BuildAll(); });
            var so = UIKit.Text(exp.transform, "Sonido y narración", 18f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.TopLeft(so.rectTransform, 20f * S, 224f * S, inW, 25f * S);
            TextoMedido(exp.transform, "Activa o silencia la música y las voces.", 20f * S, 251f * S, inW, 15f * S, 1.4f, UIKit.Hex("5A7189"));
            BuildSegmentado(exp.transform, 286f * S, inW, new[] { "Activado", "Silenciado" }, AppState.Muted ? 1 : 0,
                i => { bool silencio = i == 1; AppState.Muted = silencio; if (bgAudioSrc != null) { if (AppState.Muted) bgAudioSrc.Pause(); else bgAudioSrc.UnPause(); }  });
            y += 362f * S + 22f * S;

            // Mi personaje: lo pidió el cliente, para cambiar lo elegido al inicio
            var per = BuildTarjetaPref(content, y, 263f * S, cardW, UIKit.Blue, icProfile, "TU EXPLORADOR", "Mi personaje");
            var fila = UIKit.Box(per.transform, UIKit.Hex("F1F7FB"), Mathf.RoundToInt(18f * S), "Identidad");
            UIKit.TopLeft(fila.rectTransform, 20f * S, 90f * S, inW, 84f * S);
            var perBg = UIKit.Box(fila.transform, UIKit.Hex("E6F3FF"), Mathf.RoundToInt(16f * S), "RobotBg");
            UIKit.TopLeft(perBg.rectTransform, 13f * S, 13f * S, 58f * S, 58f * S);
            perBg.gameObject.AddComponent<Mask>();
            var perRobot = UIKit.Img(perBg.transform, PersonajeElegido(), "Robot"); perRobot.preserveAspect = true;
            UIKit.TopLeft(perRobot.rectTransform, 3f * S, 5f * S, 52f * S, 63f * S);
            var profileSub = UIKit.Text(fila.transform, PerfilResumen(), 18f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.TopLeft(profileSub.rectTransform, 84f * S, 16f * S, inW - 96f * S, 52f * S);
            var cambiar = UIKit.Button(per.transform, "CAMBIAR NOMBRE Y PERSONAJE", UIKit.Blue, Color.white, Mathf.RoundToInt(16f * S), () => ShowOnly(pPerfil), Mathf.RoundToInt(15f * S));
            UIKit.TopLeft(R(cambiar), 20f * S, 191f * S, inW, 52f * S);
            y += 263f * S + 22f * S;

            // Mis datos
            string datosTxt = "Aquí puedes revisar y administrar tu progreso.";
            var data = BuildTarjetaPref(content, y, 10f, cardW, UIKit.Scan, figChart, "ALMACENAMIENTO LOCAL", "Mis datos");
            var datosP = TextoMedido(data.transform, datosTxt, 20f * S, 90f * S, inW, 16f * S, 1.5f, UIKit.Hex("4F6881"));
            float yBoton = 90f * S + datosP.rectTransform.sizeDelta.y + 13f * S;
            var resetBorde = UIKit.Button(data.transform, "", UIKit.Coral, Color.white, Mathf.RoundToInt(16f * S), null, 10);
            UIKit.TopLeft(R(resetBorde), 20f * S, yBoton, inW, 52f * S);
            var resetInt = UIKit.Box(resetBorde.transform, UIKit.Hex("FFF1F2"), Mathf.RoundToInt(14f * S), "Interior");
            UIKit.Stretch(resetInt.rectTransform, 2f * S);
            var resetTxt = UIKit.Text(resetInt.transform, "REINICIAR MI PROGRESO", 16f * S, UIKit.Hex("BD3540"));
            UIKit.Stretch(resetTxt.rectTransform);
            float altoData = yBoton + 52f * S + 20f * S;
            AjustarAlto(data, altoData, -5);
            y += altoData + 22f * S;

            // Acerca de
            var about = BuildTarjetaPref(content, y, 10f, cardW, UIKit.Hex("6F8196"), figInfo, "INFORMACIÓN", "Acerca de esta aventura");
            var aboutP = TextoMedido(about.transform, "Una aplicación educativa para aprender sobre diabetes tipo 1 mediante ilustraciones, juegos y realidad aumentada.", 20f * S, 90f * S, inW, 16f * S, 1.5f, UIKit.Hex("4F6881"));
            float yAcerca = 90f * S + aboutP.rectTransform.sizeDelta.y + 13f * S;
            var ident = UIKit.Box(about.transform, UIKit.Hex("F1F7FB"), Mathf.RoundToInt(18f * S), "Identidad");
            UIKit.TopLeft(ident.rectTransform, 20f * S, yAcerca, inW, 84f * S);
            var appBg = UIKit.Box(ident.transform, UIKit.Hex("DFF4E9"), Mathf.RoundToInt(16f * S), "RobotBg");
            UIKit.TopLeft(appBg.rectTransform, 13f * S, 13f * S, 58f * S, 58f * S);
            appBg.gameObject.AddComponent<Mask>();
            var appRobot = UIKit.Img(appBg.transform, robotVerde, "Robot"); appRobot.preserveAspect = true;
            UIKit.TopLeft(appRobot.rectTransform, 3f * S, 5f * S, 52f * S, 63f * S);
            var appNombre = UIKit.Text(ident.transform, "Diabetes Tipo 1", 18f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.TopLeft(appNombre.rectTransform, 84f * S, 16f * S, inW - 96f * S, 27f * S);
            var appVersion = UIKit.Text(ident.transform, "Versión " + Application.version, 15f * S, UIKit.Hex("657B91"), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.TopLeft(appVersion.rectTransform, 84f * S, 44f * S, inW - 96f * S, 24f * S);
            yAcerca += 84f * S + 17f * S;
            var nota = UIKit.Box(about.transform, UIKit.Hex("EDF7FF"), Mathf.RoundToInt(14f * S), "Nota");
            var notaP = TextoMedido(nota.transform, "Esta aplicación complementa la educación terapéutica y no reemplaza las indicaciones del equipo de salud.", 18f * S, 14f * S, inW - 32f * S, 15f * S, 1.5f, UIKit.Hex("4F6881"));
            float altoNota = notaP.rectTransform.sizeDelta.y + 28f * S;
            UIKit.TopLeft(nota.rectTransform, 20f * S, yAcerca, inW, altoNota);
            var notaLinea = UIKit.Box(nota.transform, UIKit.Blue, 2, "Linea");
            UIKit.TopLeft(notaLinea.rectTransform, 0, 0, 4f * S, altoNota);
            float altoAbout = yAcerca + altoNota + 20f * S;
            AjustarAlto(about, altoAbout, -5);
            y += altoAbout + 26f * S;
            content.sizeDelta = new Vector2(w, y);

            // Confirmación de reinicio
            var resetModal = UIKit.Node("ResetModal", p);
            UIKit.Stretch(resetModal);
            var backdrop = UIKit.Box(resetModal, new Color(20f / 255f, 42f / 255f, 65f / 255f, .55f), 2, "Backdrop");
            UIKit.Stretch(backdrop.rectTransform);
            backdrop.raycastTarget = true;
            float modalW = Mathf.Min(350f * S, w - 40f * S), modalIn = modalW - 44f * S;
            var confirmation = UIKit.Box(resetModal, Color.white, Mathf.RoundToInt(24f * S), "Confirmation");
            var crt = confirmation.rectTransform;
            crt.anchorMin = crt.anchorMax = new Vector2(.5f, .5f);
            crt.sizeDelta = new Vector2(modalW, 318f * S);
            UIKit.AddShadow(confirmation, Mathf.RoundToInt(24f * S), .22f, -10, 12);
            Anim.Confirmacion(confirmation);
            var aviso = UIKit.Box(confirmation.transform, UIKit.Hex("FFF0F1"), Mathf.RoundToInt(20f * S), "WarningCircle");
            UIKit.TopLeft(aviso.rectTransform, (modalW - 64f * S) / 2f, 26f * S, 64f * S, 64f * S);
            var warning = UIKit.Img(aviso.transform, figWarning != null ? figWarning : icAlert, "Warning");
            warning.color = UIKit.Hex("C13C47"); warning.preserveAspect = true; UIKit.Stretch(warning.rectTransform, 17.5f * S);
            var mt = UIKit.Text(confirmation.transform, "¿Reiniciar tu progreso?", 24f * S, UIKit.Navy);
            UIKit.TopLeft(mt.rectTransform, 22f * S, 104f * S, modalIn, 34f * S);
            var mb = UIKit.Text(confirmation.transform, "Se eliminarán tus estrellas y el avance de las misiones. Tus ajustes y fotografías permanecerán guardados.",
                16f * S, UIKit.Hex("546C83"), TextAlignmentOptions.Center, FontStyles.Normal);
            mb.lineSpacing = 11f;
            UIKit.TopLeft(mb.rectTransform, 22f * S, 147f * S, modalIn, 76f * S);
            float bw = (modalIn - 9f * S) / 2f;
            var cancel = UIKit.Button(confirmation.transform, "Cancelar", UIKit.Hex("EDF2F6"), UIKit.Navy,
                Mathf.RoundToInt(15f * S), () => resetModal.gameObject.SetActive(false), Mathf.RoundToInt(15f * S));
            UIKit.TopLeft(R(cancel), 22f * S, 244f * S, bw, 52f * S);
            var confirm = UIKit.Button(confirmation.transform, "Sí, reiniciar", UIKit.Hex("D8414D"), Color.white,
                Mathf.RoundToInt(15f * S), () => { resetModal.gameObject.SetActive(false); AppState.ResetProgress(); RefreshConfig(); RefreshProgreso(); Toast("Tu progreso se reinició correctamente."); }, Mathf.RoundToInt(15f * S));
            UIKit.TopLeft(R(confirm), 22f * S + bw + 9f * S, 244f * S, bw, 52f * S);
            resetModal.gameObject.SetActive(false);
            resetBorde.onClick.AddListener(() => resetModal.gameObject.SetActive(true));

            BuildBottomNav(p, 2);
            resetModal.SetAsLastSibling(); // el modal tapa también la barra inferior
            p.gameObject.AddComponent<ConfigRefs>().Set(profileSub, null, resetModal);
            return p;
        }

        // Cambia el alto de una tarjeta ya creada con AddShadow (la sombra es el hermano anterior).
        static void AjustarAlto(Image card, float alto, float dySombra)
        {
            var rt = card.rectTransform;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, alto);
            var sombra = rt.parent.GetChild(rt.GetSiblingIndex() - 1) as RectTransform;
            if (sombra != null) UIKit.AjustarSombra(sombra, rt, dySombra);
        }

        static string PerfilResumen()
        {
            var nombre = AppState.PlayerName.Trim();
            return nombre.Length > 0 ? nombre : "Explorador";
        }

        void RefreshConfig()
        {
            if (pConfig == null) return;
            var refs = pConfig.GetComponent<ConfigRefs>();
            if (refs == null) return;
            if (refs.ProfileSub != null) refs.ProfileSub.text = PerfilResumen();
        }

        // ============================================================
        // 9 - AYUDA
        // ============================================================
        RectTransform BuildAyuda()
        {
            var p = Panel("9_Ayuda");
            const float S = 2.77f;
            string[] q =
            {
                "¿Cómo escaneo una página?",
                "¿Cómo gano estrellas?",
                "¿La figura no aparece o se ve rara?",
                "¿Se guarda mi progreso?"
            };
            string[] a =
            {
                "Abre la opción Escanear Página. Permite el acceso a la cámara y apunta hacia la imagen del libro. Mantén el teléfono estable para descubrir el contenido en realidad aumentada.",
                "Puedes conseguir estrellas al completar los cuestionarios de los libros. ¡Cada actividad es una oportunidad para seguir aprendiendo!",
                "Revisa que tengas buena iluminación, acerca o aleja lentamente la cámara y procura mantener visible la imagen del libro.",
                "Tus estrellas, respuestas y fotografías se guardan en este dispositivo cuando el almacenamiento local está disponible. Puedes revisarlas en la sección Progreso."
            };
            Sprite[] iconos = { figScan, figStar, figCamera != null ? figCamera : icCamera, figChart };
            Color[] colores = { UIKit.Scan, UIKit.Prog, UIKit.Fisio, UIKit.Nutri };

            var fondo = UIKit.Img(p, null, "Fondo"); fondo.color = UIKit.Hex("FFF8F1"); UIKit.Stretch(fondo.rectTransform);
            float w = Land ? 1000 : 1080, cardW = w - 40f * S;
            var content = BuildAreaScroll(p, "Help", 82f * S, w);
            ((RectTransform)content.parent).offsetMax = Vector2.zero;
            BuildPortada(content, w, UIKit.Scan, "¡ESTAMOS AQUÍ PARA AYUDARTE!", "¿Necesitas ayuda?", "¡Te explicamos cómo funciona todo!", robotNaranja, 215f * S, 74f);

            // Bienvenida
            float inW = cardW - 46f * S;
            var welcome = UIKit.Box(content, Color.white, Mathf.RoundToInt(24f * S), "Welcome");
            UIKit.Top(welcome.rectTransform, 0, 232f * S, cardW, 250f * S);
            var welcomeSombra = UIKit.AddShadow(welcome, Mathf.RoundToInt(24f * S), .10f, -7, 7).rectTransform;
            var wa = UIKit.Box(welcome.transform, UIKit.Hex("E6F3FF"), Mathf.RoundToInt(19f * S), "Avatar");
            UIKit.TopLeft(wa.rectTransform, 23f * S, 23f * S, 64f * S, 64f * S);
            wa.gameObject.AddComponent<Mask>();
            var wr = UIKit.Img(wa.transform, PersonajeElegido(), "Robot"); wr.preserveAspect = true;
            UIKit.TopLeft(wr.rectTransform, 3.5f * S, 6f * S, 57f * S, 69f * S);
            var wk = UIKit.Text(welcome.transform, "TU CENTRO DE AYUDA", 11f * S, UIKit.Hex("C25F15"), TextAlignmentOptions.Left);
            wk.characterSpacing = 8f;
            UIKit.TopLeft(wk.rectTransform, 100f * S, 30f * S, cardW - 123f * S, 17f * S);
            var wt = UIKit.Text(welcome.transform, AppState.Greeting, 23f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.TopLeft(wt.rectTransform, 100f * S, 48f * S, cardW - 123f * S, 32f * S);
            var wb = TextoMedido(welcome.transform, "Estoy aquí para ayudarte. Puedes leer las respuestas o escucharlas cuando quieras.", 23f * S, 104f * S, inW, 17f * S, 1.5f, UIKit.Hex("4F6781"));
            float yBotonBienvenida = 104f * S + wb.rectTransform.sizeDelta.y + 13f * S;

            var seccion = UIKit.Text(content, "Preguntas frecuentes", 23f * S, UIKit.Navy, TextAlignmentOptions.Left);

            // Reproductor único: se muda a la tarjeta que esté sonando
            var player = UIKit.Box(welcome.transform, UIKit.Hex("FFF2E6"), Mathf.RoundToInt(17f * S), "Reproductor");
            player.gameObject.SetActive(false);
            var plIco = UIKit.Box(player.transform, UIKit.Hex("E86E18"), Mathf.RoundToInt(13f * S), "Altavoz");
            UIKit.TopLeft(plIco.rectTransform, 11f * S, 12f * S, 42f * S, 42f * S);
            var plIcoImg = UIKit.Img(plIco.transform, figSpeaker, "Icono"); plIcoImg.color = Color.white; plIcoImg.preserveAspect = true;
            UIKit.Stretch(plIcoImg.rectTransform, 9.5f * S);
            var plTxt = UIKit.Text(player.transform, "Escuchando explicación", 14f * S, UIKit.Hex("57300F"), TextAlignmentOptions.Left);
            var ondas = BuildSoundWaves(player.transform, 61f * S, 37f * S, 30f * S, 12f * S);
            var plPausa = UIKit.Button(player.transform, "", Color.white, UIKit.Navy, Mathf.RoundToInt(14f * S), null, 10);
            var plPausaIco = UIKit.Img(plPausa.transform, figPause, "Icono"); plPausaIco.color = UIKit.Hex("B44C0A"); plPausaIco.preserveAspect = true;
            UIKit.Stretch(plPausaIco.rectTransform, 11.5f * S);
            var plStop = UIKit.Button(player.transform, "", Color.white, UIKit.Navy, Mathf.RoundToInt(14f * S), null, 10);
            var plStopIco = UIKit.Img(plStop.transform, figStop, "Icono"); plStopIco.color = UIKit.Hex("B44C0A"); plStopIco.preserveAspect = true;
            UIKit.Stretch(plStopIco.rectTransform, 12.5f * S);

            // Preguntas: acordeón, una abierta a la vez
            var tarjetas = new Image[4]; var sombras = new RectTransform[4]; var interiores = new RectTransform[4];
            var respuestas = new TMP_Text[4]; var lineas = new GameObject[4]; var chevrones = new Image[4];
            var altavoces = new Image[4]; var altavozIcos = new Image[4]; var altosCabecera = new float[4];
            float faqAnchoTexto = cardW - 192f * S, faqAnchoResp = cardW - 44f * S;
            int abierta = 0, sonando = -1;
            System.Action acomodar = null;
            System.Action<int> reproducir = null;
            for (int i = 0; i < 4; i++)
            {
                int idx = i;
                var card = UIKit.Box(content, Color.white, Mathf.RoundToInt(20f * S), "Faq" + i);
                sombras[i] = UIKit.AddShadow(card, Mathf.RoundToInt(20f * S), .12f, -5, 5).rectTransform;
                UIKit.Top(card.rectTransform, 0, 0, cardW, 92f * S);
                tarjetas[i] = card;
                var interior = UIKit.Box(card.transform, Color.white, Mathf.RoundToInt(18f * S), "Interior");
                UIKit.Stretch(interior.rectTransform, 2f * S);
                interiores[i] = interior.rectTransform;
                var titulo = UIKit.Text(interior.transform, q[i], 18f * S, UIKit.Navy, TextAlignmentOptions.Left);
                UIKit.TamanoFijo(titulo, 18f * S);
                UIKit.Interlinea(titulo, 1.25f);
                float altoTitulo = UIKit.Alto(titulo, q[i], faqAnchoTexto);
                float cabecera = Mathf.Max(88f * S, altoTitulo + 30f * S);
                altosCabecera[i] = cabecera;
                var toggle = UIKit.Button(interior.transform, "", new Color(1, 1, 1, 0), UIKit.Navy, 2, () => { abierta = abierta == idx ? -1 : idx; acomodar(); }, 10, false);
                UIKit.TopLeft(R(toggle), 0, 0, cardW - 64f * S, cabecera);
                titulo.transform.SetParent(toggle.transform, false);
                UIKit.TopLeft(titulo.rectTransform, 83f * S, (cabecera - altoTitulo) / 2f, faqAnchoTexto, altoTitulo + 4f);
                var caja = UIKit.Box(toggle.transform, colores[i], Mathf.RoundToInt(18f * S), "IconoCaja");
                UIKit.TopLeft(caja.rectTransform, 13f * S, (cabecera - 58f * S) / 2f, 58f * S, 58f * S);
                var ico = UIKit.Img(caja.transform, iconos[i], "Icono"); ico.color = Color.white; ico.preserveAspect = true;
                UIKit.Stretch(ico.rectTransform, 15.5f * S);
                var chev = UIKit.Img(toggle.transform, figChevron, "Chevron"); chev.color = UIKit.Hex("71869D"); chev.preserveAspect = true;
                UIKit.TopLeft(chev.rectTransform, cardW - 97f * S, (cabecera - 22f * S) / 2f, 22f * S, 22f * S);
                chev.rectTransform.pivot = new Vector2(.5f, .5f);
                chev.rectTransform.anchoredPosition += new Vector2(11f * S, -11f * S);
                chevrones[i] = chev;
                var audio = UIKit.Button(interior.transform, "", UIKit.Hex("FFF0E5"), UIKit.Navy, Mathf.RoundToInt(24f * S), () => { abierta = idx; reproducir(idx + 1); }, 10);
                UIKit.TopLeft(R(audio), cardW - 64f * S, (cabecera - 48f * S) / 2f, 48f * S, 48f * S);
                altavoces[i] = audio.GetComponent<Image>();
                var audioIco = UIKit.Img(audio.transform, figSpeaker, "Icono"); audioIco.color = UIKit.Hex("D76110"); audioIco.preserveAspect = true;
                UIKit.Stretch(audioIco.rectTransform, 13f * S);
                altavozIcos[i] = audioIco;
                var linea = UIKit.Box(interior.transform, UIKit.Hex("EEF1F4"), 2, "Linea");
                UIKit.TopLeft(linea.rectTransform, 16f * S, cabecera, cardW - 36f * S, 1f * S);
                lineas[i] = linea.gameObject;
                respuestas[i] = TextoMedido(interior.transform, a[i], 20f * S, cabecera + 17f * S, faqAnchoResp, 17f * S, 1.5f, UIKit.Hex("4B637D"));
                Anim.RespuestaEntra(respuestas[i]);
            }

            // Consejitos
            var tips = UIKit.Box(content, UIKit.Hex("E9F7FF"), Mathf.RoundToInt(24f * S), "Consejos");
            var tipsSombra = UIKit.AddShadow(tips, Mathf.RoundToInt(24f * S), .10f, -5, 5).rectTransform;
            UIKit.Top(tips.rectTransform, 0, 0, cardW, 300f * S);
            var tipsCaja = UIKit.Box(tips.transform, UIKit.Blue, Mathf.RoundToInt(16f * S), "IconoCaja");
            UIKit.TopLeft(tipsCaja.rectTransform, 22f * S, 28f * S, 52f * S, 52f * S);
            var tipsIco = UIKit.Img(tipsCaja.transform, figInfo, "Icono"); tipsIco.color = Color.white; tipsIco.preserveAspect = true;
            UIKit.Stretch(tipsIco.rectTransform, 13.5f * S);
            float tipsTextoW = cardW - 108f * S;
            var tk = UIKit.Text(tips.transform, "PARA UNA MEJOR EXPERIENCIA", 11f * S, UIKit.Hex("3276B4"), TextAlignmentOptions.Left);
            tk.characterSpacing = 8f;
            UIKit.TopLeft(tk.rectTransform, 86f * S, 22f * S, tipsTextoW, 17f * S);
            var tt = UIKit.Text(tips.transform, "Consejitos para explorar", 23f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Interlinea(tt, 1.15f);
            UIKit.TopLeft(tt.rectTransform, 86f * S, 41f * S, tipsTextoW, 56f * S);
            string[] consejos = { "Busca un lugar con buena luz.", "Mantén el teléfono estable.", "Explora a tu ritmo." };
            Sprite[] consejoIco = { figStar, figCamera != null ? figCamera : icCamera, figTemas[7] };
            for (int i = 0; i < 3; i++)
            {
                var fila = UIKit.Box(tips.transform, Color.white, Mathf.RoundToInt(16f * S), "Consejo");
                UIKit.TopLeft(fila.rectTransform, 22f * S, (116f + i * 68f) * S, cardW - 44f * S, 58f * S);
                var c = UIKit.Box(fila.transform, UIKit.Hex("EAF5FF"), Mathf.RoundToInt(13f * S), "Caja");
                UIKit.TopLeft(c.rectTransform, 12f * S, 9f * S, 40f * S, 40f * S);
                var ci = UIKit.Img(c.transform, consejoIco[i], "Icono"); ci.color = UIKit.Blue; ci.preserveAspect = true;
                UIKit.Stretch(ci.rectTransform, 9f * S);
                var ct = UIKit.Text(fila.transform, consejos[i], 16f * S, UIKit.Hex("425E7A"), TextAlignmentOptions.Left);
                ct.fontWeight = FontWeight.Bold;
                UIKit.TopLeft(ct.rectTransform, 64f * S, 12f * S, cardW - 120f * S, 34f * S);
            }

            var botonBienvenida = BotonAyuda(welcome.transform, "ESCUCHAR BIENVENIDA", UIKit.Hex("E86E18"), Color.white, () => reproducir(0));
            UIKit.TopLeft(R(botonBienvenida), 23f * S, yBotonBienvenida, inW, 54f * S);
            var botonConsejos = BotonAyuda(tips.transform, "ESCUCHAR CONSEJOS", UIKit.Hex("BFE4FF"), UIKit.Hex("173D64"), () => reproducir(5));
            UIKit.TopLeft(R(botonConsejos), 22f * S, 340f * S, cardW - 44f * S, 54f * S);

            bool pausado = false;
            float altoPlayer = 66f * S;
            acomodar = () =>
            {
                float y = 232f * S;
                bool enBienvenida = sonando == 0, enConsejos = sonando == 5;
                float altoW = yBotonBienvenida + 54f * S + (enBienvenida ? 16f * S + altoPlayer : 0f) + 23f * S;
                welcome.rectTransform.sizeDelta = new Vector2(cardW, altoW);
                UIKit.AjustarSombra(welcomeSombra, welcome.rectTransform, -7);
                y += altoW + 26f * S;
                UIKit.Top(seccion.rectTransform, 0, y, cardW, 32f * S);
                y += 46f * S;
                for (int i = 0; i < 4; i++)
                {
                    bool abre = abierta == i, suena = sonando == i + 1;
                    float h = altosCabecera[i] + 4f * S;
                    float altoResp = respuestas[i].rectTransform.sizeDelta.y;
                    if (abre) h += 17f * S + altoResp + 19f * S + (suena ? 16f * S + altoPlayer : 0f);
                    UIKit.Top(tarjetas[i].rectTransform, 0, y, cardW, h);
                    UIKit.AjustarSombra(sombras[i], tarjetas[i].rectTransform, -5);
                    tarjetas[i].color = suena ? UIKit.Scan : abre ? UIKit.Hex("FFD5B6") : Color.white;
                    respuestas[i].gameObject.SetActive(abre);
                    lineas[i].SetActive(abre);
                    chevrones[i].rectTransform.localRotation = Quaternion.Euler(0, 0, abre ? -90f : 0f);
                    altavoces[i].color = suena ? UIKit.Hex("E86E18") : UIKit.Hex("FFF0E5");
                    altavozIcos[i].color = suena ? Color.white : UIKit.Hex("D76110");
                    y += h + 14f * S;
                }
                y += 14f * S;
                float altoTips = 416f * S + (enConsejos ? 16f * S + altoPlayer : 0f);
                UIKit.Top(tips.rectTransform, 0, y, cardW, altoTips);
                UIKit.AjustarSombra(tipsSombra, tips.rectTransform, -5);
                y += altoTips + 28f * S;
                content.sizeDelta = new Vector2(w, y);

                player.gameObject.SetActive(sonando >= 0);
                if (sonando >= 0)
                {
                    Transform padre = enBienvenida ? welcome.transform : enConsejos ? tips.transform : interiores[sonando - 1];
                    float x = enBienvenida ? 23f * S : enConsejos ? 22f * S : 16f * S;
                    float ancho = enBienvenida ? inW : enConsejos ? cardW - 44f * S : cardW - 36f * S;
                    float yP = enBienvenida ? yBotonBienvenida + 70f * S
                        : enConsejos ? 410f * S
                        : altosCabecera[sonando - 1] + 17f * S + respuestas[sonando - 1].rectTransform.sizeDelta.y + 16f * S;
                    player.transform.SetParent(padre, false);
                    UIKit.TopLeft(player.rectTransform, x, yP, ancho, altoPlayer);
                    UIKit.TopLeft(plTxt.rectTransform, 61f * S, 13f * S, ancho - 165f * S, 22f * S);
                    UIKit.TopLeft(R(plPausa), ancho - 100f * S, 11f * S, 44f * S, 44f * S);
                    UIKit.TopLeft(R(plStop), ancho - 52f * S, 11f * S, 44f * S, 44f * S);
                    plTxt.text = pausado ? "Explicación en pausa" : "Escuchando explicación";
                    plPausaIco.sprite = pausado ? figPlay : figPause;
                    ondas.SetActive(!pausado);
                }
            };

            System.Action detener = () =>
            {
                if (audioSrc != null) audioSrc.Stop();
                sonando = -1; pausado = false;
                acomodar();
            };
            plStop.onClick.AddListener(() => detener());
            plPausa.onClick.AddListener(() =>
            {
                pausado = !pausado;
                if (pausado) audioSrc.Pause(); else audioSrc.UnPause();
                acomodar();
            });
            reproducir = indice =>
            {
                if (AppState.Muted) { Toast("Audio silenciado (actívalo en Configuración)"); acomodar(); return; }
                if (audioSrc == null || ayudaClips == null || indice >= ayudaClips.Length || ayudaClips[indice] == null) { Toast("Sin audio disponible"); acomodar(); return; }
                audioSrc.Stop();
                audioSrc.clip = ayudaClips[indice];
                audioSrc.Play();
                sonando = indice; pausado = false;
                acomodar();
                StartCoroutine(EsperarFinAudio(() => pausado, () => { sonando = -1; acomodar(); }));
            };
            acomodar();
            BuildBottomNav(p, 3);
            return p;
        }

        Button BotonAyuda(Transform parent, string texto, Color fondo, Color tinta, UnityEngine.Events.UnityAction click)
        {
            const float S = 2.77f;
            var boton = UIKit.Button(parent, "", fondo, tinta, Mathf.RoundToInt(16f * S), click, 10);
            var t = UIKit.Text(boton.transform, texto, 15f * S, tinta, TextAlignmentOptions.Left);
            t.enableWordWrapping = false;
            UIKit.TamanoFijo(t, 15f * S);
            float anchoT = t.GetPreferredValues(texto).x + 4f, total = anchoT + 32f * S;
            var ico = UIKit.Img(boton.transform, figSpeaker, "Icono"); ico.color = tinta; ico.preserveAspect = true;
            Fila(ico.rectTransform, -total / 2f, 23f * S, 23f * S);
            Fila(t.rectTransform, -total / 2f + 32f * S, anchoT, 30f * S);
            return boton;
        }

        // Avisa al terminar el audio. Una pausa no cuenta como fin; si otro clip lo reemplazó, no avisa.
        IEnumerator EsperarFinAudio(System.Func<bool> pausado, System.Action alTerminar)
        {
            var clip = audioSrc.clip;
            while (audioSrc != null && audioSrc.clip == clip && (audioSrc.isPlaying || pausado()))
                yield return null;
            if (audioSrc != null && audioSrc.clip == clip && !audioSrc.isPlaying) alTerminar();
        }

        GameObject BuildSoundWaves(Transform parent, float x, float y, float w, float h)
        {
            var waves = UIKit.Node("SoundWaves", parent);
            waves.gameObject.SetActive(false);
            UIKit.TopLeft(waves, x, y, w, h);
            Color orange = UIKit.Hex("E86E18");
            float[] heights = { 5f, 10f, 7f, 11f };
            for (int i = 0; i < heights.Length; i++)
            {
                var bar = UIKit.Box(waves, orange, Mathf.RoundToInt(3f * 2.77f), "Wave" + i);
                UIKit.TopLeft(bar.rectTransform, i * 6f * 2.77f, (h / 2f) - heights[i] * 2.77f / 2f,
                    3f * 2.77f, heights[i] * 2.77f);
                var anim = Anim.Onda(bar);
                anim.delay = i * .2f;
            }
            return waves.gameObject;
        }
    }
}
