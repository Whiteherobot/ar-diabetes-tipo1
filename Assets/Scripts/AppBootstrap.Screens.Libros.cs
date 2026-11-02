using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    // Pantallas 5A/5B/5C - LIBROS: Temas -> Detalle (la Experiencia AR en sí vive en AppBootstrap.AR.cs).
    public partial class AppBootstrap
    {
        static readonly Color[] TopicAccent = { UIKit.Clin, UIKit.Fisio, UIKit.Nutri, UIKit.Prog };

        RectTransform BuildLibroTemas(int book)
        {
            const float S = 2.77f;
            var b = books[book];
            var p = Panel(book + "_Temas");
            Sprite headerIcon = book == 0 ? icPancreas : book == 1 ? icApple : b.HeroIcon;
            var header = BuildHeaderBar(p, b.Title, b.Accent, headerIcon, () => ShowOnly(pMundos));
            var robot = UIKit.Img(header, book == 0 ? robotCoral : book == 1 ? robotVerde : robotAzul, "Mascot");
            robot.preserveAspect = true;
            UIKit.Top(robot.rectTransform, 390f, 12f * S, 55f * S, 70f * S);

            var viewport = UIKit.Node("TopicsViewport", p);
            viewport.anchorMin = Vector2.zero; viewport.anchorMax = Vector2.one;
            viewport.offsetMin = new Vector2(0, 82f * S); viewport.offsetMax = new Vector2(0, -90f * S);
            viewport.gameObject.AddComponent<RectMask2D>();
            var catcher = viewport.gameObject.AddComponent<Image>();
            catcher.color = new Color(1, 1, 1, 0); catcher.raycastTarget = true;
            var content = UIKit.Node("TopicsContent", viewport);
            content.anchorMin = new Vector2(.5f, 1); content.anchorMax = new Vector2(.5f, 1);
            content.pivot = new Vector2(.5f, 1); content.anchoredPosition = Vector2.zero;
            float columnW = Land ? 1000f : 1080f;
            float y = 24f * S;
            float heroW = columnW - 32f * S, heroH = 215f * S;
            var hero = UIKit.Box(content, b.Accent, Mathf.RoundToInt(27f * S), "WorldHero");
            UIKit.Top(hero.rectTransform, 0, y, heroW, heroH);
            hero.gameObject.AddComponent<Mask>();
            UIKit.AddShadow(hero, Mathf.RoundToInt(27f * S), .14f, -8, 8);
            var kicker = UIKit.Text(hero.transform, "EXPLORA Y DESCUBRE", 13f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.Frac(kicker, .06f, .78f, .75f, .94f);
            var worldTitle = UIKit.Text(hero.transform, b.Title.Replace("Libro", "Mundo"), 27f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.Frac(worldTitle, .06f, .48f, .72f, .78f);
            var worldSub = UIKit.Text(hero.transform, "Elige un tema y comienza una nueva aventura.", 15f * S, Color.white, TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Frac(worldSub, .06f, .18f, .72f, .49f);
            var heroRobot = UIKit.Img(hero.transform, book == 0 ? robotCoral : book == 1 ? robotVerde : robotAzul, "Robot");
            heroRobot.preserveAspect = true;
            UIKit.Frac(heroRobot, .68f, -.10f, .98f, .78f);
            hero.gameObject.AddComponent<Appear>().delay = 0f;
            y += heroH + 25f * S;

            for (int i = 0; i < 4; i++)
            {
                int idx = i;
                Color acc = i == 0 ? UIKit.Blue : i == 1 ? UIKit.Fisio : i == 2 ? UIKit.Nutri : UIKit.Prog;
                var btn = UIKit.Button(content, "", UIKit.Card, UIKit.Navy, Mathf.RoundToInt(20f * S),
                    () => { currentTopic = idx; ShowOnly(bookDetalle[book]); }, 30);
                UIKit.Top(R(btn), 0, y, columnW - 40f * S, 126f * S);
                UIKit.AddShadow(btn.GetComponent<Image>(), Mathf.RoundToInt(20f * S), .12f, -6, 6);
                var stripe = UIKit.Box(btn.transform, acc, Mathf.RoundToInt(5f * S), "Stripe");
                UIKit.Top(stripe.rectTransform, -(columnW - 40f * S) / 2f + 3f * S, 18f * S, 5f * S, 90f * S);
                var disc = UIKit.Img(btn.transform, UIKit.Circle(), "Disc"); disc.color = acc;
                var tic = UIKit.Img(btn.transform, b.TopicIcon[i], "Ic"); tic.color = Color.white;
                UIKit.Frac(disc, .04f, .18f, .26f, .82f);
                UIKit.Frac(tic, .095f, .33f, .205f, .67f);
                var tl = UIKit.Text(btn.transform, b.TopicTitle[i], 18f * S, UIKit.Navy, TextAlignmentOptions.Left);
                UIKit.Frac(tl, .31f, .46f, .87f, .82f);
                var sb = UIKit.Text(btn.transform, b.TopicSub[i], 15f * S, UIKit.Muted, TextAlignmentOptions.Left, FontStyles.Normal);
                UIKit.Frac(sb, .31f, .16f, .87f, .44f);
                var chev = UIKit.Img(btn.transform, figChevron, "Chevron"); chev.color = acc; chev.preserveAspect = true;
                UIKit.Frac(chev, .88f, .30f, .97f, .70f);
                Anim.TarjetaSube(btn, .05f * i);
                y += 126f * S + 17f * S;
            }
            content.sizeDelta = new Vector2(columnW, y + 20f * S);
            BuildBottomNav(p, -1);
            return p;
        }

        RectTransform BuildLibroDetalle(int book)
        {
            const float S = 2.77f;
            var b = books[book];
            var p = Panel(book + "_Detalle");
            Sprite headerIcon = book == 0 ? icPancreas : book == 1 ? icApple : b.HeroIcon;
            var header = BuildHeaderBar(p, b.Title, b.Accent, headerIcon, () => ShowOnly(bookTemas[book]));
            var viewport = UIKit.Node("DetailViewport", p);
            viewport.anchorMin = Vector2.zero; viewport.anchorMax = Vector2.one;
            viewport.offsetMin = new Vector2(0, 82f * S); viewport.offsetMax = new Vector2(0, -90f * S);
            viewport.gameObject.AddComponent<RectMask2D>();
            var catcher = viewport.gameObject.AddComponent<Image>(); catcher.color = new Color(1, 1, 1, 0); catcher.raycastTarget = true;
            var content = UIKit.Node("DetailContent", viewport);
            content.anchorMin = new Vector2(.5f, 1); content.anchorMax = new Vector2(.5f, 1);
            content.pivot = new Vector2(.5f, 1); content.anchoredPosition = Vector2.zero;
            float columnW = Land ? 1000f : 1080f, y = 20f * S, w = columnW - 40f * S;

            var discover = UIKit.Box(content, b.Accent, Mathf.RoundToInt(28f * S), "Discover");
            UIKit.Top(discover.rectTransform, 0, y, w, 290f * S); discover.gameObject.AddComponent<Mask>();
            UIKit.AddShadow(discover, Mathf.RoundToInt(28f * S), .13f, -8, 8);
            var discoverLabel = UIKit.Text(discover.transform, "DESCUBRE", 13f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.Frac(discoverLabel, .06f, .86f, .75f, .98f);
            var band = discover;
            var img = UIKit.Img(band.transform, b.HeroIcon, "Img"); img.preserveAspect = true;
            UIKit.Frac(img, .25f, .14f, .75f, .92f);
            y += 290f * S + 20f * S;
            var learn = UIKit.Box(content, UIKit.Card, Mathf.RoundToInt(24f * S), "Learn");
            UIKit.Top(learn.rectTransform, 0, y, w, 330f * S); UIKit.AddShadow(learn, Mathf.RoundToInt(24f * S), .10f, -7, 7);
            var learnLabel = UIKit.Text(learn.transform, "APRENDE", 13f * S, b.Accent, TextAlignmentOptions.Left);
            UIKit.Frac(learnLabel, .05f, .86f, .70f, .98f);
            var title = UIKit.Text(learn.transform, "", 29f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Frac(title, .05f, .67f, .95f, .84f);
            var desc = UIKit.Text(learn.transform, "", 17f * S, UIKit.Muted, TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Frac(desc, .05f, .29f, .95f, .67f);
            var coachRobot = UIKit.Img(learn.transform, book == 0 ? robotCoral : book == 1 ? robotVerde : robotAzul, "Coach");
            coachRobot.preserveAspect = true; UIKit.Frac(coachRobot, .08f, .03f, .20f, .25f);
            var bubble = UIKit.Box(learn.transform, UIKit.Hex("EDF8FF"), Mathf.RoundToInt(16f * S), "Bubble");
            UIKit.Frac(bubble.rectTransform, .20f, .04f, .92f, .22f);
            var bubbleText = UIKit.Text(bubble.transform, "¡Lo estás haciendo genial!", 15f * S, UIKit.Nutri, TextAlignmentOptions.Center);
            UIKit.Stretch(bubbleText.rectTransform);
            y += 330f * S + 20f * S;
            var interact = UIKit.Box(content, UIKit.Card, Mathf.RoundToInt(24f * S), "Interact");
            UIKit.Top(interact.rectTransform, 0, y, w, 280f * S); UIKit.AddShadow(interact, Mathf.RoundToInt(24f * S), .10f, -7, 7);
            var interactLabel = UIKit.Text(interact.transform, "INTERACTÚA", 13f * S, b.Accent, TextAlignmentOptions.Left);
            UIKit.Frac(interactLabel, .05f, .84f, .70f, .98f);
            var interactTitle = UIKit.Text(interact.transform, "Elige cómo quieres descubrirlo", 21f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Frac(interactTitle, .05f, .67f, .95f, .84f);
            var escuchar = UIKit.Button(interact.transform, "ESCUCHAR EXPLICACIÓN", UIKit.Blue, Color.white, Mathf.RoundToInt(16f * S), () => PlayNarration(), Mathf.RoundToInt(15f * S));
            var g = UIKit.Img(R(escuchar), icAudio, "Ico"); g.color = Color.white;
            UIKit.Frac(R(escuchar), .05f, .40f, .95f, .62f);
            UIKit.Frac(g, .10f, .45f, .18f, .75f);
            var scan = UIKit.Button(interact.transform, "EXPLORAR EN RA", b.Accent, Color.white, Mathf.RoundToInt(16f * S),
                () => ShowOnly(bookAR[book]), Mathf.RoundToInt(15f * S));
            var scanIcon = UIKit.Img(R(scan), figScan, "Ico"); scanIcon.color = Color.white; scanIcon.preserveAspect = true;
            UIKit.Frac(R(scan), .05f, .17f, .95f, .39f);
            UIKit.Frac(scanIcon, .10f, .22f, .18f, .52f);
            UIKit.AddShadow(scan.GetComponent<Image>(), Mathf.RoundToInt(16f * S), .14f, -6, 6);
            content.sizeDelta = new Vector2(columnW, y + 280f * S + 25f * S);

            // Guardar refs para este libro específico (se re-vinculan a los campos "actuales" en ShowOnly)
            p.gameObject.AddComponent<DetalleRefs>().Set(band, img, title, desc);
            return p;
        }

        void RefreshDetalle()
        {
            var p = bookDetalle[currentBook];
            var refs = p.GetComponent<DetalleRefs>();
            if (refs == null) return;
            var b = books[currentBook];
            refs.Title.text = b.TopicTitle[currentTopic];
            refs.Desc.text = b.Desc(currentTopic, AppState.Age);
            refs.Band.color = b.Accent;
            detTitle = refs.Title; detDesc = refs.Desc; detBand = refs.Band; detImg = refs.Img;
            var msgs = AppState.MarkTopicSeen(currentBook, currentTopic);
            if (msgs.Count > 0) ShowReward(string.Join("\n", msgs));
        }

        RectTransform BuildLibroAR(int book)
        {
            var b = books[book];
            var p = BuildArScreenCore(book + "_AR", () => modelViewers[book], () => ShowOnly(bookDetalle[book]));
            return p;
        }

        RectTransform BuildScanAR()
        {
            return BuildArScreenCore("5D_ScanAR", () => null, () => ShowOnly(pMenu));
        }
    }
}
