using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    // Pantallas 1) INICIO 2) BIENVENIDA (carrusel) 3) PERFIL 4) MENÚ.
    public partial class AppBootstrap
    {
        // ============================================================
        // 1 - INICIO
        // ============================================================
        RectTransform BuildInicio()
        {
            var p = Panel("1_Inicio");
            const float S = 2.77f;
            var group = BuildRobotGroup(p);
            UIKit.Top(group, 0f, Land ? 300f * S : 180f * S, 280f * S, 188f * S);
            var t1 = UIKit.Text(p, "Diabetes Tipo 1", 36f * S, UIKit.Navy);
            var t2 = UIKit.Text(p, "Aprendo, entiendo y me cuido", 17f * S, UIKit.Hex("55708F"), TextAlignmentOptions.Center, FontStyles.Normal);
            var b = UIKit.Button(p, "COMENZAR", UIKit.Blue, Color.white, Mathf.RoundToInt(17f * S), () => ShowOnly(pBienvenida), Mathf.RoundToInt(17f * S), true);

            if (!Land)
            {
                UIKit.Frac(t1.rectTransform, .06f, .39f, .94f, .47f);
                UIKit.Frac(t2.rectTransform, .08f, .34f, .92f, .40f);
                UIKit.Frac(R(b), .05f, .08f, .95f, .145f);
            }
            else
            {
                UIKit.Frac(group, .25f, .34f, .75f, .70f);
                UIKit.Frac(t1.rectTransform, .08f, .30f, .92f, .39f);
                UIKit.Frac(t2.rectTransform, .14f, .23f, .86f, .30f);
                UIKit.Frac(R(b), .34f, .07f, .66f, .17f);
            }
            Anim.Flotar(group);
            UIKit.AddShadow(b.GetComponent<Image>(), 40, 0.18f, -10, 6);
            return p;
        }

        // ============================================================
        // 2 - BIENVENIDA (carrusel)
        // ============================================================
        RectTransform BuildBienvenida()
        {
            var p = Panel("2_Bienvenida");
            const float S = 2.77f;
            bool land = Land;

            var card = UIKit.Box(p, UIKit.Card, 48, "Card");
            if (!land) UIKit.Frac(card.rectTransform, 0.04f, 0.20f, 0.96f, 0.97f);
            else UIKit.Frac(card.rectTransform, 0.07f, 0.26f, 0.93f, 0.84f);
            UIKit.AddShadow(card, 48, 0.12f, -10, 10);

            var viewport = UIKit.Node("Viewport", card.transform);
            UIKit.Frac(viewport, 0.04f, 0.14f, 0.96f, 0.97f);
            viewport.gameObject.AddComponent<RectMask2D>();
            var catcher = viewport.gameObject.AddComponent<Image>();
            catcher.color = new Color(1, 1, 1, 0); catcher.raycastTarget = true;

            Canvas.ForceUpdateCanvases();
            float w = viewport.rect.width, h = viewport.rect.height;
            if (w < 10) { w = (land ? Screen.height : Screen.width) * 0.8f; h = w; }

            var content = UIKit.Node("Content", viewport);
            content.anchorMin = content.anchorMax = new Vector2(0, 0.5f);
            content.pivot = new Vector2(0, 0.5f);
            content.sizeDelta = new Vector2(w * 3, h);
            content.anchoredPosition = Vector2.zero;

            string[] titles = { "Explora 3 libros", "Escanea y juega", "Aprende a cuidarte" };
            string[] bodies =
            {
                "Descubre tu cuerpo, la alimentación y el cuidado diario con modelos 3D y realidad aumentada.",
                "Apunta la cámara a las páginas de tus libros para ver increíbles modelos en realidad aumentada.",
                "Completa juegos, gana estrellas y aprende cada día algo nuevo sobre la diabetes tipo 1."
            };
            Sprite[] imgs = { robotCoral, figScan != null ? figScan : spIconScan, robotVerde };
            for (int i = 0; i < 3; i++)
            {
                var slide = UIKit.Node("Slide" + i, content);
                slide.anchorMin = slide.anchorMax = new Vector2(0, 0.5f);
                slide.pivot = new Vector2(0, 0.5f);
                slide.sizeDelta = new Vector2(w, h);
                slide.anchoredPosition = new Vector2(i * w, 0);

                var tt = UIKit.Text(slide, titles[i], 78, UIKit.Navy);
                UIKit.Frac(tt, 0.05f, 0.82f, 0.95f, 0.98f);
                var bd = UIKit.Text(slide, bodies[i], 44, UIKit.Muted, TextAlignmentOptions.Center, FontStyles.Normal);
                UIKit.Frac(bd, land ? 0.06f : 0.08f, 0.52f, land ? 0.94f : 0.92f, 0.80f);
                if (i == 0)
                {
                    var trio = BuildRobotGroup(slide);
                    UIKit.Frac(trio, .08f, .04f, .92f, .52f);
                }
                else
                {
                    var im = UIKit.Img(slide, imgs[i], "Img"); im.preserveAspect = true;
                    UIKit.Frac(im, 0.30f, 0.05f, 0.70f, 0.50f);
                }
            }

            var dots = new Image[3];
            for (int i = 0; i < 3; i++)
            {
                var d = UIKit.Img(card.transform, UIKit.Circle(), "Dot" + i);
                d.preserveAspect = true;
                d.color = i == 0 ? UIKit.Blue : UIKit.Hex("C4D3E0");
                float cx = 0.5f + (i - 1) * 0.045f;
                UIKit.Frac(d, cx - 0.012f, 0.045f, cx + 0.012f, 0.095f);
                dots[i] = d;
            }

            var siguiente = UIKit.Button(p, "SIGUIENTE", UIKit.Blue, Color.white, 40, null, Mathf.RoundToInt(17f * S));
            var omitir = UIKit.Button(p, "Omitir", new Color(0, 0, 0, 0), UIKit.Muted, 20, () => ShowOnly(pPerfil), Mathf.RoundToInt(16f * S), false);
            if (!land)
            {
                UIKit.Frac(R(siguiente), 0.18f, 0.10f, 0.82f, 0.175f);
                UIKit.Frac(R(omitir), 0.30f, 0.05f, 0.70f, 0.095f);
            }
            else
            {
                UIKit.Frac(R(siguiente), 0.34f, 0.10f, 0.66f, 0.19f);
                UIKit.Frac(R(omitir), 0.40f, 0.03f, 0.60f, 0.09f);
            }
            UIKit.AddShadow(siguiente.GetComponent<Image>(), 40, 0.18f, -10, 6);
            var sigLabel = siguiente.GetComponentInChildren<TMP_Text>();

            var carousel = viewport.gameObject.AddComponent<Carousel>();
            carousel.content = content;
            carousel.slideWidth = w;
            carousel.count = 3;
            carousel.dots = dots;
            carousel.dotOn = UIKit.Blue;
            carousel.dotOff = UIKit.Hex("C4D3E0");
            carousel.onComplete = () => ShowOnly(pPerfil);
            carousel.onIndexChanged = idx => { if (sigLabel != null) sigLabel.text = idx == 2 ? "EMPEZAR" : "SIGUIENTE"; };
            siguiente.onClick.AddListener(() => carousel.Next());
            return p;
        }

        // ============================================================
        // 3 - PERFIL
        // ============================================================
        RectTransform BuildPerfil()
        {
            var p = Panel("3_Perfil");
            const float S = 2.77f;
            bool land = Land;

            var t = UIKit.Text(p, "Crea tu explorador", 31f * S, UIKit.Navy);
            var sub = UIKit.Text(p, "Escribe tu nombre y elige tu personaje", 16f * S, UIKit.Muted, TextAlignmentOptions.Center, FontStyles.Normal);
            if (!land)
            {
                UIKit.Frac(t, 0.05f, 0.87f, 0.95f, 0.94f);
                UIKit.Frac(sub, 0.08f, 0.82f, 0.92f, 0.865f);
            }
            else
            {
                UIKit.Frac(t, 0.05f, 0.85f, 0.95f, 0.96f);
                UIKit.Frac(sub, 0.08f, 0.77f, 0.92f, 0.84f);
            }

            var campo = BuildCampoNombre(p);
            if (!land) UIKit.Frac(campo, 0.05f, 0.71f, 0.95f, 0.79f);
            else UIKit.Frac(campo, 0.25f, 0.60f, 0.75f, 0.74f);

            var lista = Personajes;
            int cols = land ? 6 : 3, rows = land ? 1 : 2;
            float x0 = 0.05f, x1 = 0.95f, y0 = land ? 0.17f : 0.19f, y1 = land ? 0.56f : 0.67f;
            float gx = land ? 0.015f : 0.03f, gy = 0.025f;
            var marcos = new GameObject[lista.Length];
            for (int i = 0; i < lista.Length; i++)
            {
                var rc = UIKit.Cell(i, cols, rows, x0, y0, x1, y1, gx, gy);
                marcos[i] = PersonajeOption(p, rc, lista[i], i, marcos);
            }
            for (int i = 0; i < marcos.Length; i++) marcos[i].SetActive(i == AppState.Character);

            var continueButton = UIKit.Button(p, "CONTINUAR", UIKit.Blue, Color.white, Mathf.RoundToInt(17f * S),
                () => ShowOnly(pMenu), Mathf.RoundToInt(17f * S), true);
            UIKit.Frac(R(continueButton), land ? .34f : .05f, .035f, land ? .66f : .95f, .115f);
            UIKit.AddShadow(continueButton.GetComponent<Image>(), Mathf.RoundToInt(17f * 2.77f), .14f, -6f, 6f);
            return p;
        }

        RectTransform BuildCampoNombre(RectTransform parent)
        {
            const float S = 2.77f;
            int radio = Mathf.RoundToInt(17f * S);
            var fondo = UIKit.Box(parent, Color.white, radio, "CampoNombre");
            fondo.raycastTarget = true;
            UIKit.AddShadow(fondo, radio, .10f, -5f, 5f);

            var area = UIKit.Node("Area", fondo.transform);
            UIKit.Stretch(area, 18f * S);
            area.gameObject.AddComponent<RectMask2D>();

            var texto = UIKit.Text(area, "", 20f * S, UIKit.Navy, TextAlignmentOptions.Left, FontStyles.Normal);
            var aviso = UIKit.Text(area, "Tu nombre", 20f * S, UIKit.Muted, TextAlignmentOptions.Left, FontStyles.Normal);
            foreach (var t in new[] { texto, aviso })
            {
                UIKit.Stretch(t.rectTransform);
                t.enableAutoSizing = false;
                t.fontSize = 20f * S;
                t.enableWordWrapping = false;
                t.richText = false;
            }

            var campo = fondo.gameObject.AddComponent<TMP_InputField>();
            campo.targetGraphic = fondo;
            campo.textViewport = area;
            campo.textComponent = texto;
            campo.placeholder = aviso;
            campo.fontAsset = UIKit.Font;
            campo.pointSize = 20f * S;
            campo.richText = false;
            campo.lineType = TMP_InputField.LineType.SingleLine;
            campo.contentType = TMP_InputField.ContentType.Name;
            campo.characterLimit = 14;
            campo.SetTextWithoutNotify(AppState.PlayerName);
            campo.onValueChanged.AddListener(v => AppState.PlayerName = v);
            return fondo.rectTransform;
        }

        // Devuelve el marco de selección para encenderlo solo en el personaje elegido.
        GameObject PersonajeOption(RectTransform parent, Rect rc, Sprite robot, int indice, GameObject[] marcos)
        {
            const float S = 2.77f;
            int radio = Mathf.RoundToInt(13f * S);
            var btn = UIKit.Button(parent, "", UIKit.Card, UIKit.Navy, radio, () =>
            {
                AppState.Character = indice;
                for (int i = 0; i < marcos.Length; i++) marcos[i].SetActive(i == indice);
            });
            UIKit.Frac(R(btn), rc.xMin, rc.yMin, rc.xMax, rc.yMax);
            UIKit.AddShadow(btn.GetComponent<Image>(), radio, 0.10f, -8, 8);
            btn.gameObject.AddComponent<Appear>().delay = 0.04f * indice;

            var marco = UIKit.Box(btn.transform, UIKit.Blue, radio, "Marco");
            UIKit.Stretch(marco.rectTransform);
            var interior = UIKit.Box(marco.transform, UIKit.Hex("E6F3FF"), Mathf.RoundToInt(10f * S), "Interior");
            UIKit.Stretch(interior.rectTransform, 3f * S);

            var img = UIKit.Img(btn.transform, robot, "Robot");
            img.preserveAspect = true;
            UIKit.Stretch(img.rectTransform, 10f * S);
            return marco.gameObject;
        }

        // ============================================================
        // 4 - MENÚ PRINCIPAL
        // ============================================================
        RectTransform BuildMenu()
        {
            var p = Panel("4_Menu");
            const float S = 2.77f;
            bool land = Land;
            float columnW = land ? 1000f : 1080f;
            float side = 20f * S;
            float y = 24f * S;

            var viewport = UIKit.Node("MenuViewport", p);
            var vrt = viewport;
            vrt.anchorMin = new Vector2(0, 0); vrt.anchorMax = new Vector2(1, 1);
            vrt.offsetMin = new Vector2(0, 82f * 2.77f); vrt.offsetMax = Vector2.zero;
            viewport.gameObject.AddComponent<RectMask2D>();
            var viewportImage = viewport.gameObject.AddComponent<Image>();
            viewportImage.color = new Color(1, 1, 1, 0);
            viewportImage.raycastTarget = true;

            var content = UIKit.Node("MenuContent", viewport);
            content.anchorMin = new Vector2(0.5f, 1); content.anchorMax = new Vector2(0.5f, 1);
            content.pivot = new Vector2(0.5f, 1); content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(columnW, 1);

            menuScroll = viewport.gameObject.AddComponent<ScrollRect>();
            menuScroll.content = content;
            menuScroll.viewport = viewport;
            menuScroll.horizontal = false;
            menuScroll.vertical = true;
            menuScroll.movementType = ScrollRect.MovementType.Elastic;
            menuScroll.inertia = true;
            menuScroll.scrollSensitivity = 3f * S;

            // ---- Hero (solo redondeado abajo: el recorte oculta las esquinas superiores) ----
            float heroW = columnW - 32f * S, heroH = 326f * S, T = 32f * S;
            var heroClip = UIKit.Node("explorer-hero", content);
            UIKit.Top(heroClip, 0, y, heroW, heroH);
            heroClip.gameObject.AddComponent<RectMask2D>();
            heroClip.gameObject.AddComponent<Appear>().delay = 0f;
            var hero = UIKit.Box(heroClip, UIKit.Hex("DFF2FF"), Mathf.RoundToInt(32f * S), "HeroFondo");
            UIKit.TopLeft(hero.rectTransform, 0, -T, heroW, heroH + T);
            hero.gameObject.AddComponent<Mask>();
            var heroBlob1 = UIKit.Img(hero.transform, UIKit.Circle(), "BlobTop");
            heroBlob1.color = new Color(131f / 255f, 112f / 255f, 218f / 255f, .12f);
            UIKit.TopLeft(heroBlob1.rectTransform, heroW - 125f * S, T - 70f * S, 180f * S, 180f * S);
            var heroBlob2 = UIKit.Img(hero.transform, UIKit.Circle(), "BlobBottom");
            heroBlob2.color = new Color(47f / 255f, 174f / 255f, 117f / 255f, .13f);
            UIKit.TopLeft(heroBlob2.rectTransform, -65f * S, T + 245f * S, 135f * S, 135f * S);

            var heroRobots = BuildRobotGroup(hero.transform, PersonajeElegido());
            UIKit.TopLeft(heroRobots, heroW - 249f * S, T + 162f * S, 252f * S, 169f * S);
            Anim.HeroLlega(heroRobots);

            var heroTitle = UIKit.Text(hero.transform, AppState.Greeting, 31f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Interlinea(heroTitle, 1.1f);
            UIKit.TopLeft(heroTitle.rectTransform, 22f * S, T + 24f * S, 240f * S, 72f * S);
            holaText = heroTitle;
            // Con un nombre corto el saludo cabe en una línea y lo demás sube, como un bloque en CSS
            heroTitle.enableAutoSizing = false;
            heroTitle.fontSize = 31f * S * UIKit.TextScale;
            float sube = heroTitle.GetPreferredValues(heroTitle.text).x <= 240f * S ? 34f * S : 0f;
            heroTitle.enableAutoSizing = true;
            var heroSub = UIKit.Text(hero.transform, "¿Qué descubriremos hoy?", 17f * S, UIKit.Hex("4D6784"), TextAlignmentOptions.Left, FontStyles.Normal);
            heroSub.fontWeight = FontWeight.SemiBold;
            UIKit.TopLeft(heroSub.rectTransform, 22f * S, T + 99f * S - sube, heroW - 44f * S, 26f * S);

            var levelPill = UIKit.Box(hero.transform, Color.white, Mathf.RoundToInt(21f * S), "LevelPill");
            UIKit.TopLeft(levelPill.rectTransform, 22f * S, T + 138f * S - sube, 82f * S, 42f * S);
            UIKit.AddShadow(levelPill, Mathf.RoundToInt(21f * S), .08f, -3f, 4f);
            nivelText = UIKit.Text(levelPill.transform, "Nivel " + AppState.Level, 15f * S, UIKit.Blue, TextAlignmentOptions.Center);
            UIKit.Stretch(nivelText.rectTransform);
            var starsPill = UIKit.Box(hero.transform, UIKit.Hex("FFF5CE"), Mathf.RoundToInt(21f * S), "StarsPill");
            UIKit.TopLeft(starsPill.rectTransform, 113f * S, T + 138f * S - sube, 74f * S, 42f * S);
            UIKit.AddShadow(starsPill, Mathf.RoundToInt(21f * S), .08f, -3f, 4f);
            var starIcon = UIKit.Img(starsPill.transform, figStar, "Star");
            starIcon.color = UIKit.Hex("FFB830"); starIcon.preserveAspect = true;
            UIKit.TopLeft(starIcon.rectTransform, 10f * S, 9.5f * S, 23f * S, 23f * S);
            starLabel = UIKit.Text(starsPill.transform, AppState.Stars.ToString(), 17f * S, UIKit.Hex("654D13"), TextAlignmentOptions.Left);
            UIKit.TopLeft(starLabel.rectTransform, 38f * S, 6f * S, 34f * S, 30f * S);
            y += heroH + 22f * S;

            // ---- Explora ----
            float featureW = columnW - 40f * S, featureH = 226f * S;
            var explore = UIKit.Box(content, UIKit.Navy, Mathf.RoundToInt(26f * S), "explore-feature");
            UIKit.Top(explore.rectTransform, 0, y, featureW, featureH);
            UIKit.AddShadow(explore, Mathf.RoundToInt(26f * S), .16f, -8f, 8f);
            explore.gameObject.AddComponent<Appear>().delay = .05f;
            explore.gameObject.AddComponent<Mask>();
            var exploreDecor = UIKit.Img(explore.transform, UIKit.Circle(), "Decor");
            exploreDecor.color = new Color(1, 1, 1, .08f); exploreDecor.preserveAspect = true;
            UIKit.TopLeft(exploreDecor.rectTransform, featureW - 88f * S, featureH - 70f * S, 150f * S, 150f * S);
            var exploreCircle = UIKit.Img(explore.transform, UIKit.Circle(), "StarDisc");
            exploreCircle.color = new Color(1, 1, 1, .11f); exploreCircle.preserveAspect = true;
            UIKit.TopLeft(exploreCircle.rectTransform, featureW - 81f * S, 28f * S, 54f * S, 54f * S);
            Anim.Deriva(exploreCircle);
            var exploreStar = UIKit.Img(exploreCircle.transform, figStar, "Star");
            exploreStar.color = UIKit.Hex("FFB830"); exploreStar.preserveAspect = true; UIKit.Stretch(exploreStar.rectTransform, 12f * S);
            var kicker = UIKit.Text(explore.transform, "EXPLORA, DESCUBRE Y APRENDE", 12f * S, UIKit.Hex("A9D8FF"), TextAlignmentOptions.Left);
            kicker.characterSpacing = 10f;
            UIKit.TopLeft(kicker.rectTransform, 25f * S, 25f * S, 262f * S, 18f * S);
            var exploreTitle = UIKit.Text(explore.transform, "¡Comienza a explorar!", 26f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.TopLeft(exploreTitle.rectTransform, 25f * S, 47f * S, featureW - 50f * S, 34f * S);
            var exploreBody = UIKit.Text(explore.transform, "Viaja por tres mundos y descubre cómo cuidar tu cuerpo cada día.", 16f * S, UIKit.Hex("DCECFF"), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Interlinea(exploreBody, 1.45f);
            UIKit.TopLeft(exploreBody.rectTransform, 25f * S, 85f * S, 285f * S, 50f * S);
            var exploreButton = UIKit.Button(explore.transform, "", Color.white, UIKit.Navy, Mathf.RoundToInt(16f * S), () => ShowOnly(pMundos), 15, true);
            UIKit.TopLeft(R(exploreButton), 25f * S, 150f * S, 203f * S, 52f * S);
            var exploreButtonText = UIKit.Text(exploreButton.transform, "EXPLORAR AHORA", 15f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Frac(exploreButtonText, .084f, .12f, .84f, .88f);
            exploreButtonText.enableWordWrapping = false;
            var exploreChevron = UIKit.Img(exploreButton.transform, figChevron, "Chevron");
            exploreChevron.color = UIKit.Navy; exploreChevron.preserveAspect = true; UIKit.Frac(exploreChevron, .80f, .29f, .92f, .71f);
            y += featureH + 28f * S;

            // ---- Aventuras ----
            var adventureTitle = UIKit.Text(content, "Elige tu aventura", 23f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Top(adventureTitle.rectTransform, 0, y, columnW - 40f * S, 32f * S);
            adventureTitle.gameObject.AddComponent<Appear>().delay = .10f;
            y += 32f * S + 16f * S;
            string[] adventureNames = { "Mundo Fisiológico", "Mundo Nutricional", "Mundo Clínico" };
            string[] adventureSubs = { "Descubre cómo funciona tu cuerpo", "Aprende sobre tus alimentos", "Conoce tus cuidados" };
            Color[] adventureColors = { UIKit.Coral, UIKit.Nutri, UIKit.Clin };
            Sprite[] adventureRobots = { robotCoral, robotVerde, robotAzul };
            for (int i = 0; i < 3; i++)
            {
                BuildAdventureCard(content, i, y, columnW - 40f * S, 196f * S, adventureNames[i], adventureSubs[i], adventureColors[i], adventureRobots[i]);
                y += 196f * S + 18f * S;
            }
            y -= 18f * S;
            y += 30f * S;

            // ---- Escanear ----
            float scanW = columnW - 40f * S, scanH = 222f * S, scanX = 113f * S, scanTextW = scanW - scanX - 20f * S;
            var scan = UIKit.Box(content, UIKit.Hex("FFF0E6"), Mathf.RoundToInt(25f * S), "scan-feature");
            UIKit.Top(scan.rectTransform, 0, y, scanW, scanH);
            UIKit.AddShadow(scan, Mathf.RoundToInt(25f * S), .10f, -6f, 7f);
            scan.gameObject.AddComponent<Appear>().delay = .30f;
            var scanIconBox = UIKit.Box(scan.transform, UIKit.Scan, Mathf.RoundToInt(22f * S), "ScanIcon");
            UIKit.TopLeft(scanIconBox.rectTransform, 20f * S, 22f * S, 72f * S, 72f * S);
            scanIconBox.rectTransform.localRotation = Quaternion.Euler(0, 0, 4f);
            var scanIcon = UIKit.Img(scanIconBox.transform, figScan, "Icon");
            scanIcon.color = Color.white; scanIcon.preserveAspect = true; UIKit.Stretch(scanIcon.rectTransform, 12f * S);
            var scanKicker = UIKit.Text(scan.transform, "REALIDAD AUMENTADA", 12f * S, UIKit.Hex("A35422"), TextAlignmentOptions.Left);
            scanKicker.characterSpacing = 7.5f;
            UIKit.TopLeft(scanKicker.rectTransform, scanX, 22f * S, scanTextW, 18f * S);
            var scanTitle = UIKit.Text(scan.transform, "¡Haz que tu libro cobre vida!", 22f * S, UIKit.Hex("663112"), TextAlignmentOptions.Left);
            UIKit.Interlinea(scanTitle, 1.15f);
            UIKit.TopLeft(scanTitle.rectTransform, scanX, 42f * S, scanTextW, 54f * S);
            var scanBody = UIKit.Text(scan.transform, "Escanea una página y descubre su contenido en 3D.", 15f * S, UIKit.Hex("764726"), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Interlinea(scanBody, 1.4f);
            UIKit.TopLeft(scanBody.rectTransform, scanX, 98f * S, scanTextW, 46f * S);
            var scanButton = UIKit.Button(scan.transform, "ESCANEAR PÁGINA", UIKit.Hex("D65D0D"), Color.white, Mathf.RoundToInt(14f * S), () => OnMenuTile(3, "Escanear página"), Mathf.RoundToInt(14f * S), true);
            UIKit.TopLeft(R(scanButton), scanX, 153f * S, 165f * S, 47f * S);
            y += scanH + 20f * S;

            // ---- Jugar ----
            float playW = columnW - 40f * S, playH = 98f * S;
            var play = UIKit.Button(content, "", UIKit.Juegos, Color.white, Mathf.RoundToInt(24f * S), () => OnMenuTile(4, "Jugar y aprender"), 19, true);
            UIKit.Top(R(play), 0, y, playW, playH);
            UIKit.AddShadow(play.GetComponent<Image>(), Mathf.RoundToInt(24f * S), .16f, -7f, 8f);
            play.gameObject.AddComponent<Appear>().delay = .35f;
            var playIconBox = UIKit.Box(play.transform, new Color(1, 1, 1, .18f), Mathf.RoundToInt(18f * S), "GameIcon");
            UIKit.TopLeft(playIconBox.rectTransform, 17f * S, 20f * S, 58f * S, 58f * S);
            var playIcon = UIKit.Img(playIconBox.transform, figGame, "Icon");
            playIcon.color = Color.white; playIcon.preserveAspect = true; UIKit.Stretch(playIcon.rectTransform, 10f * S);
            float playTextW = playW - 87f * S - 59f * S;
            var playTitle = UIKit.Text(play.transform, "Jugar y aprender", 19f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.TopLeft(playTitle.rectTransform, 87f * S, 15f * S, playTextW, 28f * S);
            var playSub = UIKit.Text(play.transform, "Completa misiones y gana estrellas", 14f * S, new Color(1, 1, 1, .90f), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Interlinea(playSub, 1.35f);
            UIKit.TopLeft(playSub.rectTransform, 87f * S, 43f * S, playTextW, 42f * S);
            var playChevron = UIKit.Img(play.transform, figChevron, "Chevron");
            playChevron.color = Color.white; playChevron.preserveAspect = true;
            UIKit.TopLeft(playChevron.rectTransform, playW - 47f * S, 36.5f * S, 25f * S, 25f * S);
            y += playH;

            content.sizeDelta = new Vector2(columnW, y + 30f * S);

            BuildBottomNav(p, 0);
            return p;
        }

        void BuildAdventureCard(RectTransform content, int index, float y, float w, float h,
                                string title, string description, Color color, Sprite robot)
        {
            const float S = 2.77f;
            var card = UIKit.Button(content, "", color, Color.white, Mathf.RoundToInt(25f * S), () => OnMenuTile(index, title), 20, true);
            UIKit.Top(R(card), 0, y, w, h);
            card.gameObject.AddComponent<Mask>();
            UIKit.AddShadow(card.GetComponent<Image>(), Mathf.RoundToInt(25f * S), .18f, -7f, 8f);
            card.gameObject.AddComponent<Appear>().delay = .15f + .05f * index;

            bool mirrored = index == 1;
            // Como en Figma: robot que sobresale por abajo; el título de una sola línea
            // ("Mundo Clínico") sube la descripción.
            float textoX = (mirrored ? 131f : 24f) * S;
            int lineas = index == 2 ? 1 : 2;
            var robotImg = UIKit.Img(card.transform, robot, "Robot");
            robotImg.preserveAspect = true;
            UIKit.TopLeft(robotImg.rectTransform, mirrored ? -14f * S : w - 152f * S, 41f * S, 154f * S, 187f * S);
            UIKit.SombraImagen(robotImg, .18f, 7f * S);
            var small = UIKit.Text(card.transform, "AVENTURA " + (index + 1), 12f * S, new Color(1, 1, 1, .85f), TextAlignmentOptions.Left);
            small.characterSpacing = 8f;
            UIKit.TopLeft(small.rectTransform, textoX, 27f * S, 190f * S, 18f * S);
            var strong = UIKit.Text(card.transform, title, 23f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.Interlinea(strong, 1.12f);
            UIKit.TopLeft(strong.rectTransform, textoX, 51f * S, 190f * S, (lineas * 26f + 3f) * S);
            var desc = UIKit.Text(card.transform, description, 15f * S, new Color(1, 1, 1, .92f), TextAlignmentOptions.Left, FontStyles.Normal);
            desc.fontWeight = FontWeight.SemiBold;
            UIKit.Interlinea(desc, 1.35f);
            UIKit.TopLeft(desc.rectTransform, textoX, (51f + lineas * 26f + 8f) * S, 170f * S, 44f * S);

            var arrowDisc = UIKit.Box(card.transform, Color.white, Mathf.RoundToInt(22.5f * S), "Arrow");
            UIKit.TopLeft(arrowDisc.rectTransform, mirrored ? 18f * S : w - 63f * S, 16f * S, 45f * S, 45f * S);
            var arrow = UIKit.Img(arrowDisc.transform, figChevron, "Chevron");
            arrow.color = color; arrow.preserveAspect = true; UIKit.Stretch(arrow.rectTransform, 11f * S);
        }

        void OnMenuTile(int idx, string label)
        {
            if (idx >= 0 && idx <= 2) { currentBook = idx; currentTopic = 0; ShowOnly(bookTemas[idx]); }
            else if (idx == 3) ShowOnly(pScanAR);
            else if (idx == 4) ShowOnly(pJuegos);
            else if (idx == 5) ShowOnly(pProgreso);
        }

        // ============================================================
        // 5 - MUNDOS
        // ============================================================
        RectTransform BuildMundos()
        {
            const float S = 2.77f;
            var p = Panel("5_Mundos");
            float columnW = Land ? 1000f : 1080f;
            float side = 20f * S;
            float y = 20f * S;
            float headerH = 90f * S;

            BuildHeaderBar(p, "Elige tu aventura", UIKit.Blue, figBook != null ? figBook : icBook,
                () => ShowOnly(pMenu));

            var viewport = UIKit.Node("WorldsViewport", p);
            viewport.anchorMin = Vector2.zero; viewport.anchorMax = Vector2.one;
            viewport.offsetMin = new Vector2(0, 82f * 2.77f); viewport.offsetMax = new Vector2(0, -headerH);
            viewport.gameObject.AddComponent<RectMask2D>();
            var catcher = viewport.gameObject.AddComponent<Image>();
            catcher.color = new Color(1, 1, 1, 0); catcher.raycastTarget = true;

            var scroll = viewport.gameObject.AddComponent<ScrollRect>();
            var content = UIKit.Node("WorldsContent", viewport);
            content.anchorMin = new Vector2(.5f, 1); content.anchorMax = new Vector2(.5f, 1);
            content.pivot = new Vector2(.5f, 1); content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(columnW, 1);
            scroll.content = content; scroll.viewport = viewport;
            scroll.horizontal = false; scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Elastic; scroll.inertia = true;
            scroll.scrollSensitivity = 3f * S;

            float introH = 250f * S, introW = columnW - 40f * S, introX = 156f * S, introTextW = introW - introX - 18f * S;
            var intro = UIKit.Box(content, UIKit.Hex("DFF2FF"), Mathf.RoundToInt(28f * S), "WorldsIntro");
            UIKit.Top(intro.rectTransform, 0, y, introW, introH);
            intro.gameObject.AddComponent<Mask>();
            intro.gameObject.AddComponent<Appear>().delay = .05f;
            var introRobots = BuildRobotGroup(intro.transform);
            UIKit.TopLeft(introRobots, 18f * S, 80f * S, 122f * S, 90f * S);
            var kicker = UIKit.Text(intro.transform, "TRES MUNDOS POR DESCUBRIR", 11f * S, UIKit.Blue, TextAlignmentOptions.Left);
            kicker.characterSpacing = 7f;
            UIKit.Interlinea(kicker, 2f);
            UIKit.TopLeft(kicker.rectTransform, introX, 46f * S, introTextW, 46f * S);
            var introTitle = UIKit.Text(intro.transform, "¿A dónde iremos hoy?", 27f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Interlinea(introTitle, 1.12f);
            UIKit.TopLeft(introTitle.rectTransform, introX, 96f * S, introTextW, 64f * S);
            var introBody = UIKit.Text(intro.transform, "Cada aventura guarda algo nuevo para aprender.", 15f * S, UIKit.Hex("536E8B"), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Interlinea(introBody, 1.4f);
            UIKit.TopLeft(introBody.rectTransform, introX, 162f * S, introTextW, 46f * S);
            y += introH + 20f * S;

            string[] names = { "Mundo Fisiológico", "Mundo Nutricional", "Mundo Clínico" };
            string[] descriptions = { "Descubre cómo funciona tu cuerpo", "Aprende sobre tus alimentos", "Conoce tus cuidados" };
            Color[] colors = { UIKit.Coral, UIKit.Nutri, UIKit.Clin };
            Sprite[] robots = { robotCoral, robotVerde, robotAzul };
            for (int i = 0; i < 3; i++)
            {
                BuildAdventureCard(content, i, y, columnW - 40f * S, 196f * S,
                    names[i], descriptions[i], colors[i], robots[i]);
                y += 196f * S + 18f * S;
            }
            content.sizeDelta = new Vector2(columnW, y + 10f * S);
            BuildBottomNav(p, 0);
            return p;
        }
    }
}
