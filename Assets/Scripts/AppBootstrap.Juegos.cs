using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    // Pantalla 6) JUEGOS Y RETOS: hub por libro + quiz de 3-4 preguntas (Choice/Matching/MultiSelect).
    public partial class AppBootstrap
    {
        // "Libro Fisiológico" -> "Misión Fisiológica" (concordancia como en el diseño)
        static string MisionNombre(string libro)
        {
            return "Misión " + libro.Replace("Libro ", "").Replace("Fisiológico", "Fisiológica").Replace("Clínico", "Clínica");
        }

        RectTransform BuildJuegos()
        {
            var p = Panel("6_Juegos");
            const float S = 2.77f;
            bool land = Land;
            var viewport = UIKit.Node("GamesViewport", p);
            viewport.anchorMin = Vector2.zero; viewport.anchorMax = Vector2.one;
            viewport.offsetMin = new Vector2(0, 82f * S); viewport.offsetMax = Vector2.zero;
            viewport.gameObject.AddComponent<RectMask2D>();
            var vi = viewport.gameObject.AddComponent<Image>(); vi.color = new Color(1, 1, 1, 0);
            var content = UIKit.Node("GamesContent", viewport);
            content.anchorMin = new Vector2(.5f, 1); content.anchorMax = new Vector2(.5f, 1);
            content.pivot = new Vector2(.5f, 1); float w = land ? 1000 : 1080; content.sizeDelta = new Vector2(w, 2200);
            var scroll = viewport.gameObject.AddComponent<ScrollRect>();
            scroll.content = content; scroll.viewport = viewport; scroll.horizontal = false; scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Elastic; scroll.inertia = true;
            float y = 0;
            var hero = UIKit.Box(content, UIKit.Juegos, 0, "GamesHero"); UIKit.Top(hero.rectTransform, 0, y, w, 310f * S); hero.gameObject.AddComponent<Mask>();
            var blob = UIKit.Img(hero.transform, UIKit.Circle(), "Blob"); blob.color = new Color(1, 1, 1, .12f); UIKit.Top(blob.rectTransform, w / 2f - 85f * S, -55f * S, 190f * S, 190f * S);
            var kicker = UIKit.Text(hero.transform, "MISIONES DEL CONOCIMIENTO", 12f * S, Color.white, TextAlignmentOptions.Left); kicker.fontStyle = FontStyles.Bold; UIKit.TopLeft(kicker.rectTransform, 22f * S, 40f * S, 345f * S, 26f * S);
            var heroTitle = UIKit.Text(hero.transform, "¡Vamos a jugar!", 31f * S, Color.white, TextAlignmentOptions.Left); UIKit.TopLeft(heroTitle.rectTransform, 22f * S, 78f * S, 390f * S, 50f * S);
            var heroSub = UIKit.Text(hero.transform, "Completa misiones y descubre cuánto has aprendido.", 17f * S, Color.white, TextAlignmentOptions.Left, FontStyles.Normal); UIKit.TopLeft(heroSub.rectTransform, 22f * S, 135f * S, 205f * S, 70f * S);
            var heroRobots = BuildRobotGroup(hero.transform); UIKit.TopLeft(heroRobots, w - 315f * S, 205f * S, 330f * S, 150f * S);
            var spark = UIKit.Img(hero.transform, spStar, "Star"); spark.color = UIKit.Hex("FFF1A8"); spark.preserveAspect = true; UIKit.Top(spark.rectTransform, w / 2f + 220f * S, 38f * S, 34f * S, 34f * S);
            y += 340f * S;
            var missionTitle = UIKit.Text(content, "Elige tu misión", 24f * S, UIKit.Navy, TextAlignmentOptions.Left); UIKit.Top(missionTitle.rectTransform, 0, y, w - 40f * S, 42f * S); y += 67f * S;
            var count = new TMP_Text[3];
            var missionFills = new Image[3];
            for (int i = 0; i < 3; i++)
            {
                var b = books[i];
                int idx = i;
                var btn = UIKit.Button(content, "", b.Accent, Color.white, 30, () => { quizBook = idx; ShowOnly(bookQuiz[idx]); });
                float cardW = w - 40f * S;
                UIKit.Top(R(btn), 0, y, cardW, 240f * S);
                btn.gameObject.AddComponent<Mask>();
                UIKit.AddShadow(btn.GetComponent<Image>(), 30, 0.12f, -6, 6);
                btn.gameObject.AddComponent<Appear>().delay = 0.05f * i;
                var kickerM = UIKit.Text(btn.transform, "MISIÓN " + (i + 1), 12f * S, Color.white, TextAlignmentOptions.Left); UIKit.TopLeft(kickerM.rectTransform, 22f * S, 25f * S, 190f * S, 26f * S);
                var tl = UIKit.Text(btn.transform, MisionNombre(b.Title), 22f * S, Color.white, TextAlignmentOptions.Left); UIKit.TopLeft(tl.rectTransform, 22f * S, 53f * S, 215f * S, 34f * S); tl.enableWordWrapping = false;
                var descs = new[] { "Descubre cómo funciona tu cuerpo", "Explora el mundo de los alimentos", "Aprende a cuidar tu salud" };
                var desc = UIKit.Text(btn.transform, descs[i], 15f * S, Color.white, TextAlignmentOptions.Left, FontStyles.Normal); UIKit.TopLeft(desc.rectTransform, 22f * S, 105f * S, 170f * S, 45f * S);
                var cnt = UIKit.Text(btn.transform, "Lista para comenzar", 13f * S, Color.white, TextAlignmentOptions.Left, FontStyles.Bold); UIKit.TopLeft(cnt.rectTransform, 22f * S, 139f * S, 260f * S, 25f * S);
                var missionFill = ProgressBar(btn.transform, 0f, .06f, .28f, .57f, .35f, Color.white, .8f);
                var play = UIKit.Button(btn.transform, "JUGAR", Color.white, b.Accent, Mathf.RoundToInt(14f * S), () => { quizBook = idx; ShowOnly(bookQuiz[idx]); }, Mathf.RoundToInt(14f * S), true); UIKit.TopLeft(R(play), 22f * S, 188f * S, 110f * S, 40f * S);
                var playIcon = UIKit.Img(play.transform, figPlay, "Play"); playIcon.color = b.Accent; playIcon.preserveAspect = true; UIKit.Top(playIcon.rectTransform, 35f * S, 202f * S, 18f * S, 18f * S);
                var missionRobot = UIKit.Img(btn.transform, i == 0 ? robotCoral : (i == 1 ? robotVerde : robotAzul), "Robot"); missionRobot.preserveAspect = true;
                UIKit.TopLeft(missionRobot.rectTransform, cardW - 145f * S, 30f * S, 145f * S, 205f * S);
                var reward = UIKit.Box(btn.transform, Color.white, 99, "Reward"); UIKit.TopLeft(reward.rectTransform, cardW - 95f * S, 15f * S, 75f * S, 42f * S);
                var rewardIcon = UIKit.Img(reward.transform, spStar, "Star"); rewardIcon.color = UIKit.Prog; rewardIcon.preserveAspect = true; UIKit.Frac(rewardIcon, .12f, .20f, .40f, .80f);
                var rewardText = UIKit.Text(reward.transform, "15", 16f * S, UIKit.Hex("654D13"), TextAlignmentOptions.Right); UIKit.Frac(rewardText, .42f, .05f, .92f, .95f);
                count[i] = cnt;
                missionFills[i] = missionFill;
                y += 258f * S;
            }
            p.gameObject.AddComponent<JuegosRefs>().Set(count, missionFills);
            BuildBottomNav(p, -1);
            return p;
        }

        void RefreshJuegos()
        {
            if (pJuegos == null) return;
            var refs = pJuegos.GetComponent<JuegosRefs>();
            if (refs == null) return;
            for (int i = 0; i < 3; i++)
            {
                if (refs.Count[i] != null) refs.Count[i].text = AppState.QuizCorrectCount(i) + "/" + books[i].Quiz.Length + " correctas";
                if (refs.Fill != null && refs.Fill[i] != null)
                    SetProgress(refs.Fill[i], (float)AppState.QuizCorrectCount(i) / books[i].Quiz.Length);
            }
        }

        static readonly Color GreenOk = UIKit.Hex("DFF7E9");
        static readonly Color RedBad = UIKit.Hex("FDE5E5");
        static readonly Color BlueSel = UIKit.Hex("EAF5FF");

        RectTransform BuildQuiz(int book)
        {
            var b = books[book];
            var p = Panel(book + "_Quiz");
            bool land = Land;
            BuildHeaderBar(p, MisionNombre(b.Title), b.Accent, b.HeroIcon, () => ShowOnly(pJuegos));

            // Cuántas opciones/pares necesita CADA tipo de juego en este libro (0 = ese tipo no
            // aparece en este libro). Se construye solo lo que hace falta, ya posicionado y con
            // su sombra final — así se evita el bug de sombras "flotantes" por reposicionar
            // después de llamar AddShadow.
            int mcCount = 0, pairCount = 0, msCount = 0;
            foreach (var g in b.Quiz)
            {
                if (g.Type == GameType.Choice) mcCount = Mathf.Max(mcCount, g.Options.Length);
                else if (g.Type == GameType.Matching) pairCount = Mathf.Max(pairCount, g.Left.Length);
                else if (g.Type == GameType.MultiSelect) msCount = Mathf.Max(msCount, g.Options.Length);
            }

            var progress = UIKit.Text(p, "", 17f * 2.77f, UIKit.Navy, TextAlignmentOptions.Left);
            var progressType = UIKit.Text(p, "", 14f * 2.77f, UIKit.Muted, TextAlignmentOptions.Right, FontStyles.Bold);
            var progressFill = ProgressBar(p, 0f, .06f, .755f, .94f, .77f, b.Accent, .4f);
            var card = UIKit.Box(p, UIKit.Card, 36, "QCard");
            var coachBubble = UIKit.Box(p, Color.white, Mathf.RoundToInt(4f * 2.77f), "CoachBubble");
            UIKit.Frac(coachBubble.rectTransform, 0.20f, 0.62f, 0.87f, 0.71f);
            var coach = UIKit.Text(coachBubble.transform, "¡Tú puedes, lee con calma!", 15f * 2.77f, UIKit.Muted, TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Stretch(coach.rectTransform, 18f);
            var coachRobot = UIKit.Img(p, book == 0 ? robotCoral : (book == 1 ? robotVerde : robotAzul), "Coach"); coachRobot.preserveAspect = true;
            UIKit.Frac(coachRobot, 0.06f, 0.61f, 0.19f, 0.75f);
            var questionType = UIKit.Text(card.transform, "", 13f * 2.77f, b.Accent, TextAlignmentOptions.Left, FontStyles.Bold);
            UIKit.Frac(questionType, 0.08f, 0.70f, 0.92f, 0.90f);
            var question = UIKit.Text(card.transform, "", 21f * 2.77f, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Frac(question, 0.08f, 0.12f, 0.92f, 0.68f);
            var questionAudio = UIKit.Button(card.transform, "", UIKit.Hex("EAF5FF"), UIKit.Blue, Mathf.RoundToInt(15f * 2.77f), null, 15, false);
            UIKit.Frac(R(questionAudio), .80f, .72f, .95f, .95f);
            var questionAudioIcon = UIKit.Img(questionAudio.transform, figSpeaker != null ? figSpeaker : icAudio, "Speaker");
            questionAudioIcon.color = UIKit.Blue; questionAudioIcon.preserveAspect = true; UIKit.Stretch(questionAudioIcon.rectTransform, 10f);

            float progY0, progY1, cardY0, cardY1, ansY0, ansY1, ax0, ax1;
            if (!land) { progY0 = 0.78f; progY1 = 0.83f; cardY0 = 0.46f; cardY1 = 0.64f; ansY0 = 0.05f; ansY1 = 0.42f; ax0 = 0.08f; ax1 = 0.92f; }
            else { progY0 = 0.76f; progY1 = 0.82f; cardY0 = 0.48f; cardY1 = 0.70f; ansY0 = 0.07f; ansY1 = 0.43f; ax0 = 0.14f; ax1 = 0.86f; }
            UIKit.Frac(progress, ax0, progY0, ax1 - 0.30f, progY1);
            UIKit.Frac(progressType, ax0 + 0.32f, progY0, ax1, progY1);
            UIKit.Frac(card.rectTransform, 0.06f, cardY0, 0.94f, cardY1);
            Anim.PreguntaEntra(card);

            // ---- Grupo Choice (opción múltiple / verdadero-falso / completar frase) ----
            var mcGroup = UIKit.Node("McGroup", p);
            UIKit.Stretch(mcGroup);
            var mcBtn = new Button[mcCount]; var mcImg = new Image[mcCount]; var mcLbl = new TMP_Text[mcCount];
            if (mcCount > 0)
            {
                float gap = land ? 0.02f : 0.025f;
                float oh = (ansY1 - ansY0 - gap * (mcCount - 1)) / mcCount;
                for (int i = 0; i < mcCount; i++)
                {
                    var btn = UIKit.Button(mcGroup, "", UIKit.Card, UIKit.Navy, 30, null);
                    var lbl = UIKit.Text(btn.transform, "", 16f * 2.77f, UIKit.Navy, TextAlignmentOptions.Left);
                    UIKit.Frac(lbl, 0.05f, 0.04f, 0.95f, 0.96f);
                    float top = ansY1 - i * (oh + gap);
                    UIKit.Frac(R(btn), ax0, top - oh, ax1, top);
                    mcBtn[i] = btn; mcImg[i] = btn.GetComponent<Image>(); mcLbl[i] = lbl;
                }
            }

            // ---- Grupo Matching (relacionar, tap-to-pair) ----
            var matchGroup = UIKit.Node("MatchGroup", p);
            UIKit.Stretch(matchGroup);
            var leftBtn = new Button[pairCount]; var leftImg = new Image[pairCount]; var leftLbl = new TMP_Text[pairCount];
            var rightBtn = new Button[pairCount]; var rightImg = new Image[pairCount]; var rightLbl = new TMP_Text[pairCount];
            if (pairCount > 0)
            {
                float gap = 0.018f;
                float rh = (ansY1 - ansY0 - gap * (pairCount - 1)) / pairCount;
                float lx0 = land ? 0.10f : 0.05f, lx1 = 0.47f, rx0 = 0.53f, rx1 = land ? 0.90f : 0.95f;
                for (int i = 0; i < pairCount; i++)
                {
                    float top = ansY1 - i * (rh + gap);
                    var lb = UIKit.Button(matchGroup, "", UIKit.Card, UIKit.Navy, 24, null, 26);
                    var ll = UIKit.Text(lb.transform, "", 16f * 2.77f, UIKit.Navy, TextAlignmentOptions.Center, FontStyles.Bold);
                    UIKit.Frac(ll, 0.03f, 0.02f, 0.97f, 0.98f);
                    UIKit.Frac(R(lb), lx0, top - rh, lx1, top);
                    leftBtn[i] = lb; leftImg[i] = lb.GetComponent<Image>(); leftLbl[i] = ll;

                    var rb = UIKit.Button(matchGroup, "", UIKit.Card, UIKit.Navy, 24, null, 26);
                    var rl = UIKit.Text(rb.transform, "", 16f * 2.77f, UIKit.Navy, TextAlignmentOptions.Center, FontStyles.Bold);
                    UIKit.Frac(rl, 0.03f, 0.02f, 0.97f, 0.98f);
                    UIKit.Frac(R(rb), rx0, top - rh, rx1, top);
                    rightBtn[i] = rb; rightImg[i] = rb.GetComponent<Image>(); rightLbl[i] = rl;
                }
            }

            // ---- Grupo MultiSelect (grilla de opciones + botón Confirmar) ----
            var msGroup = UIKit.Node("MsGroup", p);
            UIKit.Stretch(msGroup);
            var msBtn = new Button[msCount]; var msImg = new Image[msCount]; var msLbl = new TMP_Text[msCount];
            Button msConfirm = null; Image msConfirmImg = null;
            if (msCount > 0)
            {
                int cols = 2, rows = Mathf.CeilToInt(msCount / 2f);
                float confirmH = 0.09f, gridY0 = ansY0 + confirmH + 0.03f, gridY1 = ansY1;
                for (int i = 0; i < msCount; i++)
                {
                    var rc = UIKit.Cell(i, cols, rows, ax0, gridY0, ax1, gridY1, 0.03f, 0.02f);
                    var btn = UIKit.Button(msGroup, "", UIKit.Card, UIKit.Navy, 24, null, 28);
                    var lbl = UIKit.Text(btn.transform, "", 16f * 2.77f, UIKit.Navy, TextAlignmentOptions.Center, FontStyles.Bold);
                    UIKit.Stretch(lbl.rectTransform, 10);
                    UIKit.Frac(R(btn), rc.xMin, rc.yMin, rc.xMax, rc.yMax);
                    msBtn[i] = btn; msImg[i] = btn.GetComponent<Image>(); msLbl[i] = lbl;
                }
                msConfirm = UIKit.Button(msGroup, "Confirmar", b.Accent, Color.white, 32, null, 34);
                UIKit.Frac(R(msConfirm), ax0, ansY0, ax1, ansY0 + confirmH);
                msConfirmImg = msConfirm.GetComponent<Image>();
            }

            // AddShadow lee el rect ACTUAL del elemento para calcular la sombra: se llama después
            // de fijar la posición final (Frac) de cada grupo, ya construido a su tamaño real.
            UIKit.AddShadow(card, 36, 0.10f, -6, 6);
            for (int i = 0; i < mcCount; i++) UIKit.AddShadow(mcImg[i], 26, 0.10f, -5, 5);
            for (int i = 0; i < pairCount; i++) { UIKit.AddShadow(leftImg[i], 18, 0.10f, -4, 4); UIKit.AddShadow(rightImg[i], 18, 0.10f, -4, 4); }
            for (int i = 0; i < msCount; i++) UIKit.AddShadow(msImg[i], 20, 0.10f, -4, 4);
            if (msConfirmImg != null) UIKit.AddShadow(msConfirmImg, 24, 0.12f, -5, 5);

            var audioPanel = UIKit.Box(p, UIKit.Hex("DFEFFF"), Mathf.RoundToInt(17f * 2.77f), "QuizAudioControls");
            UIKit.Frac(audioPanel.rectTransform, .06f, .70f, .94f, .76f);
            var audioSpeaker = UIKit.Img(audioPanel.transform, figSpeaker != null ? figSpeaker : icAudio, "Speaker");
            audioSpeaker.color = UIKit.Hex("245D93"); audioSpeaker.preserveAspect = true; UIKit.Frac(audioSpeaker, .04f, .24f, .12f, .76f);
            var audioStatus = UIKit.Text(audioPanel.transform, "Escuchando", 14f * 2.77f, UIKit.Hex("24496D"), TextAlignmentOptions.Left);
            UIKit.Frac(audioStatus, .15f, .18f, .42f, .82f);
            var audioPulse = UIKit.Box(audioPanel.transform, UIKit.Blue, Mathf.RoundToInt(4f * 2.77f), "Pulse");
            UIKit.Frac(audioPulse.rectTransform, .44f, .46f, .60f, .54f);
            var pulseAnim = Anim.Pulso(audioPulse); pulseAnim.alterna = true;
            var audioToggle = UIKit.Button(audioPanel.transform, "", Color.white, UIKit.Hex("245D93"), Mathf.RoundToInt(13f * 2.77f), null, 13, false);
            UIKit.Frac(R(audioToggle), .64f, .12f, .82f, .88f);
            var audioToggleIcon = UIKit.Img(audioToggle.transform, figPause != null ? figPause : figPlay, "Toggle");
            audioToggleIcon.color = UIKit.Hex("245D93"); audioToggleIcon.preserveAspect = true; UIKit.Stretch(audioToggleIcon.rectTransform, 10f);
            var audioStop = UIKit.Button(audioPanel.transform, "", Color.white, UIKit.Hex("245D93"), Mathf.RoundToInt(13f * 2.77f), null, 13, false);
            UIKit.Frac(R(audioStop), .83f, .12f, .99f, .88f);
            var audioStopIcon = UIKit.Img(audioStop.transform, figStop != null ? figStop : figInfo, "Stop");
            audioStopIcon.color = UIKit.Hex("245D93"); audioStopIcon.preserveAspect = true; UIKit.Stretch(audioStopIcon.rectTransform, 10f);
            audioPanel.gameObject.SetActive(false);

            var feedback = UIKit.Box(p, UIKit.Hex("E8F8F0"), Mathf.RoundToInt(22f * 2.77f), "Feedback");
            UIKit.Frac(feedback.rectTransform, .04f, .03f, .96f, .30f);
            var feedbackOutline = feedback.gameObject.AddComponent<Outline>();
            feedbackOutline.effectDistance = new Vector2(2f * 2.77f, -2f * 2.77f);
            feedbackOutline.effectColor = UIKit.Hex("9BD7B8");
            Anim.Feedback(feedback);
            var feedbackIconBg = UIKit.Box(feedback.transform, Color.white, Mathf.RoundToInt(13f * 2.77f), "FeedbackIconBg");
            UIKit.Frac(feedbackIconBg.rectTransform, .05f, .60f, .17f, .90f);
            var feedbackIcon = UIKit.Img(feedbackIconBg.transform, figStar, "FeedbackIcon");
            feedbackIcon.color = UIKit.Prog; feedbackIcon.preserveAspect = true; UIKit.Stretch(feedbackIcon.rectTransform, 8f);
            var feedbackTitle = UIKit.Text(feedback.transform, "", 19f * 2.77f, UIKit.Hex("225D43"), TextAlignmentOptions.Left);
            UIKit.Frac(feedbackTitle, .21f, .72f, .74f, .92f);
            var feedbackText = UIKit.Text(feedback.transform, "", 16f * 2.77f, UIKit.Hex("225D43"), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.Frac(feedbackText, .21f, .48f, .74f, .72f);
            var feedbackRobot = UIKit.Img(feedback.transform, book == 0 ? robotCoral : (book == 1 ? robotVerde : robotAzul), "Robot");
            feedbackRobot.preserveAspect = true; UIKit.Frac(feedbackRobot, .77f, .50f, .96f, .96f);
            var feedbackAudio = UIKit.Button(feedback.transform, "", Color.white, UIKit.Blue, Mathf.RoundToInt(14f * 2.77f), null, 14, false);
            UIKit.Frac(R(feedbackAudio), .05f, .09f, .17f, .40f);
            var feedbackAudioIcon = UIKit.Img(feedbackAudio.transform, figSpeaker != null ? figSpeaker : icAudio, "Speaker");
            feedbackAudioIcon.color = UIKit.Blue; feedbackAudioIcon.preserveAspect = true; UIKit.Stretch(feedbackAudioIcon.rectTransform, 9f);
            var continueButton = UIKit.Button(feedback.transform, "CONTINUAR ›", UIKit.Blue, Color.white, Mathf.RoundToInt(15f * 2.77f), null, Mathf.RoundToInt(15f * 2.77f), false);
            UIKit.Frac(R(continueButton), .21f, .09f, .95f, .40f);
            feedback.gameObject.SetActive(false);

            // Overlay de resultado final (oculto hasta terminar los juegos del libro)
            var resultPanel = UIKit.Node("Result", p);
            UIKit.Stretch(resultPanel);
            var resBg = UIKit.Box(resultPanel, new Color(0.95f, 0.98f, 1f, 0.97f), 0, "Bg");
            UIKit.Stretch(resBg.rectTransform);
            var resultCard = UIKit.Box(resultPanel, Color.white, 24 * 2, "ResultCard");
            UIKit.Frac(resultCard, 0.06f, 0.18f, 0.94f, 0.82f); UIKit.AddShadow(resultCard, 48, .12f, -8, 8);
            var resultRobot = UIKit.Img(resultCard.transform, book == 0 ? robotCoral : (book == 1 ? robotVerde : robotAzul), "Robot"); resultRobot.preserveAspect = true; UIKit.Frac(resultRobot, 0.32f, 0.66f, 0.68f, 0.94f);
            Anim.Celebrar(resultRobot);
            var resultStar = UIKit.Img(resultCard.transform, figStar, "Star"); resultStar.color = UIKit.Prog; resultStar.preserveAspect = true; UIKit.Frac(resultStar, 0.43f, 0.56f, 0.57f, 0.66f);
            Anim.Estrella(resultStar);
            var resultTrack = UIKit.Box(resultCard.transform, UIKit.Hex("E2ECF5"), 12, "ResultTrack");
            UIKit.Frac(resultTrack.rectTransform, 0.12f, 0.30f, 0.88f, 0.34f);
            var resultFill = UIKit.Box(resultTrack.transform, b.Accent, 12, "ResultFill");
            UIKit.Frac(resultFill.rectTransform, 0f, 0f, 0f, 1f);
            Anim.Llenar(resultFill.GetComponent<Image>(), .7f);
            var resultText = UIKit.Text(resultCard.transform, "", 25f * 2.77f, UIKit.Navy);
            var retryBtn = UIKit.Button(resultCard.transform, "Volver a jugar", b.Accent, Color.white, 18 * 2, () => ShowOnly(p, false), 17 * 2);
            var backBtn = UIKit.Button(resultCard.transform, "Regresar a misiones", Color.white, UIKit.Navy, 18 * 2, () => ShowOnly(pJuegos), 16 * 2);
            if (!land)
            {
                UIKit.Frac(resultText, 0.12f, 0.36f, 0.88f, 0.63f);
                UIKit.Frac(R(retryBtn), 0.15f, 0.21f, 0.85f, 0.31f);
                UIKit.Frac(R(backBtn), 0.15f, 0.10f, 0.85f, 0.20f);
            }
            else
            {
                UIKit.Frac(resultText, 0.12f, 0.36f, 0.88f, 0.63f);
                UIKit.Frac(R(retryBtn), 0.30f, 0.24f, 0.70f, 0.36f);
                UIKit.Frac(R(backBtn), 0.30f, 0.10f, 0.70f, 0.22f);
            }
            resultPanel.gameObject.SetActive(false);

            var refs = p.gameObject.AddComponent<QuizRefs>();
            refs.Set(progress, progressType, progressFill, resultFill, question, resultText, mcGroup, mcBtn, mcImg, mcLbl,
                matchGroup, leftBtn, leftImg, leftLbl, rightBtn, rightImg, rightLbl,
                msGroup, msBtn, msImg, msLbl, msConfirm, resultPanel, feedback.rectTransform,
                feedbackTitle, feedbackText, feedbackIcon, feedbackAudio, continueButton, audioPanel.rectTransform,
                audioStatus, audioPulse, audioToggleIcon, audioToggle, audioStop, questionAudio);

            for (int i = 0; i < mcCount; i++) { int idx = i; mcBtn[i].onClick.AddListener(() => AnswerChoice(refs, idx)); }
            for (int i = 0; i < pairCount; i++)
            {
                int idx = i;
                leftBtn[i].onClick.AddListener(() => TapLeft(refs, idx));
                rightBtn[i].onClick.AddListener(() => TapRight(refs, idx));
            }
            for (int i = 0; i < msCount; i++) { int idx = i; msBtn[i].onClick.AddListener(() => ToggleMs(refs, idx)); }
            if (msConfirm != null) msConfirm.onClick.AddListener(() => ConfirmMs(refs));
            questionAudio.onClick.AddListener(() => PlayQuizAudio(refs));
            feedbackAudio.onClick.AddListener(() => PlayQuizAudio(refs));
            continueButton.onClick.AddListener(() => ContinueQuiz(refs));
            audioToggle.onClick.AddListener(() => ToggleQuizAudio(refs));
            audioStop.onClick.AddListener(() => StopQuizAudio(refs));
            return p;
        }

        void RefreshQuiz()
        {
            if (bookQuiz == null || quizBook < 0 || quizBook >= bookQuiz.Length || bookQuiz[quizBook] == null) return;
            quizQ = 0; quizScore = 0;
            var refs = bookQuiz[quizBook].GetComponent<QuizRefs>();
            if (refs == null) return;
            refs.ResultPanel.gameObject.SetActive(false);
            refs.FeedbackPanel.gameObject.SetActive(false);
            refs.AudioPanel.gameObject.SetActive(false);
            if (audioSrc != null) audioSrc.Stop();
            ShowQuizQuestion(refs);
        }

        void ShowQuizQuestion(QuizRefs refs)
        {
            var qz = books[quizBook].Quiz[quizQ];
            refs.FeedbackPanel.gameObject.SetActive(false);
            refs.AudioPanel.gameObject.SetActive(false);
            refs.QuestionAudio.gameObject.SetActive(QuizNarration() != null);
            refs.Question.text = qz.Q;
            refs.Progress.text = "Pregunta " + (quizQ + 1) + " de " + books[quizBook].Quiz.Length;
            if (refs.ProgressType != null)
                refs.ProgressType.text = qz.Type == GameType.Choice ? "Opción múltiple" :
                    (qz.Type == GameType.Matching ? "Relaciona" : "Elige todas las correctas");
            if (refs.ProgressFill != null)
                SetProgress(refs.ProgressFill, (quizQ + 1f) / books[quizBook].Quiz.Length);
            refs.McGroup.gameObject.SetActive(qz.Type == GameType.Choice);
            refs.MatchGroup.gameObject.SetActive(qz.Type == GameType.Matching);
            refs.MsGroup.gameObject.SetActive(qz.Type == GameType.MultiSelect);

            if (qz.Type == GameType.Choice)
            {
                quizAnswered = false;
                for (int i = 0; i < refs.McButtons.Length; i++)
                {
                    bool active = i < qz.Options.Length;
                    refs.McButtons[i].gameObject.SetActive(active);
                    if (!active) continue;
                    refs.McLabels[i].text = qz.Options[i];
                    refs.McImages[i].color = UIKit.Card;
                    refs.McButtons[i].interactable = true;
                }
            }
            else if (qz.Type == GameType.Matching)
            {
                int n = qz.Left.Length;
                matchRightOrder = new int[n];
                for (int i = 0; i < n; i++) matchRightOrder[i] = i;
                for (int i = n - 1; i > 0; i--) // Fisher-Yates: orden visual de la columna derecha
                {
                    int j = UnityEngine.Random.Range(0, i + 1);
                    int tmp = matchRightOrder[i]; matchRightOrder[i] = matchRightOrder[j]; matchRightOrder[j] = tmp;
                }
                matchDoneLeft = new bool[n]; matchDoneRight = new bool[n];
                matchSelected = -1; matchCount = 0; matchLocked = false;
                for (int i = 0; i < refs.LeftButtons.Length; i++)
                {
                    bool active = i < n;
                    refs.LeftButtons[i].gameObject.SetActive(active);
                    refs.RightButtons[i].gameObject.SetActive(active);
                    if (!active) continue;
                    refs.LeftLabels[i].text = qz.Left[i];
                    refs.RightLabels[i].text = qz.Right[matchRightOrder[i]];
                    refs.LeftImages[i].color = UIKit.Card;
                    refs.RightImages[i].color = UIKit.Card;
                    refs.LeftButtons[i].interactable = true;
                    refs.RightButtons[i].interactable = true;
                }
            }
            else // MultiSelect
            {
                int n = qz.Options.Length;
                msSelected = new bool[n]; msAnswered = false;
                for (int i = 0; i < refs.MsButtons.Length; i++)
                {
                    bool active = i < n;
                    refs.MsButtons[i].gameObject.SetActive(active);
                    if (!active) continue;
                    refs.MsLabels[i].text = qz.Options[i];
                    refs.MsImages[i].color = UIKit.Card;
                    refs.MsButtons[i].interactable = true;
                }
                if (refs.MsConfirm != null) refs.MsConfirm.interactable = true;
            }
        }

        void AnswerChoice(QuizRefs refs, int optIdx)
        {
            if (quizAnswered) return;
            quizAnswered = true;
            var qz = books[quizBook].Quiz[quizQ];
            bool correct = optIdx == qz.Correct;
            refs.McImages[optIdx].color = correct ? GreenOk : RedBad;
            if (!correct) refs.McImages[qz.Correct].color = GreenOk;
            for (int i = 0; i < qz.Options.Length; i++) refs.McButtons[i].interactable = false;
            RegisterAnswer(correct);
            ShowFeedback(refs, correct);
        }

        void TapLeft(QuizRefs refs, int idx)
        {
            if (matchLocked || matchDoneLeft[idx]) return;
            matchSelected = idx;
            for (int i = 0; i < matchDoneLeft.Length; i++)
                if (!matchDoneLeft[i]) refs.LeftImages[i].color = i == idx ? BlueSel : UIKit.Card;
        }

        void TapRight(QuizRefs refs, int row)
        {
            if (matchLocked || matchDoneRight[row] || matchSelected < 0) return;
            var qz = books[quizBook].Quiz[quizQ];
            int leftIdx = matchSelected;
            bool correct = matchRightOrder[row] == leftIdx;
            if (correct)
            {
                matchDoneLeft[leftIdx] = true; matchDoneRight[row] = true;
                refs.LeftImages[leftIdx].color = GreenOk; refs.RightImages[row].color = GreenOk;
                refs.LeftButtons[leftIdx].interactable = false; refs.RightButtons[row].interactable = false;
                matchSelected = -1; matchCount++;
                if (matchCount >= qz.Left.Length)
                {
                    RegisterAnswer(true);
                    ShowFeedback(refs, true);
                }
            }
            else
            {
                StartCoroutine(FlashWrongMatch(refs, leftIdx, row));
            }
        }

        IEnumerator FlashWrongMatch(QuizRefs refs, int leftIdx, int row)
        {
            matchLocked = true;
            refs.LeftImages[leftIdx].color = RedBad;
            refs.RightImages[row].color = RedBad;
            yield return new WaitForSeconds(0.45f);
            if (!matchDoneLeft[leftIdx]) refs.LeftImages[leftIdx].color = UIKit.Card;
            if (!matchDoneRight[row]) refs.RightImages[row].color = UIKit.Card;
            matchSelected = -1;
            matchLocked = false;
        }

        void ToggleMs(QuizRefs refs, int idx)
        {
            if (msAnswered) return;
            msSelected[idx] = !msSelected[idx];
            refs.MsImages[idx].color = msSelected[idx] ? BlueSel : UIKit.Card;
        }

        void ConfirmMs(QuizRefs refs)
        {
            if (msAnswered) return;
            msAnswered = true;
            var qz = books[quizBook].Quiz[quizQ];
            bool allCorrect = true;
            for (int i = 0; i < qz.CorrectMask.Length; i++)
            {
                bool shouldSel = qz.CorrectMask[i];
                refs.MsImages[i].color = msSelected[i] == shouldSel ? GreenOk : RedBad;
                refs.MsButtons[i].interactable = false;
                if (msSelected[i] != shouldSel) allCorrect = false;
            }
            if (refs.MsConfirm != null) refs.MsConfirm.interactable = false;
            RegisterAnswer(allCorrect);
            ShowFeedback(refs, allCorrect);
        }

        void RegisterAnswer(bool correct)
        {
            if (!correct) return;
            quizScore++;
            var msgs = AppState.MarkQuizCorrect(quizBook, quizQ);
            if (msgs.Count > 0) ShowReward(string.Join("\n", msgs));
        }

        void ShowFeedback(QuizRefs refs, bool correct)
        {
            refs.FeedbackPanel.gameObject.SetActive(true);
            refs.FeedbackPanel.GetComponent<Image>().color = correct ? UIKit.Hex("E8F8F0") : UIKit.Hex("FFF4E6");
            var outline = refs.FeedbackPanel.GetComponent<Outline>();
            if (outline != null) outline.effectColor = correct ? UIKit.Hex("9BD7B8") : UIKit.Hex("F5C48B");
            refs.FeedbackTitle.color = correct ? UIKit.Hex("225D43") : UIKit.Hex("704516");
            refs.FeedbackText.color = correct ? UIKit.Hex("225D43") : UIKit.Hex("704516");
            refs.FeedbackTitle.text = correct ? "¡Lo conseguiste!" : "¡Buen intento!";
            refs.FeedbackText.text = correct ? "¡Muy bien! Sigue así y gana más estrellas." : "Mira las respuestas marcadas y sigue aprendiendo.";
            refs.Continue.GetComponentInChildren<TMP_Text>().text =
                quizQ == books[quizBook].Quiz.Length - 1 ? "VER RESULTADO ›" : "CONTINUAR ›";
            refs.FeedbackIcon.sprite = correct ? figStar : (figInfo != null ? figInfo : icInfo);
            refs.FeedbackIcon.color = correct ? UIKit.Prog : UIKit.Blue;
            var robot = refs.FeedbackPanel.Find("Robot");
            if (robot != null) robot.GetComponent<Image>().color = Color.white;
            var anim = refs.FeedbackPanel.GetComponent<Anim>();
            if (anim != null) anim.Reproducir();
            if (correct)
            {
                var star = refs.FeedbackPanel.Find("FeedbackIconBg/FeedbackIcon");
                if (star != null)
                {
                    var starAnim = star.GetComponent<Anim>();
                    if (starAnim == null) starAnim = Anim.Estrella(star);
                    starAnim.Reproducir();
                }
            }
        }

        void ContinueQuiz(QuizRefs refs)
        {
            quizQ++;
            if (quizQ >= books[quizBook].Quiz.Length) ShowQuizResult(refs);
            else ShowQuizQuestion(refs);
        }

        AudioClip QuizNarration()
        {
            var clips = books[quizBook].Narration;
            if (clips == null) return null;
            for (int i = 0; i < clips.Length; i++)
                if (clips[i] != null) return clips[i];
            return null;
        }

        void PlayQuizAudio(QuizRefs refs)
        {
            var clip = QuizNarration();
            if (clip == null || audioSrc == null) return;
            if (audioSrc.isPlaying) { audioSrc.Stop(); UpdateQuizAudio(refs, false); return; }
            audioSrc.clip = clip;
            audioSrc.Play();
            UpdateQuizAudio(refs, true);
            StartCoroutine(WatchQuizAudio(refs));
        }

        IEnumerator WatchQuizAudio(QuizRefs refs)
        {
            while (audioSrc != null && audioSrc.clip != null)
            {
                bool playing = audioSrc.isPlaying;
                if (!playing && audioSrc.time >= audioSrc.clip.length - .05f) break;
                UpdateQuizAudio(refs, true);
                yield return null;
            }
            if (refs != null && refs.AudioPanel != null) UpdateQuizAudio(refs, false);
        }

        void ToggleQuizAudio(QuizRefs refs)
        {
            if (audioSrc == null || audioSrc.clip == null) return;
            if (audioSrc.isPlaying) audioSrc.Pause();
            else audioSrc.UnPause();
            UpdateQuizAudio(refs, audioSrc.isPlaying || audioSrc.time > 0f);
        }

        void StopQuizAudio(QuizRefs refs)
        {
            if (audioSrc != null) { audioSrc.Stop(); audioSrc.clip = null; }
            UpdateQuizAudio(refs, false);
        }

        void UpdateQuizAudio(QuizRefs refs, bool visible)
        {
            if (refs == null || refs.AudioPanel == null) return;
            refs.AudioPanel.gameObject.SetActive(visible);
            bool playing = audioSrc != null && audioSrc.isPlaying;
            if (refs.AudioStatus != null) refs.AudioStatus.text = playing ? "Escuchando" : "Audio en pausa";
            if (refs.AudioToggleIcon != null) refs.AudioToggleIcon.sprite = playing ? figPause : figPlay;
            if (refs.AudioPulse != null)
            {
                refs.AudioPulse.color = new Color(UIKit.Blue.r, UIKit.Blue.g, UIKit.Blue.b, playing ? 1f : .45f);
                var pulse = refs.AudioPulse.GetComponent<Anim>();
                if (pulse != null) pulse.enabled = playing;
            }
        }

        void ShowQuizResult(QuizRefs refs)
        {
            refs.ResultPanel.gameObject.SetActive(true);
            refs.ResultText.text = "¡Terminaste!\n" + quizScore + "/" + books[quizBook].Quiz.Length + " correctas";
            SetProgress(refs.ResultFill, (float)quizScore / books[quizBook].Quiz.Length);
        }
    }
}
