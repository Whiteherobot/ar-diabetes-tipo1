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

            var t = UIKit.Text(p, "¿Cuál es tu edad?", 31f * S, UIKit.Navy);
            var sub = UIKit.Text(p, "Personalizamos la experiencia según tu edad", 16f * S, UIKit.Muted, TextAlignmentOptions.Center, FontStyles.Normal);
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

            int cols = land ? 3 : 1, rows = land ? 1 : 3;
            float x0 = 0.05f, x1 = 0.95f, y0 = land ? 0.20f : 0.19f, y1 = land ? 0.67f : 0.74f;
            float gx = 0.03f, gy = 0.035f;
            Sprite[] avs = { spAvatar1, spAvatar2, spAvatar3 };
            string[] labs = { "5 - 9 años", "10 - 12 años", "13 - 15 años" };
            AppState.AgeGroup[] grp = { AppState.AgeGroup.Kids_5_9, AppState.AgeGroup.Kids_10_12, AppState.AgeGroup.Teens_13_15 };
            Color[] acc = { UIKit.Prog, UIKit.Nutri, UIKit.Juegos };
            for (int i = 0; i < 3; i++)
            {
                var rc = UIKit.Cell(i, cols, rows, x0, y0, x1, y1, gx, gy);
                AgeOption(p, rc, avs[i], labs[i], grp[i], acc[i], land);
            }
            var continueButton = UIKit.Button(p, "CONTINUAR", UIKit.Blue, Color.white, Mathf.RoundToInt(17f * S),
                () => ShowOnly(pMenu), Mathf.RoundToInt(17f * S), true);
            UIKit.Frac(R(continueButton), land ? .34f : .05f, .035f, land ? .66f : .95f, .115f);
            UIKit.AddShadow(continueButton.GetComponent<Image>(), Mathf.RoundToInt(17f * 2.77f), .14f, -6f, 6f);
            return p;
        }

        void AgeOption(RectTransform parent, Rect rc, Sprite avatar, string label, AppState.AgeGroup group, Color accent, bool land)
        {
            var btn = UIKit.Button(parent, "", UIKit.Card, UIKit.Navy, 36, () => { AppState.Age = group; ShowOnly(pMenu); });
            UIKit.Frac(R(btn), rc.xMin, rc.yMin, rc.xMax, rc.yMax);
            UIKit.AddShadow(btn.GetComponent<Image>(), 36, 0.10f, -8, 8);
            btn.gameObject.AddComponent<Appear>().delay = 0.05f * (int)group;

            var disc = UIKit.Img(btn.transform, UIKit.Circle(), "Disc"); disc.color = accent; disc.preserveAspect = true;
            var av = UIKit.Img(btn.transform, avatar, "Avatar"); av.preserveAspect = true;
            var tl = UIKit.Text(btn.transform, label, 54, UIKit.Navy, land ? TextAlignmentOptions.Center : TextAlignmentOptions.Left);

            if (!land)
            {
                UIKit.Frac(disc, 0.04f, 0.16f, 0.28f, 0.84f);
                UIKit.Frac(av, 0.05f, 0.14f, 0.27f, 0.86f);
                UIKit.Frac(tl, 0.32f, 0.2f, 0.88f, 0.8f);
                var chev = UIKit.Text(btn.transform, ">", 60, accent, TextAlignmentOptions.Right);
                UIKit.Frac(chev, 0.88f, 0.3f, 0.97f, 0.7f);
            }
            else
            {
                UIKit.Frac(disc, 0.28f, 0.52f, 0.72f, 0.92f);
                UIKit.Frac(av, 0.29f, 0.51f, 0.71f, 0.93f);
                UIKit.Frac(tl, 0.05f, 0.12f, 0.95f, 0.42f);
            }
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

            // ---- Hero ----
            float heroW = columnW - 32f * S, heroH = 326f * S;
            var hero = UIKit.Box(content, UIKit.Hex("DFF2FF"), Mathf.RoundToInt(32f * S), "explorer-hero");
            UIKit.Top(hero.rectTransform, 0, y, heroW, heroH);
            hero.gameObject.AddComponent<Mask>(); // recorte con esquinas redondeadas
            hero.gameObject.AddComponent<Appear>().delay = 0f;
            var heroBlob1 = UIKit.Img(hero.transform, UIKit.Circle(), "BlobTop");
            heroBlob1.color = new Color(131f / 255f, 112f / 255f, 218f / 255f, .12f);
            UIKit.Top(heroBlob1.rectTransform, heroW / 2f - 35f * S, -70f * S, 180f * S, 180f * S); // x desde el centro
            var heroBlob2 = UIKit.Img(hero.transform, UIKit.Circle(), "BlobBottom");
            heroBlob2.color = new Color(47f / 255f, 174f / 255f, 117f / 255f, .13f);
            UIKit.Top(heroBlob2.rectTransform, -heroW / 2f + 2.5f * S, heroH - 81f * S, 135f * S, 135f * S);

            var heroTitle = UIKit.Text(hero.transform, "¡Hola, explorador!", 31f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Frac(heroTitle, .055f, .64f, .70f, .94f);
            var heroSub = UIKit.Text(hero.transform, "¿Qué descubriremos hoy?", 17f * S, UIKit.Hex("4D6784"), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Frac(heroSub, .055f, .49f, .72f, .64f);

            var levelPill = UIKit.Box(hero.transform, Color.white, Mathf.RoundToInt(21f * S), "LevelPill");
            UIKit.Frac(levelPill, .055f, .30f, .32f, .45f);
            UIKit.AddShadow(levelPill, Mathf.RoundToInt(21f * S), .08f, -3f, 4f);
            nivelText = UIKit.Text(levelPill.transform, "Nivel " + AppState.Level, 15f * S, UIKit.Blue, TextAlignmentOptions.Center);
            UIKit.Stretch(nivelText.rectTransform);
            var starsPill = UIKit.Box(hero.transform, UIKit.Hex("FFF5CE"), Mathf.RoundToInt(21f * S), "StarsPill");
            UIKit.Frac(starsPill, .35f, .30f, .63f, .45f);
            UIKit.AddShadow(starsPill, Mathf.RoundToInt(21f * S), .08f, -3f, 4f);
            var starIcon = UIKit.Img(starsPill.transform, figStar, "Star");
            starIcon.color = UIKit.Hex("FFB830"); starIcon.preserveAspect = true;
            UIKit.Frac(starIcon, .10f, .18f, .36f, .82f);
            starLabel = UIKit.Text(starsPill.transform, AppState.Stars.ToString(), 15f * S, UIKit.Hex("654D13"), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Frac(starLabel, .42f, .12f, .90f, .88f);

            var heroRobots = BuildRobotGroup(hero.transform);
            UIKit.Frac(heroRobots, .304f, -.015f, 1.008f, .503f);
            Anim.HeroLlega(heroRobots);
            heroRobots.transform.SetSiblingIndex(4);
            y += heroH + 28f * S;

            // ---- Explora ----
            float featureW = columnW - 40f * S, featureH = 230f * S;
            var explore = UIKit.Box(content, UIKit.Navy, Mathf.RoundToInt(26f * S), "explore-feature");
            UIKit.Top(explore.rectTransform, 0, y, featureW, featureH);
            UIKit.AddShadow(explore, Mathf.RoundToInt(26f * S), .16f, -8f, 8f);
            explore.gameObject.AddComponent<Appear>().delay = .05f;
            explore.gameObject.AddComponent<Mask>();
            var exploreCircle = UIKit.Img(explore.transform, UIKit.Circle(), "StarDisc");
            exploreCircle.color = new Color(1, 1, 1, .11f); exploreCircle.preserveAspect = true;
            UIKit.Top(exploreCircle.rectTransform, featureW / 2f - 54f * S, 28f * S, 54f * S, 54f * S);
            Anim.Deriva(exploreCircle);
            var exploreStar = UIKit.Img(exploreCircle.transform, figStar, "Star");
            exploreStar.color = UIKit.Hex("FFB830"); exploreStar.preserveAspect = true; UIKit.Stretch(exploreStar.rectTransform, 12f * S);
            var exploreDecor = UIKit.Img(explore.transform, UIKit.Circle(), "Decor");
            exploreDecor.color = new Color(1, 1, 1, .08f); exploreDecor.preserveAspect = true;
            UIKit.Top(exploreDecor.rectTransform, featureW / 2f - 13f * S, featureH - 70f * S, 150f * S, 150f * S);
            var kicker = UIKit.Text(explore.transform, "EXPLORA, DESCUBRE Y APRENDE", 12f * S, UIKit.Hex("A9D8FF"), TextAlignmentOptions.Left);
            kicker.characterSpacing = 10f; UIKit.Frac(kicker, .06f, .73f, .94f, .91f);
            var exploreTitle = UIKit.Text(explore.transform, "¡Comienza a explorar!", 26f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.Frac(exploreTitle, .06f, .54f, .94f, .74f);
            var exploreBody = UIKit.Text(explore.transform, "Viaja por tres mundos y descubre cómo cuidar tu cuerpo cada día.", 16f * S, UIKit.Hex("DCECFF"), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Frac(exploreBody, .06f, .31f, .82f, .54f);
            var exploreButton = UIKit.Button(explore.transform, "", Color.white, UIKit.Navy, Mathf.RoundToInt(16f * S), () => ShowOnly(pMundos), 15, true);
            UIKit.Frac(R(exploreButton), .06f, .07f, .70f, .29f);
            var exploreButtonText = UIKit.Text(exploreButton.transform, "EXPLORAR AHORA", 15f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Frac(exploreButtonText, .08f, .12f, .82f, .88f);
            exploreButtonText.enableWordWrapping = false;
            var exploreChevron = UIKit.Img(exploreButton.transform, figChevron, "Chevron");
            exploreChevron.color = UIKit.Navy; exploreChevron.preserveAspect = true; UIKit.Frac(exploreChevron, .82f, .25f, .95f, .75f);
            y += featureH + 28f * S;

            // ---- Aventuras ----
            var adventureTitle = UIKit.Text(content, "Elige tu aventura", 22f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Top(adventureTitle.rectTransform, 0, y, columnW - 40f * S, 34f * S);
            adventureTitle.gameObject.AddComponent<Appear>().delay = .10f;
            y += 34f * S + 18f * S;
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
            float scanW = columnW - 40f * S, scanH = 200f * S;
            var scan = UIKit.Box(content, UIKit.Hex("FFF0E6"), Mathf.RoundToInt(25f * S), "scan-feature");
            UIKit.Top(scan.rectTransform, 0, y, scanW, scanH);
            UIKit.AddShadow(scan, Mathf.RoundToInt(25f * S), .10f, -6f, 7f);
            scan.gameObject.AddComponent<Appear>().delay = .30f;
            var scanIconBox = UIKit.Box(scan.transform, UIKit.Scan, Mathf.RoundToInt(22f * S), "ScanIcon");
            UIKit.Frac(scanIconBox.rectTransform, .06f, .58f, .28f, .90f);
            scanIconBox.rectTransform.localRotation = Quaternion.Euler(0, 0, -4f);
            var scanIcon = UIKit.Img(scanIconBox.transform, figScan, "Icon");
            scanIcon.color = Color.white; scanIcon.preserveAspect = true; UIKit.Stretch(scanIcon.rectTransform, 12f * S);
            var scanKicker = UIKit.Text(scan.transform, "REALIDAD AUMENTADA", 12f * S, UIKit.Hex("A35422"), TextAlignmentOptions.Left);
            UIKit.Frac(scanKicker, .34f, .78f, .94f, .93f);
            var scanTitle = UIKit.Text(scan.transform, "¡Haz que tu libro cobre vida!", 22f * S, UIKit.Hex("663112"), TextAlignmentOptions.Left);
            UIKit.Frac(scanTitle, .34f, .53f, .94f, .80f);
            var scanBody = UIKit.Text(scan.transform, "Escanea una página y descubre su contenido en 3D.", 15f * S, UIKit.Hex("764726"), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Frac(scanBody, .34f, .32f, .94f, .54f);
            var scanButton = UIKit.Button(scan.transform, "ESCANEAR PÁGINA", UIKit.Hex("D65D0D"), Color.white, Mathf.RoundToInt(14f * S), () => OnMenuTile(3, "Escanear página"), Mathf.RoundToInt(14f * S), true);
            UIKit.Frac(R(scanButton), .34f, .07f, .78f, .28f);
            y += scanH + 20f * S;

            // ---- Jugar ----
            float playW = columnW - 40f * S, playH = 98f * S;
            var play = UIKit.Button(content, "", UIKit.Juegos, Color.white, Mathf.RoundToInt(24f * S), () => OnMenuTile(4, "Jugar y aprender"), 19, true);
            UIKit.Top(R(play), 0, y, playW, playH);
            UIKit.AddShadow(play.GetComponent<Image>(), Mathf.RoundToInt(24f * S), .16f, -7f, 8f);
            play.gameObject.AddComponent<Appear>().delay = .35f;
            var playIconBox = UIKit.Box(play.transform, new Color(1, 1, 1, .18f), Mathf.RoundToInt(18f * S), "GameIcon");
            UIKit.Frac(playIconBox.rectTransform, .05f, .18f, .20f, .82f);
            var playIcon = UIKit.Img(playIconBox.transform, figGame, "Icon");
            playIcon.color = Color.white; playIcon.preserveAspect = true; UIKit.Stretch(playIcon.rectTransform, 10f * S);
            var playTitle = UIKit.Text(play.transform, "Jugar y aprender", 19f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.Frac(playTitle, .25f, .48f, .80f, .82f);
            var playSub = UIKit.Text(play.transform, "Completa misiones y gana estrellas", 14f * S, new Color(1, 1, 1, .90f), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Frac(playSub, .25f, .16f, .80f, .48f);
            var playChevron = UIKit.Img(play.transform, figChevron, "Chevron");
            playChevron.color = Color.white; playChevron.preserveAspect = true; UIKit.Frac(playChevron, .86f, .34f, .96f, .66f);
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
            var small = UIKit.Text(card.transform, "AVENTURA " + (index + 1), 12f * S, new Color(1, 1, 1, .85f), TextAlignmentOptions.Left);
            var strong = UIKit.Text(card.transform, title, 23f * S, Color.white, TextAlignmentOptions.Left);
            var desc = UIKit.Text(card.transform, description, 15f * S, new Color(1, 1, 1, .92f), TextAlignmentOptions.Left, FontStyles.Normal);
            if (!mirrored)
            {
                UIKit.Frac(small, .065f, .78f, .60f, .94f);
                UIKit.Frac(strong, .065f, .48f, .68f, .80f);
                UIKit.Frac(desc, .065f, .24f, .63f, .48f);
            }
            else
            {
                UIKit.Frac(small, .42f, .78f, .94f, .94f);
                UIKit.Frac(strong, .42f, .48f, .94f, .80f);
                UIKit.Frac(desc, .42f, .24f, .94f, .48f);
            }

            var robotImg = UIKit.Img(card.transform, robot, "Robot");
            robotImg.preserveAspect = true;
            UIKit.Frac(robotImg, mirrored ? -.04f : .62f, -.14f, mirrored ? .38f : 1.04f, .80f);
            var arrowDisc = UIKit.Box(card.transform, Color.white, Mathf.RoundToInt(23f * S), "Arrow");
            UIKit.Frac(arrowDisc.rectTransform, mirrored ? .06f : .78f, .72f, mirrored ? .20f : .92f, .95f);
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
            float y = 18f * S;
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

            float introH = 230f * S;
            var intro = UIKit.Box(content, UIKit.Hex("DFF2FF"), Mathf.RoundToInt(28f * S), "WorldsIntro");
            UIKit.Top(intro.rectTransform, 0, y, columnW - 40f * S, introH);
            intro.gameObject.AddComponent<Mask>();
            intro.gameObject.AddComponent<Appear>().delay = .05f;
            var introRobots = BuildRobotGroup(intro.transform);
            UIKit.Frac(introRobots, .04f, .25f, .42f, .75f);
            var kicker = UIKit.Text(intro.transform, "TRES MUNDOS POR DESCUBRIR", 11f * S, UIKit.Blue, TextAlignmentOptions.Left);
            kicker.characterSpacing = 8f; UIKit.Frac(kicker, .49f, .70f, .95f, .90f);
            var introTitle = UIKit.Text(intro.transform, "¿A dónde iremos hoy?", 25f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Frac(introTitle, .49f, .43f, .96f, .72f);
            var introBody = UIKit.Text(intro.transform, "Cada aventura guarda algo nuevo para aprender.", 15f * S, UIKit.Hex("536E8B"), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Frac(introBody, .49f, .16f, .95f, .44f);
            y += introH + 26f * S;

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
            content.sizeDelta = new Vector2(columnW, y + 24f * S);
            BuildBottomNav(p, 0);
            return p;
        }
    }
}
