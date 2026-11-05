using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    // Pantallas 5A/5B/5C - LIBROS: Temas -> Detalle (la Experiencia AR en sí vive en AppBootstrap.AR.cs).
    public partial class AppBootstrap
    {
        static readonly Color[] TopicAccent = { UIKit.Clin, UIKit.Fisio, UIKit.Nutri, UIKit.Prog };

        static readonly Color[] FondoLibro = { UIKit.Hex("FFF2F3"), UIKit.Hex("EFFAF4"), UIKit.Hex("EFF7FF") };

        Sprite RobotDeLibro(int book) => book == 0 ? robotCoral : book == 1 ? robotVerde : robotAzul;

        // Área desplazable bajo el encabezado; abajo deja libre lo que ocupe la barra inferior.
        RectTransform BuildAreaScroll(RectTransform p, string nombre, float abajo, float columnW)
        {
            const float S = 2.77f;
            var viewport = UIKit.Node(nombre + "Viewport", p);
            viewport.anchorMin = Vector2.zero; viewport.anchorMax = Vector2.one;
            viewport.offsetMin = new Vector2(0, abajo); viewport.offsetMax = new Vector2(0, -90f * S);
            viewport.gameObject.AddComponent<RectMask2D>();
            var catcher = viewport.gameObject.AddComponent<Image>();
            catcher.color = new Color(1, 1, 1, 0); catcher.raycastTarget = true;
            var content = UIKit.Node(nombre + "Content", viewport);
            content.anchorMin = new Vector2(.5f, 1); content.anchorMax = new Vector2(.5f, 1);
            content.pivot = new Vector2(.5f, 1); content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(columnW, 1);
            UIKit.Scroll(viewport, content);
            return content;
        }

        RectTransform BuildLibroTemas(int book)
        {
            const float S = 2.77f;
            var b = books[book];
            var p = Panel(book + "_Temas");
            var fondo = UIKit.Img(p, null, "Fondo"); fondo.color = FondoLibro[book]; UIKit.Stretch(fondo.rectTransform);
            BuildHeaderBar(p, b.Title, b.Accent, figBook != null ? figBook : icBook, () => ShowOnly(pMundos), RobotDeLibro(book));

            float columnW = Land ? 1000f : 1080f, w = columnW - 40f * S;
            var content = BuildAreaScroll(p, "Topics", 82f * S, columnW);
            float y = 20f * S;

            float heroH = 215f * S;
            var hero = UIKit.Box(content, b.Accent, Mathf.RoundToInt(27f * S), "WorldHero");
            UIKit.Top(hero.rectTransform, 0, y, w, heroH);
            UIKit.AddShadow(hero, Mathf.RoundToInt(27f * S), .14f, -8, 8);
            hero.gameObject.AddComponent<Mask>();
            hero.gameObject.AddComponent<Appear>().delay = 0f;
            var heroRobot = UIKit.Img(hero.transform, RobotDeLibro(book), "Robot");
            heroRobot.preserveAspect = true;
            UIKit.TopLeft(heroRobot.rectTransform, w - 138f * S, 50f * S, 139f * S, 167f * S);
            UIKit.SombraImagen(heroRobot, .2f, 7f * S);
            var kicker = UIKit.Text(hero.transform, "EXPLORA Y DESCUBRE", 12f * S, new Color(1, 1, 1, .86f), TextAlignmentOptions.Left);
            kicker.characterSpacing = 8f;
            var worldTitle = UIKit.Text(hero.transform, b.Title.Replace("Libro", "Mundo"), 27f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.TamanoFijo(worldTitle, 27f * S);
            UIKit.Interlinea(worldTitle, 1.12f);
            float tituloH = UIKit.Alto(worldTitle, worldTitle.text, 220f * S);
            var worldSub = UIKit.Text(hero.transform, "Elige un tema y comienza una nueva aventura.", 15f * S, Color.white, TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Interlinea(worldSub, 1.4f);
            float bloqueH = 18f * S + 7f * S + tituloH + 7f * S + 44f * S;
            float yTexto = 25f * S + (heroH - 47f * S - bloqueH) / 2f;
            UIKit.TopLeft(kicker.rectTransform, 24f * S, yTexto, 220f * S, 18f * S);
            UIKit.TopLeft(worldTitle.rectTransform, 24f * S, yTexto + 25f * S, 220f * S, tituloH + 4f);
            UIKit.TopLeft(worldSub.rectTransform, 24f * S, yTexto + 32f * S + tituloH, 215f * S, 46f * S);
            y += heroH + 25f * S;

            for (int i = 0; i < 4; i++)
            {
                int idx = i;
                bool inverso = i % 2 == 1;
                Color acc = TopicAccent[i];
                var btn = UIKit.Button(content, "", UIKit.Card, UIKit.Navy, Mathf.RoundToInt(20f * S),
                    () => { currentTopic = idx; ShowOnly(bookDetalle[book]); }, 30);
                float cardH = 126f * S;
                UIKit.Top(R(btn), 0, y, w, cardH);
                UIKit.AddShadow(btn.GetComponent<Image>(), Mathf.RoundToInt(20f * S), .12f, -6, 6);
                btn.gameObject.AddComponent<Mask>();
                var stripe = UIKit.Box(btn.transform, acc, Mathf.RoundToInt(5f * S), "Stripe");
                UIKit.TopLeft(stripe.rectTransform, -5f * S, 18f * S, 10f * S, cardH - 36f * S);
                var disc = UIKit.Img(btn.transform, UIKit.Circle(), "Disc"); disc.color = acc;
                UIKit.TopLeft(disc.rectTransform, inverso ? w - 121f * S : 15f * S, 26f * S, 74f * S, 74f * S);
                var tic = UIKit.Img(disc.transform, b.TopicIcon[i], "Ic"); tic.color = Color.white; tic.preserveAspect = true;
                UIKit.Stretch(tic.rectTransform, 23.5f * S);

                float textoX = (inverso ? 20f : 105f) * S, textoW = inverso ? w - 135f * S : w - 145f * S;
                var tl = UIKit.Text(btn.transform, b.TopicTitle[i], 18f * S, UIKit.Navy, TextAlignmentOptions.Left);
                UIKit.TamanoFijo(tl, 18f * S);
                UIKit.Interlinea(tl, 1.364f);
                var sb = UIKit.Text(btn.transform, b.TopicSub[i], 15f * S, UIKit.Hex("61758D"), TextAlignmentOptions.Left, FontStyles.Normal);
                UIKit.TamanoFijo(sb, 15f * S);
                UIKit.Interlinea(sb, 1.4f);
                float altoT = UIKit.Alto(tl, tl.text, textoW), altoS = UIKit.Alto(sb, sb.text, textoW);
                float yT = (cardH - altoT - 4f * S - altoS) / 2f;
                UIKit.TopLeft(tl.rectTransform, textoX, yT, textoW, altoT + 4f);
                UIKit.TopLeft(sb.rectTransform, textoX, yT + altoT + 4f * S, textoW, altoS + 4f);

                var chev = UIKit.Img(btn.transform, figChevron, "Chevron"); chev.color = acc; chev.preserveAspect = true;
                UIKit.TopLeft(chev.rectTransform, w - 40f * S, 52f * S, 22f * S, 22f * S);
                Anim.TarjetaSube(btn, .05f * i);
                y += cardH + 17f * S;
            }
            content.sizeDelta = new Vector2(columnW, y + 11f * S);
            BuildBottomNav(p, 0);
            return p;
        }

        Button BotonPrincipal(Transform parent, string texto, Color color, Sprite icono, UnityEngine.Events.UnityAction click)
        {
            const float S = 2.77f;
            var boton = UIKit.Button(parent, "", color, Color.white, Mathf.RoundToInt(17f * S), click, 20);
            var etiqueta = UIKit.Text(boton.transform, texto, 17f * S, Color.white, TextAlignmentOptions.Left);
            etiqueta.characterSpacing = 3.5f;
            etiqueta.enableWordWrapping = false;
            UIKit.TamanoFijo(etiqueta, 17f * S);
            float anchoT = etiqueta.GetPreferredValues(texto).x + 4f, total = anchoT + 33f * S;
            var ico = UIKit.Img(boton.transform, icono, "Ico"); ico.color = Color.white; ico.preserveAspect = true;
            Fila(ico.rectTransform, -total / 2f, 23f * S, 23f * S);
            Fila(etiqueta.rectTransform, -total / 2f + 33f * S, anchoT, 40f * S);
            return boton;
        }

        RectTransform BuildLibroDetalle(int book)
        {
            const float S = 2.77f;
            var b = books[book];
            var p = Panel(book + "_Detalle");
            Sprite headerIcon = book == 0 ? figTemas[1] : book == 1 ? figTemas[6] : figKit;
            BuildHeaderBar(p, b.Title, b.Accent, headerIcon, () => ShowOnly(bookTemas[book]), RobotDeLibro(book));

            float columnW = Land ? 1000f : 1080f, y = 20f * S, w = columnW - 40f * S;
            var content = BuildAreaScroll(p, "Detail", 0f, columnW);

            var discover = UIKit.Box(content, b.Accent, Mathf.RoundToInt(28f * S), "Discover");
            UIKit.Top(discover.rectTransform, 0, y, w, 290f * S);
            UIKit.AddShadow(discover, Mathf.RoundToInt(28f * S), .13f, -8, 8);
            discover.gameObject.AddComponent<Mask>();
            var discoverLabel = UIKit.Text(discover.transform, "DESCUBRE", 13f * S, Color.white, TextAlignmentOptions.Left);
            discoverLabel.characterSpacing = 9f;
            UIKit.TopLeft(discoverLabel.rectTransform, 22f * S, 19f * S, 200f * S, 20f * S);
            var band = discover;
            var img = UIKit.Img(band.transform, book == 0 && spPancreasPersonaje != null ? spPancreasPersonaje : b.HeroIcon, "Img");
            img.preserveAspect = true;
            UIKit.TopLeft(img.rectTransform, (w - 230f * S) / 2f, 55f * S, 230f * S, 180f * S);
            var discoverRobot = UIKit.Img(discover.transform, RobotDeLibro(book), "Robot"); discoverRobot.preserveAspect = true;
            UIKit.TopLeft(discoverRobot.rectTransform, w - 86f * S, 208f * S, 78f * S, 94f * S);
            UIKit.SombraImagen(discoverRobot, .2f, 4f * S);
            y += 290f * S + 20f * S;

            var learn = UIKit.Box(content, UIKit.Card, Mathf.RoundToInt(24f * S), "Learn");
            UIKit.Top(learn.rectTransform, 0, y, w, 330f * S);
            var learnShadow = UIKit.AddShadow(learn, Mathf.RoundToInt(24f * S), .10f, -7, 7);
            var learnLabel = UIKit.Text(learn.transform, "APRENDE", 12f * S, UIKit.Blue, TextAlignmentOptions.Left);
            learnLabel.characterSpacing = 9f;
            UIKit.TopLeft(learnLabel.rectTransform, 24f * S, 24f * S, w - 48f * S, 18f * S);
            var title = UIKit.Text(learn.transform, "", 29f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.TamanoFijo(title, 29f * S);
            UIKit.Interlinea(title, 1.15f);
            var desc = UIKit.Text(learn.transform, "", 17f * S, UIKit.Hex("4D6681"), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.TamanoFijo(desc, 17f * S);
            UIKit.Interlinea(desc, 1.55f);
            var coach = UIKit.Node("Coach", learn.transform);
            UIKit.TopLeft(coach, 24f * S, 0, w - 48f * S, 66f * S);
            var coachRobot = UIKit.Img(coach, RobotDeLibro(book), "Coach");
            coachRobot.preserveAspect = true; UIKit.TopLeft(coachRobot.rectTransform, 0, 0, 55f * S, 66f * S);
            var bubble = UIKit.Box(coach, UIKit.Hex("EDF8FF"), Mathf.RoundToInt(14f * S), "Bubble");
            UIKit.TopLeft(bubble.rectTransform, 63f * S, 13f * S, 203f * S, 40f * S);
            var bubbleText = UIKit.Text(bubble.transform, "¡Lo estás haciendo genial!", 15f * S, UIKit.Hex("526C89"), TextAlignmentOptions.Center);
            bubbleText.fontWeight = FontWeight.Bold;
            UIKit.Stretch(bubbleText.rectTransform);
            y += 330f * S + 20f * S;

            var interact = UIKit.Box(content, UIKit.Card, Mathf.RoundToInt(24f * S), "Interact");
            UIKit.Top(interact.rectTransform, 0, y, w, 249f * S);
            var interactShadow = UIKit.AddShadow(interact, Mathf.RoundToInt(24f * S), .10f, -7, 7);
            var interactLabel = UIKit.Text(interact.transform, "INTERACTÚA", 12f * S, UIKit.Blue, TextAlignmentOptions.Left);
            interactLabel.characterSpacing = 9f;
            UIKit.TopLeft(interactLabel.rectTransform, 24f * S, 24f * S, w - 48f * S, 18f * S);
            var interactTitle = UIKit.Text(interact.transform, "Elige cómo quieres descubrirlo", 21f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.TopLeft(interactTitle.rectTransform, 24f * S, 47f * S, w - 48f * S, 30f * S);
            var escuchar = BotonPrincipal(interact.transform, "ESCUCHAR EXPLICACIÓN", UIKit.Blue, icAudio, () => PlayNarration());
            UIKit.TopLeft(R(escuchar), 24f * S, 92f * S, w - 48f * S, 60f * S);
            var scan = BotonPrincipal(interact.transform, "EXPLORAR EN RA", b.Accent, figScan, () => ShowOnly(bookAR[book]));
            UIKit.TopLeft(R(scan), 24f * S, 165f * S, w - 48f * S, 60f * S);

            // Guardar refs para este libro específico (se re-vinculan a los campos "actuales" en ShowOnly)
            var refs = p.gameObject.AddComponent<DetalleRefs>();
            refs.Set(band, img, title, desc);
            refs.Content = content; refs.Learn = learn.rectTransform; refs.LearnShadow = learnShadow.rectTransform;
            refs.Coach = coach; refs.Interact = interact.rectTransform; refs.InteractShadow = interactShadow.rectTransform;
            refs.AnchoColumna = columnW;
            title.text = b.TopicTitle[0]; desc.text = b.Desc(0);
            AcomodarDetalle(refs);
            return p;
        }

        // El alto de "Aprende" depende del texto del tema, y "Interactúa" va debajo.
        void AcomodarDetalle(DetalleRefs r)
        {
            const float S = 2.77f;
            float ancho = r.Learn.sizeDelta.x - 48f * S;
            float tituloH = UIKit.Alto(r.Title, r.Title.text, ancho), descH = UIKit.Alto(r.Desc, r.Desc.text, ancho);
            float yTitulo = 50f * S, yDesc = yTitulo + tituloH + 13f * S, yCoach = yDesc + descH + 22f * S;
            UIKit.TopLeft(r.Title.rectTransform, 24f * S, yTitulo, ancho, tituloH + 4f);
            UIKit.TopLeft(r.Desc.rectTransform, 24f * S, yDesc, ancho, descH + 4f);
            UIKit.TopLeft(r.Coach, 24f * S, yCoach, ancho, 66f * S);

            float altoLearn = yCoach + 66f * S + 24f * S;
            float cambio = altoLearn - r.Learn.sizeDelta.y;
            r.Learn.sizeDelta = new Vector2(r.Learn.sizeDelta.x, altoLearn);
            r.LearnShadow.sizeDelta += new Vector2(0, cambio);
            r.Interact.anchoredPosition += new Vector2(0, -cambio);
            r.InteractShadow.anchoredPosition += new Vector2(0, -cambio);
            r.Content.sizeDelta = new Vector2(r.AnchoColumna, -r.Interact.anchoredPosition.y + r.Interact.sizeDelta.y + 28f * S);
        }

        void RefreshDetalle()
        {
            if (bookDetalle == null || currentBook < 0 || currentBook >= bookDetalle.Length) return;
            var p = bookDetalle[currentBook];
            var refs = p.GetComponent<DetalleRefs>();
            if (refs == null) return;
            var b = books[currentBook];
            refs.Title.text = b.TopicTitle[currentTopic];
            refs.Desc.text = b.Desc(currentTopic);
            AcomodarDetalle(refs);
            refs.Band.color = b.Accent;
            detTitle = refs.Title; detDesc = refs.Desc; detBand = refs.Band; detImg = refs.Img;
            var msgs = AppState.MarkTopicSeen(currentBook, currentTopic);
            if (msgs.Count > 0) ShowReward(string.Join("\n", msgs));
        }

        RectTransform BuildLibroAR(int book)
        {
            var b = books[book];
            var p = BuildArScreenCore(book + "_AR", () => book < modelViewers.Length ? modelViewers[book] : null, () => ShowOnly(bookDetalle[book]));
            return p;
        }

        RectTransform BuildScanAR()
        {
            return BuildArScreenCore("5D_ScanAR", () => null, () => ShowOnly(pMenu));
        }
    }
}
