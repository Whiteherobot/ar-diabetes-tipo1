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
            // Ilustración del cliente con el saludo, los robots y el título; solo el botón va encima.
            var arte = LienzoArte(p, recFondoInicio, true);
            var b = BotonImagen(arte, recComenzar, () => ShowOnly(pBienvenida), "Comenzar");
            UIKit.Frac(R(b), .12f, .06f, .88f, .14f);
            return p;
        }

        // Rect con la proporción de una ilustración de pantalla completa, centrado y sin recortarla:
        // lo que va encima se ubica en fracciones de la ilustración, no de la pantalla.
        RectTransform LienzoArte(RectTransform parent, Sprite arte, bool conImagen)
        {
            var lienzo = UIKit.Node("Arte", parent);
            UIKit.Stretch(lienzo);
            var fit = lienzo.gameObject.AddComponent<AspectRatioFitter>();
            fit.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            fit.aspectRatio = arte != null ? arte.rect.width / arte.rect.height : 1080f / 2296f;
            if (conImagen)
            {
                var im = UIKit.Img(lienzo, arte, "Ilustracion");
                im.preserveAspect = false;
                UIKit.Stretch(im.rectTransform);
            }
            return lienzo;
        }

        // Botón hecho con una ilustración del cliente: el texto ya viene en la imagen.
        Button BotonImagen(Transform parent, Sprite arte, UnityEngine.Events.UnityAction onClick, string nombre)
        {
            var im = UIKit.Img(parent, arte, nombre);
            im.preserveAspect = true;
            im.raycastTarget = true;
            var b = im.gameObject.AddComponent<Button>();
            b.targetGraphic = im;
            b.transition = Selectable.Transition.None;
            im.gameObject.AddComponent<PressEffect>();
            if (onClick != null) b.onClick.AddListener(onClick);
            return b;
        }

        // Tarjeta ilustrada a todo el ancho de la columna, con el alto que pide su proporción.
        Button TarjetaImagen(RectTransform content, Sprite arte, float y, float w, out float h,
                             UnityEngine.Events.UnityAction onClick, string nombre)
        {
            h = arte != null ? w * arte.rect.height / arte.rect.width : w * .45f;
            var b = BotonImagen(content, arte, onClick, nombre);
            UIKit.Top(R(b), 0, y, w, h);
            return b;
        }

        // Flecha en disco blanco (ilustración del cliente) sobre una tarjeta; no recibe toques,
        // el toque lo atiende la tarjeta entera.
        void FlechaTarjeta(Transform card, Sprite flecha, float x, float y, float lado)
        {
            if (flecha == null) return;
            var im = UIKit.Img(card, flecha, "Flecha");
            im.preserveAspect = true;
            UIKit.TopLeft(im.rectTransform, x, y, lado, lado);
        }

        // ============================================================
        // 2 - BIENVENIDA (carrusel)
        // ============================================================
        RectTransform BuildBienvenida()
        {
            var p = Panel("2_Bienvenida");
            // Cada slide es una ilustración completa del cliente (tarjeta con título y texto incluidos).
            // El carrusel desliza las ilustraciones; puntos y botones van en un lienzo fijo encima.
            var marco = LienzoArte(p, recBienvenida[0], false);
            var viewport = UIKit.Node("Viewport", marco);
            UIKit.Stretch(viewport);
            viewport.gameObject.AddComponent<RectMask2D>();
            var catcher = viewport.gameObject.AddComponent<Image>();
            catcher.color = new Color(1, 1, 1, 0); catcher.raycastTarget = true;

            // Content mide 3 viewports de ancho y cada slide un tercio: no depende de que el layout
            // ya esté calculado al construir (el carrusel mide el ancho real en cada frame).
            var content = UIKit.Node("Content", viewport);
            content.anchorMin = Vector2.zero; content.anchorMax = new Vector2(3f, 1f);
            content.pivot = new Vector2(0, 0.5f);
            content.offsetMin = content.offsetMax = Vector2.zero;

            for (int i = 0; i < 3; i++)
            {
                var slide = UIKit.Img(content, recBienvenida[i], "Slide" + i);
                slide.preserveAspect = false;
                UIKit.Frac(slide.rectTransform, i / 3f, 0f, (i + 1) / 3f, 1f);
            }
            Canvas.ForceUpdateCanvases();
            float w = viewport.rect.width;

            // Puntos dentro de la tarjeta blanca, cerca de su borde inferior (la tarjeta termina al 22 %).
            var dots = new Image[3];
            for (int i = 0; i < 3; i++)
            {
                var d = UIKit.Img(marco, UIKit.Circle(), "Dot" + i);
                d.preserveAspect = true;
                d.color = i == 0 ? UIKit.Blue : UIKit.Hex("C4D3E0");
                float cx = 0.5f + (i - 1) * 0.045f;
                UIKit.Frac(d, cx - 0.012f, 0.245f, cx + 0.012f, 0.262f);
                dots[i] = d;
            }

            var siguiente = BotonImagen(marco, recSiguiente, null, "Siguiente");
            UIKit.Frac(R(siguiente), .12f, .105f, .88f, .185f);
            var omitir = BotonImagen(marco, recOmitir, () => ShowOnly(pPerfil), "Omitir");
            UIKit.Frac(R(omitir), .12f, .02f, .88f, .10f);
            var sigImg = siguiente.GetComponent<Image>();

            var carousel = viewport.gameObject.AddComponent<Carousel>();
            carousel.content = content;
            carousel.slideWidth = w;
            carousel.medirViewport = true;
            carousel.count = 3;
            carousel.dots = dots;
            carousel.dotOn = UIKit.Blue;
            carousel.dotOff = UIKit.Hex("C4D3E0");
            carousel.onComplete = () => ShowOnly(pPerfil);
            // En la última ilustración el botón pasa a COMENZAR (el texto viene en la imagen).
            carousel.onIndexChanged = idx => { if (sigImg != null) sigImg.sprite = idx == 2 ? recComenzar : recSiguiente; };
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

            // 3 personajes (uno por mundo) en una fila; en la tarjeta va el busto ilustrado.
            var lista = Personajes;
            int cols = 3, rows = 1;
            float x0 = land ? 0.20f : 0.05f, x1 = land ? 0.80f : 0.95f;
            float y0 = land ? 0.17f : 0.44f, y1 = land ? 0.56f : 0.65f;
            float gx = land ? 0.02f : 0.03f, gy = 0f;
            var marcos = new GameObject[lista.Length];
            int elegido = PersonajeVigente(AppState.Character);
            if (elegido != AppState.Character) AppState.Character = elegido;
            for (int i = 0; i < lista.Length; i++)
            {
                var rc = UIKit.Cell(i, cols, rows, x0, y0, x1, y1, gx, gy);
                var busto = bustos != null && i < bustos.Length && bustos[i] != null ? bustos[i] : lista[i];
                marcos[i] = PersonajeOption(p, rc, busto, i, marcos);
            }
            for (int i = 0; i < marcos.Length; i++) marcos[i].SetActive(i == elegido);

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
            // Tarjeta ilustrada del cliente; el botón EXPLORA AHORA va abajo a la izquierda, debajo del texto.
            float featureW = columnW - 40f * S;
            var explore = TarjetaImagen(content, recExplorar, y, featureW, out float featureH, () => ShowOnly(pMundos), "explore-feature");
            explore.gameObject.AddComponent<Appear>().delay = .05f;
            float btnW = featureW * .44f, btnH = recExploraAhora != null ? btnW * recExploraAhora.rect.height / recExploraAhora.rect.width : 50f * S;
            var exploreButton = BotonImagen(explore.transform, recExploraAhora, () => ShowOnly(pMundos), "ExploraAhora");
            UIKit.TopLeft(R(exploreButton), featureW * .045f, featureH * .67f, btnW, btnH);
            y += featureH + 28f * S;

            // ---- Aventuras ----
            var adventureTitle = UIKit.Text(content, "Elige tu aventura", 23f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Top(adventureTitle.rectTransform, 0, y, columnW - 40f * S, 32f * S);
            adventureTitle.gameObject.AddComponent<Appear>().delay = .10f;
            y += 32f * S + 16f * S;
            for (int i = 0; i < 3; i++)
            {
                y += BuildAdventureCard(content, i, y, columnW - 40f * S) + 18f * S;
            }
            y -= 18f * S;
            y += 30f * S;

            // ---- Escanear y Jugar: tarjetas ilustradas, con la flecha del cliente al borde derecho ----
            float sideW = columnW - 40f * S;
            var scan = TarjetaImagen(content, recEscanear, y, sideW, out float scanH, () => OnMenuTile(3, "Escanear página"), "scan-feature");
            scan.gameObject.AddComponent<Appear>().delay = .30f;
            float lado = scanH * .30f;
            FlechaTarjeta(scan.transform, recFlechas[3], sideW - lado - sideW * .025f, (scanH - lado) * .5f, lado);
            y += scanH + 20f * S;

            var play = TarjetaImagen(content, recJuegos, y, sideW, out float playH, () => OnMenuTile(4, "Jugar y aprender"), "play-feature");
            play.gameObject.AddComponent<Appear>().delay = .35f;
            lado = playH * .30f;
            FlechaTarjeta(play.transform, recFlechas[4], sideW - lado - sideW * .025f, (playH - lado) * .5f, lado);
            y += playH;

            content.sizeDelta = new Vector2(columnW, y + 30f * S);

            BuildBottomNav(p, 0);
            return p;
        }

        // Tarjeta ilustrada de un mundo (título, texto y robot vienen en la imagen del cliente) con
        // su flecha de color. Devuelve el alto que ocupó.
        float BuildAdventureCard(RectTransform content, int index, float y, float w)
        {
            string[] nombres = { "Mundo Fisiológico", "Mundo Nutricional", "Mundo Clínico" };
            var card = TarjetaImagen(content, recMundos[index], y, w, out float h, () => OnMenuTile(index, nombres[index]), "Mundo" + index);
            card.gameObject.AddComponent<Appear>().delay = .15f + .05f * index;
            // Abajo a la derecha, junto al robot: arriba están la etiqueta y la cabeza del robot.
            float lado = h * .24f;
            FlechaTarjeta(card.transform, recFlechas[index], w - lado - w * .025f, h - lado - h * .06f, lado);
            return h;
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

            // Intro ilustrada del cliente («¿A dónde iremos hoy?»), solo decorativa.
            float introW = columnW - 40f * S;
            float introH = recIntroMundos != null ? introW * recIntroMundos.rect.height / recIntroMundos.rect.width : 250f * S;
            var intro = UIKit.Img(content, recIntroMundos, "WorldsIntro");
            UIKit.Top(intro.rectTransform, 0, y, introW, introH);
            intro.gameObject.AddComponent<Appear>().delay = .05f;
            y += introH + 20f * S;

            for (int i = 0; i < 3; i++)
            {
                y += BuildAdventureCard(content, i, y, columnW - 40f * S) + 18f * S;
            }
            content.sizeDelta = new Vector2(columnW, y + 10f * S);
            BuildBottomNav(p, 0);
            return p;
        }
    }
}
