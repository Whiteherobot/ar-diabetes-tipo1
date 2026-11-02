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
        // 8 - CONFIGURACIÓN
        // ============================================================
        RectTransform BuildConfig()
        {
            var p = Panel("8_Config");
            const float S = 2.77f;
            bool land = Land;
            var viewport = UIKit.Node("ConfigViewport", p);
            viewport.anchorMin = Vector2.zero; viewport.anchorMax = Vector2.one;
            viewport.offsetMin = new Vector2(0, 82f * S); viewport.offsetMax = Vector2.zero;
            viewport.gameObject.AddComponent<RectMask2D>();
            var content = UIKit.Node("ConfigContent", viewport);
            content.anchorMin = new Vector2(.5f, 1); content.anchorMax = new Vector2(.5f, 1);
            content.pivot = new Vector2(.5f, 1); float w = land ? 1000 : 1080; content.sizeDelta = new Vector2(w, 1700f * S);
            var scroll = viewport.gameObject.AddComponent<ScrollRect>();
            scroll.content = content; scroll.viewport = viewport; scroll.horizontal = false; scroll.vertical = true;
            float cardW = w - 40f * S, y = 0;
            var hero = UIKit.Box(content, UIKit.Blue, Mathf.RoundToInt(0f), "SettingsHero"); UIKit.Top(hero.rectTransform, 0, y, w, 250f * S); hero.gameObject.AddComponent<Mask>();
            var kicker = UIKit.Text(hero.transform, "MI ESPACIO", 12f * S, Color.white, TextAlignmentOptions.Left); UIKit.TopLeft(kicker.rectTransform, 23f * S, 38f * S, 220f * S, 25f * S);
            var title = UIKit.Text(hero.transform, "¡Mi espacio!", 31f * S, Color.white, TextAlignmentOptions.Left); UIKit.TopLeft(title.rectTransform, 23f * S, 70f * S, 300f * S, 50f * S);
            var sub = UIKit.Text(hero.transform, "Personaliza tu aventura.", 17f * S, Color.white, TextAlignmentOptions.Left, FontStyles.Normal); UIKit.TopLeft(sub.rectTransform, 23f * S, 128f * S, 270f * S, 38f * S);
            var heroRobot = UIKit.Img(hero.transform, robotAzul, "Robot"); heroRobot.preserveAspect = true; UIKit.TopLeft(heroRobot.rectTransform, w - 155f * S, 75f * S, 130f * S, 160f * S);
            Anim.RobotEntra(heroRobot);
            y += 232f * S;
            var welcome = UIKit.Box(content, Color.white, Mathf.RoundToInt(24f * S), "SettingsWelcome"); UIKit.Top(welcome.rectTransform, 0, y, cardW, 155f * S); UIKit.AddShadow(welcome, Mathf.RoundToInt(24f * S), .09f, -6, 6);
            var welcomeRobotBg = UIKit.Box(welcome.transform, UIKit.Hex("EEEAFf"), Mathf.RoundToInt(20f * S), "RobotBg"); UIKit.TopLeft(welcomeRobotBg.rectTransform, 20f * S, 20f * S, 72f * S, 72f * S);
            var welcomeRobot = UIKit.Img(welcomeRobotBg.transform, robotAzul, "Robot"); welcomeRobot.preserveAspect = true; UIKit.Stretch(welcomeRobot.rectTransform, 8f * S);
            var welcomeKicker = UIKit.Text(welcome.transform, "TU AVENTURA, TUS ELECCIONES", 11f * S, UIKit.Fisio, TextAlignmentOptions.Left); UIKit.TopLeft(welcomeKicker.rectTransform, 110f * S, 24f * S, cardW - 130f * S, 24f * S);
            var welcomeTitle = UIKit.Text(welcome.transform, "¡Todo a tu gusto!", 23f * S, UIKit.Navy, TextAlignmentOptions.Left); UIKit.TopLeft(welcomeTitle.rectTransform, 110f * S, 50f * S, cardW - 130f * S, 34f * S);
            var welcomeBody = UIKit.Text(welcome.transform, "Elige cómo quieres aprender y explorar.", 15f * S, UIKit.Muted, TextAlignmentOptions.Left, FontStyles.Normal); UIKit.TopLeft(welcomeBody.rectTransform, 110f * S, 90f * S, cardW - 130f * S, 42f * S);
            y += 180f * S;
            var exp = UIKit.Box(content, Color.white, Mathf.RoundToInt(22f * S), "Experience"); UIKit.Top(exp.rectTransform, 0, y, cardW, 250f * S); UIKit.AddShadow(exp, Mathf.RoundToInt(22f * S), .08f, -5, 5);
            var expKicker = UIKit.Text(exp.transform, "PERSONALIZACIÓN", 11f * S, UIKit.Fisio, TextAlignmentOptions.Left); UIKit.TopLeft(expKicker.rectTransform, 22f * S, 22f * S, 300f * S, 24f * S);
            var expTitle = UIKit.Text(exp.transform, "Mi experiencia", 23f * S, UIKit.Navy, TextAlignmentOptions.Left); UIKit.TopLeft(expTitle.rectTransform, 22f * S, 47f * S, 300f * S, 34f * S);
            var sizeTitle = UIKit.Text(exp.transform, "Tamaño de letra", 17f * S, UIKit.Navy, TextAlignmentOptions.Left); UIKit.TopLeft(sizeTitle.rectTransform, 22f * S, 98f * S, 300f * S, 28f * S);
            var sizeSub = UIKit.Text(exp.transform, "Elige cómo quieres leer los textos.", 14f * S, UIKit.Muted, TextAlignmentOptions.Left, FontStyles.Normal); UIKit.TopLeft(sizeSub.rectTransform, 22f * S, 126f * S, 370f * S, 28f * S);
            string[] scaleLabels = { "Normal", "Grande", "Muy grande" };
            for (int i = 0; i < 3; i++)
            {
                int idx = i; float value = i == 0 ? 1f : (i == 1 ? 1.15f : 1.3f);
                bool selected = Mathf.Approximately(AppState.FontScale, value);
                var sb = UIKit.Button(exp.transform, scaleLabels[i], selected ? UIKit.Fisio : UIKit.Hex("F0F3F7"), selected ? Color.white : UIKit.Navy,
                    Mathf.RoundToInt(12f * S), () => { AppState.FontScale = value; BuildAll(); }, Mathf.RoundToInt(15f * S));
                UIKit.TopLeft(R(sb), (22f + i * 104f) * S, 172f * S, 96f * S, 46f * S);
            }
            y += 275f * S;
            var data = UIKit.Box(content, Color.white, Mathf.RoundToInt(22f * S), "Data"); UIKit.Top(data.rectTransform, 0, y, cardW, 220f * S); UIKit.AddShadow(data, Mathf.RoundToInt(22f * S), .08f, -5, 5);
            var dataKicker = UIKit.Text(data.transform, "ALMACENAMIENTO LOCAL", 11f * S, UIKit.Blue, TextAlignmentOptions.Left); UIKit.TopLeft(dataKicker.rectTransform, 22f * S, 22f * S, 360f * S, 24f * S);
            var dataTitle = UIKit.Text(data.transform, "Mis datos", 23f * S, UIKit.Navy, TextAlignmentOptions.Left); UIKit.TopLeft(dataTitle.rectTransform, 22f * S, 48f * S, 300f * S, 34f * S);
            var ageSub = ConfigOption(data.transform, 22f * S, 92f * S, "Cambiar perfil de edad", AppState.AgeLabel(AppState.Age), icProfile, UIKit.Blue, () => ShowOnly(pPerfil));
            var muteSub = ConfigOption(data.transform, 22f * S, 135f * S, "Sonido y narración", AppState.Muted ? "Silenciado" : "Activado", icAudio, UIKit.Nutri, () => { AppState.Muted = !AppState.Muted; if (bgAudioSrc != null) { if (AppState.Muted) bgAudioSrc.Pause(); else bgAudioSrc.UnPause(); } RefreshConfig(); });
            var resetModal = UIKit.Node("ResetModal", p);
            UIKit.Stretch(resetModal);
            var backdrop = UIKit.Box(resetModal, new Color(0.08f, 0.16f, 0.25f, .55f), 0, "Backdrop");
            UIKit.Stretch(backdrop.rectTransform);
            backdrop.raycastTarget = true;
            var confirmation = UIKit.Box(resetModal, Color.white, Mathf.RoundToInt(24f * S), "Confirmation");
            UIKit.Frac(confirmation.rectTransform, .07f, .29f, .93f, .71f);
            UIKit.AddShadow(confirmation, Mathf.RoundToInt(24f * S), .22f, -10, 12);
            Anim.Confirmacion(confirmation);
            var confirmationIcon = UIKit.Box(confirmation.transform, UIKit.Hex("FFF0F1"), Mathf.RoundToInt(20f * S), "WarningCircle");
            UIKit.Frac(confirmationIcon.rectTransform, .38f, .70f, .62f, .88f);
            var warning = UIKit.Img(confirmationIcon.transform, figWarning != null ? figWarning : icAlert, "Warning");
            warning.color = UIKit.Hex("C13C47"); warning.preserveAspect = true; UIKit.Stretch(warning.rectTransform, 12f * S);
            var confirmationTitle = UIKit.Text(confirmation.transform, "¿Reiniciar tu progreso?", 24f * S, UIKit.Navy, TextAlignmentOptions.Center);
            UIKit.Frac(confirmationTitle, .08f, .54f, .92f, .68f);
            var confirmationBody = UIKit.Text(confirmation.transform,
                "Se eliminarán tus estrellas y el avance de las misiones. Tus ajustes y fotografías permanecerán guardados.",
                16f * S, UIKit.Muted, TextAlignmentOptions.Center, FontStyles.Normal);
            UIKit.Frac(confirmationBody, .08f, .30f, .92f, .53f);
            var cancel = UIKit.Button(confirmation.transform, "Cancelar", UIKit.Hex("EDF2F6"), UIKit.Navy,
                Mathf.RoundToInt(15f * S), () => resetModal.gameObject.SetActive(false), Mathf.RoundToInt(15f * S));
            UIKit.Frac(R(cancel), .08f, .10f, .46f, .25f);
            var confirm = UIKit.Button(confirmation.transform, "Sí, reiniciar", UIKit.Hex("D8414D"), Color.white,
                Mathf.RoundToInt(15f * S), () => { resetModal.gameObject.SetActive(false); AppState.ResetProgress(); RefreshConfig(); RefreshProgreso(); Toast("Progreso reiniciado"); }, Mathf.RoundToInt(15f * S));
            UIKit.Frac(R(confirm), .54f, .10f, .92f, .25f);
            resetModal.gameObject.SetActive(false);
            var reset = UIKit.Button(data.transform, "REINICIAR MI PROGRESO", UIKit.Hex("D65D0D"), Color.white, Mathf.RoundToInt(13f * S),
                () => resetModal.gameObject.SetActive(true), Mathf.RoundToInt(12f * S));
            UIKit.TopLeft(R(reset), 22f * S, 175f * S, cardW - 44f * S, 38f * S);

            BuildBottomNav(p, 2);
            resetModal.SetAsLastSibling(); // el modal tapa también la barra inferior
            p.gameObject.AddComponent<ConfigRefs>().Set(ageSub, muteSub, resetModal);
            return p;
        }

        TMP_Text ConfigOption(Transform parent, float x, float y, string title, string sub, Sprite icon, Color color, UnityEngine.Events.UnityAction click)
        {
            var btn = UIKit.Button(parent, "", new Color(1, 1, 1, 0), UIKit.Navy, 10, click, 10, false);
            UIKit.TopLeft(R(btn), x, y, 480f, 38f);
            var image = UIKit.Img(btn.transform, icon, "Icon"); image.color = color; image.preserveAspect = true; UIKit.TopLeft(image.rectTransform, 0, 4f * 2.77f, 28f, 28f);
            var label = UIKit.Text(btn.transform, title + "\n<size=75%>" + sub + "</size>", 15f * 2.77f, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.TopLeft(label.rectTransform, 42f, 0, 430f, 38f);
            return label;
        }

        string FontScaleLabel(float scale)
        {
            return scale >= 1.25f ? "Muy grande" : (scale >= 1.1f ? "Grande" : "Normal");
        }

        // Fila de Configuración: círculo+icono, título, subtítulo (devuelve el subtítulo para refrescarlo).
        TMP_Text ConfigRow(RectTransform parent, int index, int total, float y0, float y1, string title, string sub,
                           Sprite icon, Color accent, UnityEngine.Events.UnityAction onClick)
        {
            var rc = UIKit.Cell(index, 1, total, 0.05f, y0, 0.95f, y1, 0f, 0.03f);
            var btn = UIKit.Button(parent, "", UIKit.Card, UIKit.Navy, 30, onClick);
            UIKit.Frac(R(btn), rc.xMin, rc.yMin, rc.xMax, rc.yMax);
            UIKit.AddShadow(btn.GetComponent<Image>(), 30, 0.08f, -4, 4);
            var disc = UIKit.Img(btn.transform, UIKit.Circle(), "Disc"); disc.color = accent;
            UIKit.Frac(disc, 0.03f, 0.2f, 0.20f, 0.8f);
            var ic = UIKit.Img(btn.transform, icon, "Ic"); ic.color = Color.white;
            UIKit.Frac(ic, 0.065f, 0.32f, 0.16f, 0.68f);
            var tl = UIKit.Text(btn.transform, title, 32, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Frac(tl, 0.26f, 0.5f, 0.94f, 0.86f);
            var sb = UIKit.Text(btn.transform, sub, 24, UIKit.Muted, TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Frac(sb, 0.26f, 0.14f, 0.94f, 0.48f);
            return sb;
        }

        void RefreshConfig()
        {
            if (pConfig == null) return;
            var refs = pConfig.GetComponent<ConfigRefs>();
            if (refs == null) return;
            if (refs.AgeSub != null) refs.AgeSub.text = AppState.AgeLabel(AppState.Age);
            if (refs.MuteSub != null) refs.MuteSub.text = AppState.Muted ? "Silenciado · toca para activar" : "Activado · toca para silenciar";
        }

        // ============================================================
        // 9 - AYUDA
        // ============================================================
        RectTransform BuildAyuda()
        {
            var p = Panel("9_Ayuda");
            const float S = 2.77f;
            bool land = Land;
            string[] q =
            {
                "¿Cómo escaneo una página?",
                "¿Cómo gano estrellas?",
                "La figura no aparece o se ve rara",
                "¿Se guarda mi progreso?"
            };
            string[] a =
            {
                "Apuntá la cámara del celular a la página impresa del libro, sin taparla, hasta que aparezca la figura en 3D.",
                "Leyendo cada tema, escuchando su narración y escaneando su página en AR. ¡Completar un libro entero te da un bono extra!",
                "Apoyá la hoja impresa sobre una mesa quieta y mové solo el celular despacio alrededor. Si se ve muy grande o rara, alejate un poco de la hoja.",
                "Sí, se guarda solo en este dispositivo. Podés reiniciarlo cuando quieras desde Configuración."
            };
            var viewport = UIKit.Node("HelpViewport", p);
            viewport.anchorMin = Vector2.zero; viewport.anchorMax = Vector2.one;
            viewport.offsetMin = new Vector2(0, 82f * S); viewport.offsetMax = new Vector2(0, -90f * S);
            viewport.gameObject.AddComponent<RectMask2D>();
            var vi = viewport.gameObject.AddComponent<Image>(); vi.color = new Color(1, 1, 1, 0);
            var content = UIKit.Node("HelpContent", viewport);
            content.anchorMin = new Vector2(.5f, 1); content.anchorMax = new Vector2(.5f, 1);
            content.pivot = new Vector2(.5f, 1); float w = land ? 1000 : 1080; content.sizeDelta = new Vector2(w, 1900f * S);
            var scroll = viewport.gameObject.AddComponent<ScrollRect>();
            scroll.content = content; scroll.viewport = viewport; scroll.horizontal = false; scroll.vertical = true;
            float y = 0f;
            var hero = UIKit.Box(content, UIKit.Scan, Mathf.RoundToInt(28f * S), "HelpHero");
            UIKit.Top(hero.rectTransform, 0, y, w, 250f * S); hero.gameObject.AddComponent<Mask>();
            var heroKicker = UIKit.Text(hero.transform, "¡ESTAMOS AQUÍ PARA AYUDARTE!", 12f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.TopLeft(heroKicker.rectTransform, 22f * S, 38f * S, 390f * S, 26f * S);
            var heroTitle = UIKit.Text(hero.transform, "¿Necesitas ayuda?", 28f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.TopLeft(heroTitle.rectTransform, 22f * S, 70f * S, 270f * S, 48f * S);
            var heroSub = UIKit.Text(hero.transform, "¡Te explicamos cómo funciona todo!", 16f * S, Color.white, TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.TopLeft(heroSub.rectTransform, 22f * S, 125f * S, 250f * S, 55f * S);
            var heroRobot = UIKit.Img(hero.transform, robotCoral, "Robot"); heroRobot.preserveAspect = true;
            UIKit.TopLeft(heroRobot.rectTransform, w - 150f * S, 78f * S, 125f * S, 155f * S);
            Anim.RobotEntra(heroRobot);
            y += 270f * S;
            var welcome = UIKit.Box(content, Color.white, Mathf.RoundToInt(22f * S), "Welcome");
            float welcomeW = w - 40f * S;
            UIKit.Top(welcome.rectTransform, 0, y, welcomeW, 245f * S); UIKit.AddShadow(welcome, Mathf.RoundToInt(22f * S), .09f, -6, 6);
            var welcomeAvatar = UIKit.Box(welcome.transform, UIKit.Hex("E6F3FF"), Mathf.RoundToInt(19f * S), "Avatar");
            UIKit.TopLeft(welcomeAvatar.rectTransform, 22f * S, 20f * S, 64f * S, 64f * S);
            var welcomeRobot = UIKit.Img(welcomeAvatar.transform, robotAzul, "Robot"); welcomeRobot.preserveAspect = true; UIKit.Stretch(welcomeRobot.rectTransform, 8f * S);
            var welcomeTitle = UIKit.Text(welcome.transform, "¡Hola, explorador!", 22f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.TopLeft(welcomeTitle.rectTransform, 100f * S, 27f * S, welcomeW - 125f * S, 42f * S);
            var welcomeBody = UIKit.Text(welcome.transform, "Lee las respuestas o toca el altavoz cuando quieras.", 15f * S, UIKit.Muted, TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.TopLeft(welcomeBody.rectTransform, 22f * S, 98f * S, welcomeW - 44f * S, 45f * S);
            var welcomeAudio = UIKit.Button(welcome.transform, "ESCUCHAR BIENVENIDA", UIKit.Hex("E86E18"), Color.white, Mathf.RoundToInt(13f * S), null, Mathf.RoundToInt(15f * S));
            UIKit.TopLeft(R(welcomeAudio), 22f * S, 157f * S, welcomeW - 44f * S, 54f * S);
            var welcomeAudioIcon = UIKit.Img(welcomeAudio.transform, icAudio, "Audio"); welcomeAudioIcon.color = Color.white; welcomeAudioIcon.preserveAspect = true; UIKit.TopLeft(welcomeAudioIcon.rectTransform, 18f * S, 16f * S, 23f * S, 23f * S);
            var welcomeWaves = BuildSoundWaves(welcomeAudio.transform, welcomeW - 82f * S, 20f * S, 44f * S, 16f * S);
            welcomeAudio.onClick.AddListener(() => PlayHelpAudio(welcomeWaves));
            y += 270f * S;
            var section = UIKit.Text(content, "Preguntas frecuentes", 23f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Top(section.rectTransform, 0, y, w - 40f * S, 40f * S); y += 52f * S;
            for (int i = 0; i < 4; i++)
            {
                int idx = i;
                var card = UIKit.Box(content, UIKit.Card, Mathf.RoundToInt(18f * S), "Faq" + i);
                UIKit.Top(card.rectTransform, 0, y, w - 40f * S, 118f * S); UIKit.AddShadow(card, Mathf.RoundToInt(18f * S), .08f, -4, 4);
                var icon = UIKit.Img(card.transform, i == 0 ? figScan : (i == 1 ? figStar : (i == 2 ? icCamera : icChart)), "Icon");
                icon.color = UIKit.Scan; icon.preserveAspect = true; UIKit.TopLeft(icon.rectTransform, 18f * S, 28f * S, 42f * S, 42f * S);
                var qt = UIKit.Text(card.transform, q[i], 17f * S, UIKit.Navy, TextAlignmentOptions.Left);
                UIKit.TopLeft(qt.rectTransform, 78f * S, 20f * S, w - 170f * S, 42f * S);
                var at = UIKit.Text(card.transform, a[i], 15f * S, UIKit.Muted, TextAlignmentOptions.Left, FontStyles.Normal);
                UIKit.TopLeft(at.rectTransform, 78f * S, 66f * S, w - 170f * S, 42f * S);
                Anim.RespuestaEntra(at);
                var toggle = UIKit.Button(card.transform, "", new Color(1, 1, 1, 0), UIKit.Navy, 0, null, 10, false);
                UIKit.TopLeft(R(toggle), 70f * S, 0, w - 150f * S, 118f * S);
                toggle.onClick.AddListener(() => at.gameObject.SetActive(!at.gameObject.activeSelf));
                var audio = UIKit.Button(card.transform, "", new Color(1, 1, 1, 0), UIKit.Navy, 10, null, 10, false);
                UIKit.TopLeft(R(audio), w - 78f * S, 24f * S, 48f * S, 48f * S);
                var audioIcon = UIKit.Img(audio.transform, icAudio, "Audio"); audioIcon.color = UIKit.Scan; audioIcon.preserveAspect = true; UIKit.Stretch(audioIcon.rectTransform, 10);
                var waves = BuildSoundWaves(audio.transform, -18f * S, 17f * S, 42f * S, 14f * S);
                if (i > 0) at.gameObject.SetActive(false);
                audio.onClick.AddListener(() => at.gameObject.SetActive(!at.gameObject.activeSelf));
                audio.onClick.AddListener(() => PlayHelpAudio(waves));
                y += 135f * S;
            }
            BuildBottomNav(p, 3);
            return p;
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

        void PlayHelpAudio(GameObject waves)
        {
            PlayNarration();
            if (waves != null && audioSrc != null && audioSrc.isPlaying)
                StartCoroutine(ShowSoundWaves(waves));
        }

        IEnumerator ShowSoundWaves(GameObject waves)
        {
            waves.SetActive(true);
            while (audioSrc != null && audioSrc.isPlaying)
                yield return null;
            waves.SetActive(false);
        }
    }
}
