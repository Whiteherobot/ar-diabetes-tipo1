using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    // Pantalla 6) JUEGOS Y RETOS: hub por libro + misión de 3 preguntas.
    public partial class AppBootstrap
    {
        // "Libro Fisiológico" -> "Misión Fisiológica" (concordancia como en el diseño)
        static string MisionNombre(string libro)
        {
            return "Misión " + libro.Replace("Libro ", "").Replace("Fisiológico", "Fisiológica").Replace("Clínico", "Clínica");
        }

        Sprite IconoMision(int book) => book == 0 ? figTemas[1] : book == 1 ? figTemas[6] : figKit;

        RectTransform BuildJuegos()
        {
            var p = Panel("6_Juegos");
            const float S = 2.77f;
            var fondo = UIKit.Img(p, null, "Fondo"); fondo.color = UIKit.Hex("FFF5FA"); UIKit.Stretch(fondo.rectTransform);
            float w = Land ? 1000 : 1080, cardW = w - 40f * S;
            var content = BuildAreaScroll(p, "Games", 82f * S, w);
            ((RectTransform)content.parent).offsetMax = Vector2.zero;

            float heroH = 310f * S;
            var hero = UIKit.Box(content, UIKit.Juegos, 2, "GamesHero");
            UIKit.Top(hero.rectTransform, 0, 0, w, heroH);
            hero.gameObject.AddComponent<Mask>();
            var c1 = UIKit.Img(hero.transform, UIKit.Circle(), "Circulo"); c1.color = new Color(1, 1, 1, .11f);
            UIKit.TopLeft(c1.rectTransform, w - 115f * S, -55f * S, 190f * S, 190f * S);
            var c2 = UIKit.Img(hero.transform, UIKit.Circle(), "Circulo"); c2.color = new Color(1, 1, 1, .11f);
            UIKit.TopLeft(c2.rectTransform, -55f * S, heroH - 70f * S, 135f * S, 135f * S);
            var robots = BuildRobotGroup(hero.transform);
            UIKit.TopLeft(robots, w - 179f * S, 204f * S, 196f * S, 132f * S);
            var k = UIKit.Text(hero.transform, "MISIONES DEL CONOCIMIENTO", 12f * S, Color.white, TextAlignmentOptions.Left);
            k.characterSpacing = 8f;
            UIKit.TopLeft(k.rectTransform, 23f * S, 42f * S, w - 110f * S, 18f * S);
            var t = UIKit.Text(hero.transform, "¡Vamos a jugar!", 31f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.TopLeft(t.rectTransform, 23f * S, 66f * S, w - 46f * S, 40f * S);
            var sb = UIKit.Text(hero.transform, "Completa misiones y descubre cuánto has aprendido.", 17f * S, UIKit.Hex("FFF2F7"), TextAlignmentOptions.Left, FontStyles.Normal);
            sb.fontWeight = FontWeight.Bold;
            UIKit.Interlinea(sb, 1.38f);
            UIKit.TopLeft(sb.rectTransform, 23f * S, 108f * S, 300f * S, 52f * S);
            var chispa1 = UIKit.Img(hero.transform, figStar, "Estrella"); chispa1.color = UIKit.Hex("FFF1A8"); chispa1.preserveAspect = true;
            UIKit.TopLeft(chispa1.rectTransform, w - 57f * S, 33f * S, 24f * S, 24f * S);
            chispa1.rectTransform.localRotation = Quaternion.Euler(0, 0, 10f);
            var chispa2 = UIKit.Img(hero.transform, figStar, "Estrella"); chispa2.color = Color.white; chispa2.preserveAspect = true;
            UIKit.TopLeft(chispa2.rectTransform, 26f * S, heroH - 48f * S, 16f * S, 16f * S);
            chispa2.rectTransform.localRotation = Quaternion.Euler(0, 0, -13f);

            float y = heroH + 27f * S;
            var missionTitle = UIKit.Text(content, "Elige tu misión", 24f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Top(missionTitle.rectTransform, 0, y, cardW, 34f * S);
            y += 49f * S;

            var count = new TMP_Text[3];
            var fills = new Image[3];
            string[] descs = { "Descubre cómo funciona tu cuerpo", "Explora el mundo de los alimentos", "Aprende a cuidar tu salud" };
            float cardH = 240f * S;
            for (int i = 0; i < 3; i++)
            {
                var b = books[i];
                int idx = i;
                bool inverso = i == 1;
                UnityEngine.Events.UnityAction abrir = () => { quizBook = idx; ShowOnly(bookQuiz[idx]); };
                var btn = UIKit.Button(content, "", b.Accent, Color.white, Mathf.RoundToInt(24f * S), abrir);
                UIKit.Top(R(btn), 0, y, cardW, cardH);
                UIKit.AddShadow(btn.GetComponent<Image>(), Mathf.RoundToInt(24f * S), .14f, -7, 7);
                btn.gameObject.AddComponent<Mask>();
                var deco = UIKit.Img(btn.transform, UIKit.Circle(), "Decor"); deco.color = new Color(1, 1, 1, .1f);
                UIKit.TopLeft(deco.rectTransform, cardW - 100f * S, cardH - 85f * S, 170f * S, 170f * S);
                var robot = UIKit.Img(btn.transform, RobotDeLibro(i), "Robot"); robot.preserveAspect = true;
                UIKit.TopLeft(robot.rectTransform, inverso ? -18f * S : cardW - 145f * S, 62f * S, 163f * S, 198f * S);
                UIKit.SombraImagen(robot, .18f, 7f * S);

                float x = (inverso ? 129f : 22f) * S;
                var small = UIKit.Text(btn.transform, "MISIÓN " + (i + 1), 12f * S, new Color(1, 1, 1, .86f), TextAlignmentOptions.Left);
                small.characterSpacing = 8f;
                UIKit.TopLeft(small.rectTransform, x, 25f * S, 180f * S, 18f * S);
                var tl = UIKit.Text(btn.transform, MisionNombre(b.Title), 22f * S, Color.white, TextAlignmentOptions.Left);
                tl.enableWordWrapping = false;
                UIKit.TopLeft(tl.rectTransform, x, 47f * S, 210f * S, 30f * S);
                var desc = UIKit.Text(btn.transform, descs[i], 15f * S, new Color(1, 1, 1, .94f), TextAlignmentOptions.Left, FontStyles.Normal);
                desc.fontWeight = FontWeight.SemiBold;
                UIKit.Interlinea(desc, 1.35f);
                UIKit.TopLeft(desc.rectTransform, x, 82f * S, 185f * S, 44f * S);
                var cnt = UIKit.Text(btn.transform, "Lista para comenzar", 13f * S, Color.white, TextAlignmentOptions.Left);
                UIKit.TopLeft(cnt.rectTransform, x, 132f * S, 180f * S, 18f * S);
                var track = UIKit.Box(btn.transform, new Color(1, 1, 1, .28f), Mathf.RoundToInt(4f * S), "Track");
                UIKit.TopLeft(track.rectTransform, x, 156f * S, 165f * S, 8f * S);
                var fill = UIKit.Box(track.transform, Color.white, Mathf.RoundToInt(4f * S), "Fill");
                UIKit.Frac(fill.rectTransform, 0f, 0f, 0f, 1f);
                var play = UIKit.Button(btn.transform, "", Color.white, b.Accent, Mathf.RoundToInt(13f * S), abrir, 10);
                UIKit.TopLeft(R(play), x, 176f * S, 96f * S, 42f * S);
                var playTxt = UIKit.Text(play.transform, "JUGAR", 14f * S, b.Accent, TextAlignmentOptions.Left);
                UIKit.TopLeft(playTxt.rectTransform, 14f * S, 11f * S, 56f * S, 20f * S);
                var playIco = UIKit.Img(play.transform, figPlay, "Play"); playIco.color = b.Accent; playIco.preserveAspect = true;
                UIKit.TopLeft(playIco.rectTransform, 66f * S, 12.5f * S, 17f * S, 17f * S);

                var reward = UIKit.Box(btn.transform, Color.white, Mathf.RoundToInt(18f * S), "Reward");
                UIKit.TopLeft(reward.rectTransform, inverso ? 17f * S : cardW - 76f * S, 15f * S, 59f * S, 37f * S);
                var rewardIco = UIKit.Img(reward.transform, figStar, "Star"); rewardIco.color = UIKit.Prog; rewardIco.preserveAspect = true;
                UIKit.TopLeft(rewardIco.rectTransform, 10f * S, 9.5f * S, 18f * S, 18f * S);
                var rewardTxt = UIKit.Text(reward.transform, "15", 14f * S, UIKit.Hex("5C4300"), TextAlignmentOptions.Left);
                UIKit.TopLeft(rewardTxt.rectTransform, 32f * S, 8f * S, 24f * S, 21f * S);

                count[i] = cnt; fills[i] = fill;
                btn.gameObject.AddComponent<Appear>().delay = .05f * i;
                y += cardH + 19f * S;
            }
            content.sizeDelta = new Vector2(w, y + 9f * S);
            p.gameObject.AddComponent<JuegosRefs>().Set(count, fills);
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
                float avance = (float)AppState.QuizCorrectCount(i) / books[i].Quiz.Length;
                if (refs.Count[i] != null) refs.Count[i].text = avance > 0f ? Mathf.RoundToInt(avance * 100f) + "% completado" : "Lista para comenzar";
                if (refs.Fill != null && refs.Fill[i] != null) SetProgress(refs.Fill[i], avance);
            }
        }

        static readonly Color VerdeBorde = UIKit.Hex("66C795"), VerdeFondo = UIKit.Hex("DFF7E9");
        static readonly Color RojoBorde = UIKit.Hex("EE7B7B"), RojoFondo = UIKit.Hex("FDE5E5");

        RectTransform BuildQuiz(int book)
        {
            const float S = 2.77f;
            var b = books[book];
            var p = Panel(book + "_Quiz");
            var fondo = UIKit.Img(p, null, "Fondo"); fondo.color = UIKit.Hex("F4F9FD"); UIKit.Stretch(fondo.rectTransform);
            BuildHeaderBar(p, MisionNombre(b.Title), b.Accent, IconoMision(book), () => ShowOnly(pJuegos), RobotDeLibro(book));
            float w = Land ? 1000 : 1080, cardW = w - 40f * S;
            var content = BuildAreaScroll(p, "Quiz", 0f, w);
            var refs = p.gameObject.AddComponent<QuizRefs>();
            refs.Content = content;

            var pregunta = UIKit.Node("Pregunta", content);
            UIKit.Top(pregunta, 0, 0, w, 10f);
            refs.Pregunta = pregunta;

            refs.Progress = UIKit.Text(pregunta, "", 17f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Top(refs.Progress.rectTransform, 0, 20f * S, cardW, 25f * S);
            refs.ProgressType = UIKit.Text(pregunta, "", 14f * S, UIKit.Hex("637B93"), TextAlignmentOptions.Right);
            UIKit.Top(refs.ProgressType.rectTransform, 0, 20f * S, cardW, 25f * S);
            var track = UIKit.Box(pregunta, UIKit.Hex("DFE9EF"), Mathf.RoundToInt(6f * S), "Track");
            UIKit.Top(track.rectTransform, 0, 53f * S, cardW, 12f * S);
            refs.ProgressFill = UIKit.Box(track.transform, b.Accent, Mathf.RoundToInt(6f * S), "Fill");
            UIKit.Frac(refs.ProgressFill.rectTransform, 0f, 0f, 0f, 1f);

            var coachRobot = UIKit.Img(pregunta, RobotDeLibro(book), "Coach"); coachRobot.preserveAspect = true;
            refs.Coach = coachRobot.rectTransform;
            var burbuja = UIKit.Box(pregunta, Color.white, Mathf.RoundToInt(15f * S), "Burbuja");
            refs.CoachBurbuja = burbuja.rectTransform;
            refs.BurbujaSombra = UIKit.AddShadow(burbuja, Mathf.RoundToInt(15f * S), .08f, -3, 3).rectTransform;
            refs.CoachTxt = UIKit.Text(burbuja.transform, "", 15f * S, UIKit.Hex("4B637D"), TextAlignmentOptions.Left);
            refs.CoachTxt.fontWeight = FontWeight.Bold;
            UIKit.TamanoFijo(refs.CoachTxt, 15f * S);
            var coachAudio = BotonVoz(pregunta, 44f, 14f, UIKit.Blue, () => { var t = TextoCoach(b.Quiz[quizQ]); ReproducirVoz(refs, t); });
            refs.CoachAudio = R(coachAudio);

            var tarjeta = UIKit.Box(pregunta, Color.white, Mathf.RoundToInt(24f * S), "QCard");
            refs.Tarjeta = tarjeta.rectTransform;
            refs.TarjetaSombra = UIKit.AddShadow(tarjeta, Mathf.RoundToInt(24f * S), .10f, -6, 6).rectTransform;
            refs.Tipo = UIKit.Text(tarjeta.transform, "", 12f * S, UIKit.Blue, TextAlignmentOptions.Left);
            refs.Tipo.characterSpacing = 6f;
            refs.Question = UIKit.Text(tarjeta.transform, "", 21f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.TamanoFijo(refs.Question, 21f * S);
            UIKit.Interlinea(refs.Question, 1.42f);
            var qAudio = BotonVoz(tarjeta.transform, 48f, 15f, UIKit.Blue, () => ReproducirVoz(refs, b.Quiz[quizQ].Q));
            qAudio.GetComponent<Image>().color = UIKit.Hex("EAF5FF");
            var qart = R(qAudio);
            qart.anchorMin = qart.anchorMax = new Vector2(1, 1); qart.pivot = new Vector2(1, 1);
            qart.anchoredPosition = new Vector2(-17f * S, -17f * S);

            // Reproductor de voz (oculto hasta que se escucha algo)
            var repro = UIKit.Box(pregunta, UIKit.Hex("DFEFFF"), Mathf.RoundToInt(17f * S), "QuizAudioControls");
            refs.Reproductor = repro.rectTransform;
            var rIco = UIKit.Img(repro.transform, figSpeaker, "Speaker"); rIco.color = UIKit.Hex("245D93"); rIco.preserveAspect = true;
            UIKit.TopLeft(rIco.rectTransform, 13f * S, 16f * S, 26f * S, 26f * S);
            refs.AudioStatus = UIKit.Text(repro.transform, "", 14f * S, UIKit.Hex("24496D"), TextAlignmentOptions.Left);
            UIKit.TopLeft(refs.AudioStatus.rectTransform, 47f * S, 18f * S, 130f * S, 22f * S);
            refs.AudioPulse = UIKit.Box(repro.transform, UIKit.Blue, 4, "Pulse");
            UIKit.TopLeft(refs.AudioPulse.rectTransform, 180f * S, 27f * S, cardW - 293f * S, 4f * S);
            var pulso = Anim.Pulso(refs.AudioPulse); pulso.alterna = true;
            var rPausa = UIKit.Button(repro.transform, "", Color.white, UIKit.Navy, Mathf.RoundToInt(13f * S), () => PausarVoz(refs), 10);
            UIKit.TopLeft(R(rPausa), cardW - 105f * S, 7f * S, 44f * S, 44f * S);
            refs.AudioToggleIcon = UIKit.Img(rPausa.transform, figPause, "Toggle"); refs.AudioToggleIcon.color = UIKit.Hex("245D93"); refs.AudioToggleIcon.preserveAspect = true;
            UIKit.Stretch(refs.AudioToggleIcon.rectTransform, 11.5f * S);
            var rStop = UIKit.Button(repro.transform, "", Color.white, UIKit.Navy, Mathf.RoundToInt(13f * S), () => DetenerVoz(refs), 10);
            UIKit.TopLeft(R(rStop), cardW - 53f * S, 7f * S, 44f * S, 44f * S);
            var rStopIco = UIKit.Img(rStop.transform, figStop, "Stop"); rStopIco.color = UIKit.Hex("245D93"); rStopIco.preserveAspect = true;
            UIKit.Stretch(rStopIco.rectTransform, 12.5f * S);

            // Cuatro opciones; la pantalla decide si van en lista o en parejas
            refs.Opciones = new OpcionQuiz[4];
            for (int i = 0; i < 4; i++)
            {
                int fila = i;
                var o = new OpcionQuiz();
                o.Btn = UIKit.Button(pregunta, "", Color.white, UIKit.Navy, Mathf.RoundToInt(18f * S), () => ResponderQuiz(refs, fila), 10);
                o.Borde = o.Btn.GetComponent<Image>();
                o.Sombra = UIKit.AddShadow(o.Borde, Mathf.RoundToInt(18f * S), .09f, -3, 3).rectTransform;
                o.Interior = UIKit.Box(o.Btn.transform, Color.white, Mathf.RoundToInt(16f * S), "Interior");
                UIKit.Stretch(o.Interior.rectTransform, 2f * S);
                o.Texto = UIKit.Text(o.Btn.transform, "", 17f * S, UIKit.Navy, TextAlignmentOptions.Left);
                o.Texto.fontWeight = FontWeight.SemiBold;
                UIKit.TamanoFijo(o.Texto, 17f * S);
                UIKit.Interlinea(o.Texto, 1.4f);
                o.ParA = UIKit.Text(o.Btn.transform, "", 17f * S, UIKit.Navy);
                var linea = UIKit.Box(o.Btn.transform, UIKit.Hex("D5E0E9"), 4, "Linea");
                o.ParLinea = linea.rectTransform;
                o.ParB = UIKit.Text(o.Btn.transform, "", 16f * S, UIKit.Hex("607790"), TextAlignmentOptions.Center, FontStyles.Normal);
                o.Pill = UIKit.Box(o.Btn.transform, UIKit.Hex("CCEFDC"), Mathf.RoundToInt(8f * S), "Estado");
                o.PillIco = UIKit.Img(o.Pill.transform, figCheck, "Ico"); o.PillIco.preserveAspect = true;
                UIKit.TopLeft(o.PillIco.rectTransform, 7f * S, 6f * S, 16f * S, 16f * S);
                o.PillTxt = UIKit.Text(o.Pill.transform, "", 15f * S, Color.white, TextAlignmentOptions.Left);
                UIKit.TopLeft(o.PillTxt.rectTransform, 25f * S, 3f * S, 86f * S, 22f * S);
                var aud = BotonVoz(pregunta, 48f, 15f, UIKit.Hex("59738D"), () =>
                {
                    var q = b.Quiz[quizQ];
                    ReproducirVoz(refs, q.Options[refs.Orden[fila]]);
                });
                o.Audio = aud;
                o.AudioSombra = UIKit.AddShadow(aud.GetComponent<Image>(), Mathf.RoundToInt(15f * S), .09f, -3, 3).rectTransform;
                refs.Opciones[i] = o;
            }

            // Retroalimentación
            var fb = UIKit.Box(pregunta, UIKit.Hex("9BD7B8"), Mathf.RoundToInt(22f * S), "Feedback");
            refs.FeedbackBorde = fb; refs.Feedback = fb.rectTransform;
            var fbInt = UIKit.Box(fb.transform, UIKit.Hex("E8F8F0"), Mathf.RoundToInt(20f * S), "Interior");
            UIKit.Stretch(fbInt.rectTransform, 2f * S);
            refs.FeedbackInterior = fbInt;
            var fbCaja = UIKit.Box(fb.transform, Color.white, Mathf.RoundToInt(13f * S), "FeedbackIconBg");
            UIKit.TopLeft(fbCaja.rectTransform, 18f * S, 30f * S, 42f * S, 42f * S);
            refs.FeedbackIcon = UIKit.Img(fbCaja.transform, figStar, "FeedbackIcon"); refs.FeedbackIcon.preserveAspect = true;
            UIKit.Stretch(refs.FeedbackIcon.rectTransform, 9f * S);
            Anim.Estrella(refs.FeedbackIcon);
            refs.FeedbackTitle = UIKit.Text(fb.transform, "", 19f * S, Color.white, TextAlignmentOptions.Left);
            UIKit.TopLeft(refs.FeedbackTitle.rectTransform, 70f * S, 18f * S, cardW - 145f * S, 27f * S);
            refs.FeedbackText = UIKit.Text(fb.transform, "", 16f * S, Color.white, TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.TamanoFijo(refs.FeedbackText, 16f * S);
            UIKit.Interlinea(refs.FeedbackText, 1.4f);
            var fbRobot = UIKit.Img(fb.transform, RobotDeLibro(book), "Robot"); fbRobot.preserveAspect = true;
            UIKit.TopLeft(fbRobot.rectTransform, cardW - 73f * S, 18f * S, 55f * S, 66f * S);
            var acciones = UIKit.Node("Acciones", fb.transform);
            refs.FeedbackAcciones = acciones;
            var fbAudio = BotonVoz(acciones, 48f, 14f, UIKit.Blue, () => ReproducirVoz(refs, TextoFeedback(refs)));
            UIKit.TopLeft(R(fbAudio), 0, 1f * S, 48f * S, 48f * S);
            var seguir = UIKit.Button(acciones, "", UIKit.Blue, Color.white, Mathf.RoundToInt(15f * S), () => ContinueQuiz(refs), 10);
            UIKit.TopLeft(R(seguir), 58f * S, 0, cardW - 94f * S, 50f * S);
            refs.ContinuarTxt = UIKit.Text(seguir.transform, "", 15f * S, Color.white, TextAlignmentOptions.Center);
            UIKit.Frac(refs.ContinuarTxt, .05f, .1f, .85f, .9f);
            var segIco = UIKit.Img(seguir.transform, figChevron, "Chevron"); segIco.color = Color.white; segIco.preserveAspect = true;
            UIKit.Frac(segIco, .82f, .29f, .92f, .71f);

            // Resultado final
            var resultado = UIKit.Node("Resultado", content);
            refs.Resultado = resultado;
            float altoR = 650f * S;
            UIKit.Top(resultado, 0, 22f * S, cardW, altoR);
            var rCard = UIKit.Box(resultado, Color.white, Mathf.RoundToInt(24f * S), "ResultCard");
            UIKit.Stretch(rCard.rectTransform);
            float rw = cardW - 50f * S, ry = 28f * S;
            var rRobot = UIKit.Img(rCard.transform, RobotDeLibro(book), "Robot"); rRobot.preserveAspect = true;
            UIKit.TopLeft(rRobot.rectTransform, (cardW - 118f * S) / 2f, ry, 118f * S, 143f * S);
            Anim.Celebrar(rRobot);
            ry += 143f * S - 25f * S;
            var rEstrella = UIKit.Img(rCard.transform, UIKit.Circle(), "Star"); rEstrella.color = UIKit.Hex("FFF6D6");
            UIKit.TopLeft(rEstrella.rectTransform, (cardW - 86f * S) / 2f, ry, 86f * S, 86f * S);
            var rEstrellaIco = UIKit.Img(rEstrella.transform, figStar, "Icono"); rEstrellaIco.color = UIKit.Prog; rEstrellaIco.preserveAspect = true;
            UIKit.Stretch(rEstrellaIco.rectTransform, 14f * S);
            Anim.Estrella(rEstrellaIco);
            ry += 86f * S + 16f * S;
            var rKick = UIKit.Text(rCard.transform, "MISIÓN SUPERADA", 12f * S, UIKit.Hex("937116"));
            rKick.characterSpacing = 8f;
            UIKit.TopLeft(rKick.rectTransform, 25f * S, ry, rw, 18f * S); ry += 25f * S;
            var rTit = UIKit.Text(rCard.transform, "¡Misión completada!", 29f * S, UIKit.Navy);
            UIKit.TopLeft(rTit.rectTransform, 25f * S, ry, rw, 40f * S); ry += 45f * S;
            refs.ResultTexto = UIKit.Text(rCard.transform, "", 16f * S, UIKit.Hex("607690"), TextAlignmentOptions.Center, FontStyles.Normal);
            UIKit.TopLeft(refs.ResultTexto.rectTransform, 25f * S, ry, rw, 24f * S); ry += 38f * S;
            refs.ResultEstrellas = UIKit.Text(rCard.transform, "", 21f * S, UIKit.Hex("E3A627"));
            UIKit.TopLeft(refs.ResultEstrellas.rectTransform, 25f * S, ry, rw, 30f * S); ry += 45f * S;
            var rTrack = UIKit.Box(rCard.transform, UIKit.Hex("E7EDF2"), Mathf.RoundToInt(6f * S), "ResultTrack");
            UIKit.TopLeft(rTrack.rectTransform, 25f * S, ry, rw, 12f * S); ry += 28f * S;
            refs.ResultFill = UIKit.Box(rTrack.transform, b.Accent, Mathf.RoundToInt(6f * S), "ResultFill");
            UIKit.Frac(refs.ResultFill.rectTransform, 0f, 0f, 0f, 1f);
            var escuchar = UIKit.Button(rCard.transform, "", UIKit.Hex("EDF5FB"), UIKit.Navy, Mathf.RoundToInt(14f * S),
                () => ReproducirVozSimple("¡Misión completada! Respondiste correctamente " + quizScore + " de " + b.Quiz.Length + "."), 10);
            UIKit.TopLeft(R(escuchar), (cardW - 192f * S) / 2f, ry, 192f * S, 46f * S);
            var escTxt = UIKit.Text(escuchar.transform, "Escuchar resultado", 14f * S, UIKit.Hex("3C5E7D"), TextAlignmentOptions.Left);
            UIKit.TopLeft(escTxt.rectTransform, 44f * S, 12f * S, 140f * S, 22f * S);
            var escIco = UIKit.Img(escuchar.transform, figSpeaker, "Icono"); escIco.color = UIKit.Hex("3C5E7D"); escIco.preserveAspect = true;
            UIKit.TopLeft(escIco.rectTransform, 15f * S, 12.5f * S, 21f * S, 21f * S);
            ry += 59f * S;
            var repetir = UIKit.Button(rCard.transform, "VOLVER A JUGAR", b.Accent, Color.white, Mathf.RoundToInt(17f * S), () => ShowOnly(p, false), Mathf.RoundToInt(17f * S));
            UIKit.TopLeft(R(repetir), 25f * S, ry, rw, 60f * S); ry += 70f * S;
            var regresarBorde = UIKit.Button(rCard.transform, "", UIKit.Hex("DCE5EC"), UIKit.Navy, Mathf.RoundToInt(17f * S), () => ShowOnly(pJuegos), 10);
            UIKit.TopLeft(R(regresarBorde), 25f * S, ry, rw, 55f * S);
            var regInt = UIKit.Box(regresarBorde.transform, Color.white, Mathf.RoundToInt(15f * S), "Interior");
            UIKit.Stretch(regInt.rectTransform, 2f * S);
            var regTxt = UIKit.Text(regInt.transform, "REGRESAR A MISIONES", 16f * S, UIKit.Navy);
            UIKit.Stretch(regTxt.rectTransform);
            ry += 55f * S + 30f * S;
            UIKit.Top(resultado, 0, 22f * S, cardW, ry);
            UIKit.AddShadow(rCard, Mathf.RoundToInt(24f * S), .10f, -6, 6);
            resultado.gameObject.SetActive(false);
            return p;
        }

        Button BotonVoz(Transform parent, float tam, float radio, Color tinta, UnityEngine.Events.UnityAction click)
        {
            const float S = 2.77f;
            var boton = UIKit.Button(parent, "", Color.white, tinta, Mathf.RoundToInt(radio * S), click, 10);
            var rt = R(boton); rt.sizeDelta = new Vector2(tam * S, tam * S);
            var ico = UIKit.Img(boton.transform, figSpeaker, "Icono"); ico.color = tinta; ico.preserveAspect = true;
            var irt = ico.rectTransform;
            irt.anchorMin = irt.anchorMax = new Vector2(.5f, .5f);
            irt.sizeDelta = new Vector2(21f * S, 21f * S); irt.anchoredPosition = Vector2.zero;
            return boton;
        }

        static string TextoCoach(GameItem q) =>
            q.EsRelacion ? "¡Busca la pareja que sí corresponde!" :
            q.EsFrase ? "¡Elige la palabra que completa la idea!" : "¡Tú puedes, lee con calma!";

        static string TextoTipo(GameItem q) =>
            q.EsRelacion ? "Relaciona los elementos" : q.EsFrase ? "Completa la frase" : "Selecciona la respuesta";

        string TextoFeedback(QuizRefs r)
        {
            var q = books[quizBook].Quiz[quizQ];
            if (r.Elegida >= 0 && r.Orden[r.Elegida] == q.Correct) return "¡Excelente trabajo! Encontraste la respuesta correcta.";
            return "¡Buen intento! Vamos a aprender juntos. La respuesta correcta es " + q.Options[q.Correct] + ".";
        }

        // Frase con su espacio: vacío al inicio y con la palabra elegida después de responder.
        string TextoPregunta(QuizRefs r, GameItem q)
        {
            if (!q.EsFrase) return q.Q;
            const string nb = " ";
            string hueco;
            if (r.Elegida < 0) hueco = "<color=#8499AD>________</color>";
            else
            {
                bool bien = r.Orden[r.Elegida] == q.Correct;
                string palabra = q.Options[r.Orden[r.Elegida]].Replace(" ", nb);
                hueco = bien
                    ? "<mark=#E3F7EDFF><u color=#2FAE75><color=#237D55>" + nb + palabra + nb + "</color></u></mark>"
                    : "<mark=#FDECEEFF><u color=#E94F58><color=#B73540>" + nb + palabra + nb + "</color></u></mark>";
            }
            return q.Q.Replace("______", hueco);
        }

        void RefreshQuiz()
        {
            if (bookQuiz == null || quizBook < 0 || quizBook >= bookQuiz.Length || bookQuiz[quizBook] == null) return;
            quizQ = 0; quizScore = 0;
            quizEstrellasInicio = AppState.Stars;
            var refs = bookQuiz[quizBook].GetComponent<QuizRefs>();
            if (refs == null) return;
            refs.Resultado.gameObject.SetActive(false);
            refs.Pregunta.gameObject.SetActive(true);
            DetenerVoz(refs);
            ShowQuizQuestion(refs);
        }

        void ShowQuizQuestion(QuizRefs r)
        {
            var book = books[quizBook];
            var q = book.Quiz[quizQ];
            r.Elegida = -1;
            // Se mezclan las opciones para que la correcta no quede siempre primero
            r.Orden = new[] { 0, 1, 2, 3 };
            for (int i = 3; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                int t = r.Orden[i]; r.Orden[i] = r.Orden[j]; r.Orden[j] = t;
            }
            r.Progress.text = "Pregunta " + (quizQ + 1) + " de " + book.Quiz.Length;
            r.ProgressType.text = TextoTipo(q);
            SetProgress(r.ProgressFill, (quizQ + 1f) / book.Quiz.Length);
            r.CoachTxt.text = TextoCoach(q);
            r.Tipo.text = TextoTipo(q).ToUpper();
            for (int i = 0; i < 4; i++) r.Opciones[i].Btn.interactable = true;
            AcomodarQuiz(r);
            r.Content.anchoredPosition = Vector2.zero;
        }

        void ResponderQuiz(QuizRefs r, int fila)
        {
            if (r.Elegida >= 0) return;
            r.Elegida = fila;
            var q = books[quizBook].Quiz[quizQ];
            bool bien = r.Orden[fila] == q.Correct;
            if (bien)
            {
                quizScore++;
                var msgs = AppState.MarkQuizCorrect(quizBook, quizQ);
                // La primera línea (+5 estrellas) ya la cuenta la tarjeta; solo se avisa lo extra.
                if (msgs.Count > 1) ShowReward(string.Join("\n", msgs.GetRange(1, msgs.Count - 1)));
            }
            for (int i = 0; i < 4; i++) r.Opciones[i].Btn.interactable = false;
            AcomodarQuiz(r);
            StartCoroutine(BajarAlFinal(r));
        }

        IEnumerator BajarAlFinal(QuizRefs r)
        {
            yield return null;
            var sr = r.Content.parent.GetComponent<ScrollRect>();
            if (sr != null) { sr.velocity = Vector2.zero; sr.verticalNormalizedPosition = 0f; }
        }

        // Todo va en flujo, como en el diseño: cada bloque se coloca debajo del anterior.
        void AcomodarQuiz(QuizRefs r)
        {
            const float S = 2.77f;
            var q = books[quizBook].Quiz[quizQ];
            float w = r.Content.sizeDelta.x, cardW = w - 40f * S, izq = -cardW / 2f;
            float y = 85f * S;

            // Entrenador
            float burbujaW = cardW - 117f * S, textoW = burbujaW - 26f * S;
            float altoCoachTxt = UIKit.Alto(r.CoachTxt, r.CoachTxt.text, textoW);
            float altoBurbuja = altoCoachTxt + 18f * S, altoFila = Mathf.Max(65f * S, altoBurbuja);
            UIKit.Top(r.Coach, izq + 27f * S, y + (altoFila - 65f * S) / 2f, 54f * S, 65f * S);
            UIKit.Top(r.CoachBurbuja, izq + 63f * S + burbujaW / 2f, y + (altoFila - altoBurbuja) / 2f, burbujaW, altoBurbuja);
            UIKit.AjustarSombra(r.BurbujaSombra, r.CoachBurbuja, -3);
            UIKit.TopLeft(r.CoachTxt.rectTransform, 13f * S, 9f * S, textoW, altoCoachTxt + 4f);
            r.CoachAudio.anchorMin = r.CoachAudio.anchorMax = new Vector2(.5f, 1); r.CoachAudio.pivot = new Vector2(.5f, 1);
            r.CoachAudio.anchoredPosition = new Vector2(-izq - 22f * S, -(y + (altoFila - 44f * S) / 2f));
            y += altoFila + 14f * S;

            // Pregunta
            float preguntaW = cardW - 90f * S;
            r.Question.text = TextoPregunta(r, q);
            float altoQ = UIKit.Alto(r.Question, r.Question.text, preguntaW);
            float altoT = Mathf.Max(195f * S, 32f * S + 18f * S + 10f * S + altoQ + 28f * S);
            float yBloque = (altoT - (18f * S + 10f * S + altoQ)) / 2f;
            UIKit.Top(r.Tarjeta, 0, y, cardW, altoT);
            UIKit.AjustarSombra(r.TarjetaSombra, r.Tarjeta, -6);
            UIKit.TopLeft(r.Tipo.rectTransform, 25f * S, yBloque, preguntaW, 18f * S);
            UIKit.TopLeft(r.Question.rectTransform, 25f * S, yBloque + 28f * S, preguntaW, altoQ + 4f);
            y += altoT + 20f * S;

            // Reproductor
            bool hayAudio = r.Sonando;
            r.Reproductor.gameObject.SetActive(hayAudio);
            if (hayAudio)
            {
                UIKit.Top(r.Reproductor, 0, y - 5f * S, cardW, 58f * S);
                y += 68f * S;
            }

            // Opciones
            bool parejas = q.EsRelacion;
            float colW = (cardW - 13f * S) / 2f, optW = cardW - 55f * S;
            for (int i = 0; i < 4; i++)
            {
                var o = r.Opciones[i];
                int real = r.Orden[i];
                string texto = q.Options[real];
                bool respondida = r.Elegida >= 0;
                bool bien = respondida && real == q.Correct;
                bool mal = respondida && !bien && r.Elegida == i;
                o.Borde.color = bien ? VerdeBorde : mal ? RojoBorde : Color.white;
                o.Interior.color = bien ? VerdeFondo : mal ? RojoFondo : Color.white;
                o.Pill.gameObject.SetActive(bien || mal);
                o.Pill.color = bien ? UIKit.Hex("CCEFDC") : UIKit.Hex("FBD3D7");
                o.PillTxt.color = o.PillIco.color = bien ? UIKit.Hex("206F4B") : UIKit.Hex("A72C36");
                o.PillIco.sprite = bien ? figCheck : figX;
                o.PillTxt.text = bien ? "Correcta" : "Tu elección";
                float pillW = (bien ? 92f : 113f) * S;

                o.Texto.gameObject.SetActive(!parejas);
                o.ParA.gameObject.SetActive(parejas);
                o.ParB.gameObject.SetActive(parejas);
                o.ParLinea.gameObject.SetActive(parejas);
                float h, x, boxW;
                if (parejas)
                {
                    var par = texto.Split('—');
                    o.ParA.text = par[0].Trim();
                    o.ParB.text = par.Length > 1 ? par[1].Trim() : "";
                    o.ParA.color = bien ? UIKit.Hex("237D55") : mal ? UIKit.Hex("B73540") : UIKit.Navy;
                    o.ParB.color = bien ? UIKit.Hex("237D55") : mal ? UIKit.Hex("B73540") : UIKit.Hex("607790");
                    h = 130f * S; boxW = colW;
                    x = (i % 2 == 0 ? -1f : 1f) * (colW + 13f * S) / 2f;
                    float yFila = y + (i / 2) * 192f * S;
                    UIKit.Top(R(o.Btn), x, yFila, boxW, h);
                    UIKit.TopLeft(o.ParA.rectTransform, 8f * S, 27f * S, colW - 16f * S, 26f * S);
                    UIKit.TopLeft(o.ParLinea, (colW - 35f * S) / 2f, 60f * S, 35f * S, 3f * S);
                    UIKit.TopLeft(o.ParB.rectTransform, 8f * S, 70f * S, colW - 16f * S, 26f * S);
                    UIKit.TopLeft(o.Pill.rectTransform, (colW - pillW) / 2f, h - 16f * S, pillW, 28f * S);
                    UIKit.Top(R(o.Audio), x, yFila + h + 7f * S, 46f * S, 42f * S);
                }
                else
                {
                    float interior = optW - 34f * S;
                    o.Texto.text = texto;
                    float anchoLinea = o.Texto.GetPreferredValues(texto).x + 4f;
                    bool cabeJunto = !(bien || mal) || anchoLinea + 8f * S + pillW <= interior;
                    float anchoTexto = (bien || mal) && !cabeJunto ? interior - pillW - 8f * S : interior;
                    float altoTexto = UIKit.Alto(o.Texto, texto, anchoTexto);
                    h = Mathf.Max(70f * S, altoTexto + 34f * S);
                    boxW = optW;
                    x = -(cardW - optW) / 2f;
                    UIKit.Top(R(o.Btn), x, y, boxW, h);
                    UIKit.TopLeft(o.Texto.rectTransform, 17f * S, (h - altoTexto) / 2f, anchoTexto, altoTexto + 4f);
                    float xPill = cabeJunto ? 17f * S + anchoLinea + 8f * S : optW - 17f * S - pillW;
                    UIKit.TopLeft(o.Pill.rectTransform, xPill, (h - 28f * S) / 2f, pillW, 28f * S);
                    UIKit.Top(R(o.Audio), (cardW - 48f * S) / 2f, y, 48f * S, h);
                    y += h + 13f * S;
                }
                UIKit.AjustarSombra(o.Sombra, R(o.Btn), -3);
                UIKit.AjustarSombra(o.AudioSombra, R(o.Audio), -3);
            }
            if (parejas) y += 2 * 192f * S;
            y += 7f * S;

            // Retroalimentación
            bool hayFeedback = r.Elegida >= 0;
            r.Feedback.gameObject.SetActive(hayFeedback);
            if (hayFeedback)
            {
                bool bien = r.Orden[r.Elegida] == q.Correct;
                Color tinta = bien ? UIKit.Hex("225D43") : UIKit.Hex("704516");
                r.FeedbackBorde.color = bien ? UIKit.Hex("9BD7B8") : UIKit.Hex("F5C48B");
                r.FeedbackInterior.color = bien ? UIKit.Hex("E8F8F0") : UIKit.Hex("FFF4E6");
                r.FeedbackTitle.text = bien ? "¡Lo conseguiste!" : "¡Buen intento!";
                r.FeedbackTitle.color = tinta; r.FeedbackText.color = tinta;
                r.FeedbackIcon.sprite = bien ? figStar : figInfo;
                r.FeedbackIcon.color = bien ? UIKit.Prog : UIKit.Scan;
                r.FeedbackText.text = TextoFeedback(r);
                float textoF = cardW - 145f * S;
                float altoP = UIKit.Alto(r.FeedbackText, r.FeedbackText.text, textoF);
                UIKit.TopLeft(r.FeedbackText.rectTransform, 70f * S, 48f * S, textoF, altoP + 4f);
                float cabecera = Mathf.Max(66f * S, 30f * S + altoP);
                float altoF = 18f * S + cabecera + 15f * S + 50f * S + 18f * S;
                UIKit.Top(r.Feedback, 0, y, cardW, altoF);
                UIKit.TopLeft(r.FeedbackAcciones, 18f * S, 18f * S + cabecera + 15f * S, cardW - 36f * S, 50f * S);
                r.ContinuarTxt.text = quizQ == books[quizBook].Quiz.Length - 1 ? "VER RESULTADO" : "CONTINUAR";
                y += altoF + 25f * S;
            }
            r.Pregunta.sizeDelta = new Vector2(w, y);
            r.Content.sizeDelta = new Vector2(w, y + 10f * S);
        }

        void ContinueQuiz(QuizRefs r)
        {
            DetenerVoz(r);
            quizQ++;
            if (quizQ >= books[quizBook].Quiz.Length) ShowQuizResult(r);
            else ShowQuizQuestion(r);
        }

        void ShowQuizResult(QuizRefs r)
        {
            const float S = 2.77f;
            int total = books[quizBook].Quiz.Length;
            r.Pregunta.gameObject.SetActive(false);
            r.Resultado.gameObject.SetActive(true);
            r.ResultTexto.text = "Respondiste correctamente " + quizScore + " de " + total;
            int ganadas = Mathf.Max(0, AppState.Stars - quizEstrellasInicio);
            r.ResultEstrellas.text = "+" + ganadas + " estrellas";
            SetProgress(r.ResultFill, (float)quizScore / total);
            r.Content.sizeDelta = new Vector2(r.Content.sizeDelta.x, r.Resultado.sizeDelta.y + 44f * S);
            r.Content.anchoredPosition = Vector2.zero;
        }

        // ---- Voces ----
        AudioClip VozDe(string texto)
        {
            if (vocesPorNombre == null)
            {
                vocesPorNombre = new Dictionary<string, AudioClip>();
                if (vocesQuiz != null) foreach (var c in vocesQuiz) if (c != null) vocesPorNombre[c.name] = c;
            }
            var hablado = texto.Replace("______", "espacio en blanco");
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(hablado));
                var nombre = System.BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant().Substring(0, 12);
                return vocesPorNombre.TryGetValue(nombre, out var clip) ? clip : null;
            }
        }

        bool ReproducirVozSimple(string texto)
        {
            if (AppState.Muted) { Toast("Audio silenciado (actívalo en Configuración)"); return false; }
            var clip = VozDe(texto);
            if (clip == null || audioSrc == null) { Toast("Sin audio disponible"); return false; }
            audioSrc.Stop();
            audioSrc.clip = clip;
            audioSrc.Play();
            return true;
        }

        void ReproducirVoz(QuizRefs r, string texto)
        {
            if (!ReproducirVozSimple(texto)) return;
            r.Sonando = true; r.Pausado = false;
            PintarReproductor(r);
            AcomodarQuiz(r);
            StartCoroutine(EsperarFinAudio(() => r.Pausado, () => { r.Sonando = false; AcomodarSiPregunta(r); }));
        }

        void AcomodarSiPregunta(QuizRefs r)
        {
            if (r != null && r.Pregunta.gameObject.activeInHierarchy) AcomodarQuiz(r);
        }

        void PausarVoz(QuizRefs r)
        {
            r.Pausado = !r.Pausado;
            if (r.Pausado) audioSrc.Pause(); else audioSrc.UnPause();
            PintarReproductor(r);
        }

        void DetenerVoz(QuizRefs r)
        {
            if (audioSrc != null) audioSrc.Stop();
            r.Sonando = false; r.Pausado = false;
            r.Reproductor.gameObject.SetActive(false);
        }

        void PintarReproductor(QuizRefs r)
        {
            r.AudioStatus.text = r.Pausado ? "Audio en pausa" : "Escuchando";
            r.AudioToggleIcon.sprite = r.Pausado ? figPlay : figPause;
            r.AudioPulse.color = new Color(UIKit.Blue.r, UIKit.Blue.g, UIKit.Blue.b, r.Pausado ? .45f : 1f);
        }
    }
}
