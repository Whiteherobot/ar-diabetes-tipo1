using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    // Pantallas 7) PROGRESO 7B) INSIGNIAS 7C) COLECCIÓN AR.
    public partial class AppBootstrap
    {
        // ============================================================
        // 7 - PROGRESO
        // ============================================================
        RectTransform BuildProgreso()
        {
            var p = Panel("7_Progreso");
            const float S = 2.77f;
            bool land = Land;
            var header = BuildHeaderBar(p, "Tu progreso", UIKit.Prog, icChart, null);
            foreach (var t in header.GetComponentsInChildren<TMP_Text>(true)) t.color = UIKit.Hex("654D13");
            foreach (var i in header.GetComponentsInChildren<Image>(true))
                if (i.gameObject.name == "Icon" || i.gameObject.name == "Back") i.color = UIKit.Hex("654D13");
            CircleBtn(p, spStar, new Color(1, 1, 1, .18f), UIKit.Hex("654D13"), () => ShowOnly(pInsignias), 1f, 1f, 86, -150, -125);
            CircleBtn(p, icGear, new Color(1, 1, 1, .18f), UIKit.Hex("654D13"), () => ShowOnly(pColeccion), 1f, 1f, 86, -55, -125);

            var viewport = UIKit.Node("ProgressViewport", p);
            viewport.anchorMin = Vector2.zero; viewport.anchorMax = Vector2.one;
            viewport.offsetMin = new Vector2(0, 82f * S); viewport.offsetMax = new Vector2(0, -235);
            viewport.gameObject.AddComponent<RectMask2D>();
            var vi = viewport.gameObject.AddComponent<Image>(); vi.color = new Color(1, 1, 1, 0);
            var content = UIKit.Node("ProgressContent", viewport);
            content.anchorMin = new Vector2(.5f, 1); content.anchorMax = new Vector2(.5f, 1);
            content.pivot = new Vector2(.5f, 1); content.sizeDelta = new Vector2(land ? 1000 : 1080, 2350);
            var scroll = viewport.gameObject.AddComponent<ScrollRect>();
            scroll.content = content; scroll.viewport = viewport; scroll.horizontal = false; scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Elastic; scroll.inertia = true;
            float w = land ? 1000 : 1080, y = 28f * S;
            var title = UIKit.Text(content, "Mi aventura", 28f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Top(title.rectTransform, 0, y, w - 40f * S, 45f * S); y += 68f * S;
            // Tarjeta de nivel (medidas de Figma: padding 20, avatar 78)
            float cardW = w - 40f * S, cardH = 182f * S;
            var level = UIKit.Box(content, Color.white, Mathf.RoundToInt(22f * S), "LevelCard");
            UIKit.Top(level.rectTransform, 0, y, cardW, cardH); UIKit.AddShadow(level, Mathf.RoundToInt(22f * S), .10f, -8, 8);
            var avatar = UIKit.Box(level.transform, UIKit.Hex("FFF5CE"), Mathf.RoundToInt(18f * S), "Avatar");
            UIKit.TopLeft(avatar.rectTransform, 20f * S, 20f * S, 78f * S, 78f * S);
            var robot = UIKit.Img(avatar.transform, robotVerde, "Robot"); robot.preserveAspect = true;
            UIKit.Stretch(robot.rectTransform, 6f * S);
            float infoX = 112f * S, infoW = cardW - infoX - 20f * S;
            var statsTxt = UIKit.Text(level.transform, "", 20f * S, UIKit.Navy, TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.TopLeft(statsTxt.rectTransform, infoX, 18f * S, infoW, 48f * S);
            var stars = UIKit.Box(level.transform, UIKit.Hex("FFF5CE"), Mathf.RoundToInt(14f * S), "Stars");
            UIKit.TopLeft(stars.rectTransform, infoX, 68f * S, infoW, 40f * S);
            var star = UIKit.Img(stars.transform, figStar, "Star"); star.color = UIKit.Prog; star.preserveAspect = true;
            UIKit.TopLeft(star.rectTransform, 10f * S, 9f * S, 22f * S, 22f * S);
            var starValue = UIKit.Text(stars.transform, AppState.Stars + " estrellas", 18f * S, UIKit.Hex("654D13"), TextAlignmentOptions.Left);
            UIKit.TopLeft(starValue.rectTransform, 40f * S, 4f * S, infoW - 50f * S, 32f * S);
            var percent = UIKit.Text(level.transform, "0%", 15f * S, UIKit.Hex("C98A0E"), TextAlignmentOptions.Right);
            UIKit.TopLeft(percent.rectTransform, 20f * S, 120f * S, cardW - 40f * S, 22f * S);
            var overallFill = ProgressBar(level.transform, 0f, 20f * S / cardW, 20f * S / cardH, 1f - 20f * S / cardW, 32f * S / cardH, UIKit.Prog, .8f);
            y += cardH + 32f * S;
            var discTitle = UIKit.Text(content, "Mis descubrimientos", 23f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Top(discTitle.rectTransform, 0, y, w - 40f * S, 42f * S); y += 62f * S;
            float bookW = w - 40f * S;
            var bookCard = UIKit.Box(content, Color.white, Mathf.RoundToInt(20f * S), "BookProgress"); UIKit.Top(bookCard.rectTransform, 0, y, bookW, 285f * S); UIKit.AddShadow(bookCard, Mathf.RoundToInt(20f * S), .09f, -6, 6);
            var bookFill = new Image[3]; var bookCount = new TMP_Text[3];
            string[] labels = { "Libro Fisiológico", "Libro Nutricional", "Libro Clínico" };
            Color[] colors = { UIKit.Coral, UIKit.Nutri, UIKit.Clin };
            for (int i = 0; i < 3; i++)
            {
                float by = 28f * S + i * 82f * S;
                var label = UIKit.Text(bookCard.transform, labels[i], 16f * S, UIKit.Navy, TextAlignmentOptions.Left);
                UIKit.TopLeft(label.rectTransform, 20f * S, by, bookW - 150f * S, 38f * S);
                bookCount[i] = UIKit.Text(bookCard.transform, "0%", 15f * S, UIKit.Muted, TextAlignmentOptions.Right, FontStyles.Normal);
                UIKit.TopLeft(bookCount[i].rectTransform, bookW - 125f * S, by, 105f * S, 38f * S);
                var bookTrack = UIKit.Box(bookCard.transform, UIKit.Hex("E2ECF5"), Mathf.RoundToInt(8f * S), "BookTrack");
                UIKit.TopLeft(bookTrack.rectTransform, 20f * S, by + 48f * S,
                    bookW - 40f * S, 13f * S);
                var bookBar = UIKit.Box(bookTrack.transform, colors[i], Mathf.RoundToInt(8f * S), "BookFill");
                UIKit.Frac(bookBar.rectTransform, 0f, 0f, 0f, 1f);
                Anim.Llenar(bookBar.GetComponent<Image>(), .8f);
                bookFill[i] = bookBar.GetComponent<Image>();
            }
            y += 320f * S;
            var photoTitle = UIKit.Text(content, "Mis fotos", 23f * S, UIKit.Navy, TextAlignmentOptions.Left); UIKit.Top(photoTitle.rectTransform, 0, y, w - 40f * S, 42f * S); y += 58f * S;
            var photoGrid = UIKit.Node("PhotoGrid", content); UIKit.Top(photoGrid, 0, y, w - 40f * S, 220f * S);
            BuildBottomNav(p, 1);
            p.gameObject.AddComponent<ProgresoRefs>().Set(statsTxt, overallFill, bookFill, bookCount, photoGrid, percent);
            return p;
        }

        void RefreshProgreso()
        {
            if (pProgreso == null) return;
            var refs = pProgreso.GetComponent<ProgresoRefs>();
            if (refs == null) return;
            int total = AppState.TopicsSeenCount();
            if (refs.Stats != null)
                refs.Stats.text = "<b><size=120%>Nivel " + AppState.Level + "</size></b>\n<size=75%>Explorador principiante</size>";
            SetProgress(refs.OverallFill, total / 12f);
            if (refs.Percent != null) refs.Percent.text = Mathf.RoundToInt(Mathf.Clamp01(total / 12f) * 100f) + "%";
            for (int i = 0; i < 3; i++)
            {
                int seen = AppState.TopicsSeenCount(i);
                if (refs.BookFill != null && refs.BookFill[i] != null) SetProgress(refs.BookFill[i], seen / 4f);
                if (refs.BookCount != null && refs.BookCount[i] != null) refs.BookCount[i].text = (seen * 25) + "%";
            }
            RefreshPhotoGrid(refs.PhotoGrid);
        }

        void RefreshPhotoGrid(RectTransform grid)
        {
            if (grid == null) return;
            for (int i = grid.childCount - 1; i >= 0; i--) Destroy(grid.GetChild(i).gameObject);

            string[] files = new string[0];
            string dir = System.IO.Path.Combine(Application.persistentDataPath, "Fotos");
            if (System.IO.Directory.Exists(dir))
            {
                files = System.IO.Directory.GetFiles(dir, "*.png");
                Array.Sort(files, StringComparer.Ordinal);
                Array.Reverse(files); // más recientes primero (nombre = timestamp)
            }
            int n = Mathf.Min(files.Length, 6);
            if (n == 0)
            {
                var msg = UIKit.Text(grid, "Aún no tomaste fotos. Andá a una Experiencia AR y tocá el botón de cámara.",
                    28, UIKit.Muted, TextAlignmentOptions.Center, FontStyles.Normal);
                UIKit.Stretch(msg.rectTransform);
                return;
            }
            for (int i = 0; i < n; i++)
            {
                var rc = UIKit.Cell(i, 3, 2, 0f, 0f, 1f, 1f, 0.05f, 0.08f);
                string path = files[i];
                var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                tex.LoadImage(System.IO.File.ReadAllBytes(path));
                var thumb = UIKit.Button(grid, "", UIKit.Card, UIKit.Navy, 20, () => OpenPhotoViewer(path), 0, false);
                UIKit.Frac(R(thumb), rc.xMin, rc.yMin, rc.xMax, rc.yMax);
                var raw = new GameObject("Thumb", typeof(RectTransform)).AddComponent<RawImage>();
                raw.transform.SetParent(thumb.transform, false); raw.texture = tex; raw.raycastTarget = false;
                UIKit.Stretch(raw.rectTransform, 4);
            }
        }

        void OpenPhotoViewer(string path)
        {
            if (photoViewer == null || photoViewerImg == null) return;
            var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(System.IO.File.ReadAllBytes(path));
            photoViewerImg.texture = tex;
            photoViewer.gameObject.SetActive(true);
        }

        // ============================================================
        // 7B - INSIGNIAS (vitrina de logros, accedida desde Progreso)
        // ============================================================
        RectTransform BuildInsignias()
        {
            var p = Panel("7B_Insignias");
            bool land = Land;
            BuildHeaderBar(p, "Insignias", UIKit.Prog, spStar, () => ShowOnly(pProgreso));

            var subtitle = UIKit.Text(p, "", 34, UIKit.Navy, TextAlignmentOptions.Center, FontStyles.Bold);
            UIKit.Frac(subtitle, 0.06f, land ? 0.795f : 0.825f, 0.94f, land ? 0.850f : 0.870f);

            // Mismo ícono que ya usa cada libro/sección en el resto de la app (nada nuevo que cargar).
            Sprite[] icons = { spIconFisio, spIconNutri, spIconClinico, spStar,
                                spIconJuegos, spIconJuegos, spIconJuegos, spIconScan, icCalendar };

            var badges = AppState.Badges;
            var iconBgs = new Image[badges.Length];
            var titles = new TMP_Text[badges.Length];
            var status = new TMP_Text[badges.Length];

            const float S = 2.77f;
            float x0 = 0.08f, x1 = 0.92f, y0 = 0.03f, y1 = land ? 0.74f : 0.79f;
            for (int i = 0; i < badges.Length; i++)
            {
                var rc = UIKit.Cell(i, 2, Mathf.CeilToInt(badges.Length / 2f), x0, y0, x1, y1, 0.04f, 0.025f);
                var b = badges[i];
                var card = UIKit.Box(p, UIKit.Card, Mathf.RoundToInt(18f * S), "Badge" + i);
                UIKit.Frac(card.rectTransform, rc.xMin, rc.yMin, rc.xMax, rc.yMax);

                // Tamaño fijo en píxeles (no Frac): la tarjeta no es cuadrada, y con preserveAspect
                // un Frac de aspecto no-1:1 encogería el círculo en vez de llenarlo bien.
                var iconBg = UIKit.Img(card.transform, UIKit.Circle(), "IconBg"); iconBg.color = b.Color;
                var ibrt = R(iconBg);                 ibrt.anchorMin = ibrt.anchorMax = new Vector2(0.5f, 0.78f);
                ibrt.pivot = new Vector2(0.5f, 0.5f); ibrt.anchoredPosition = Vector2.zero; ibrt.sizeDelta = new Vector2(48f * S, 48f * S);
                var iconImg = UIKit.Img(iconBg.transform, icons[i], "Icon"); iconImg.color = Color.white;
                UIKit.Stretch(iconImg.rectTransform, 20);

                var title = UIKit.Text(card.transform, b.Title, 14f * S, UIKit.Navy, TextAlignmentOptions.Center, FontStyles.Bold);
                UIKit.Frac(title, 0.05f, 0.34f, 0.95f, 0.48f);
                var desc = UIKit.Text(card.transform, b.Desc, 12f * S, UIKit.Muted, TextAlignmentOptions.Center, FontStyles.Normal);
                UIKit.Frac(desc, 0.05f, 0.14f, 0.95f, 0.34f);
                var st = UIKit.Text(card.transform, "", 12f * S, UIKit.Muted, TextAlignmentOptions.Center, FontStyles.Bold);
                UIKit.Frac(st, 0.05f, 0.02f, 0.95f, 0.14f);

                UIKit.AddShadow(card, Mathf.RoundToInt(18f * S), 0.10f, -4, 4);

                iconBgs[i] = iconBg; titles[i] = title; status[i] = st;
            }

            p.gameObject.AddComponent<InsigniasRefs>().Set(subtitle, iconBgs, titles, status);
            return p;
        }

        void RefreshInsignias()
        {
            if (pInsignias == null) return;
            var refs = pInsignias.GetComponent<InsigniasRefs>();
            if (refs == null) return;
            var badges = AppState.Badges;
            int got = AppState.BadgesUnlockedCount();
            if (refs.Subtitle != null) refs.Subtitle.text = got + "/" + badges.Length + " insignias conseguidas";
            for (int i = 0; i < badges.Length; i++)
            {
                bool on = AppState.IsBadgeUnlocked(i);
                if (refs.IconBgs != null && refs.IconBgs[i] != null)
                    refs.IconBgs[i].color = on ? badges[i].Color : Color.Lerp(badges[i].Color, UIKit.Muted, 0.75f);
                if (refs.Titles != null && refs.Titles[i] != null)
                    refs.Titles[i].color = on ? UIKit.Navy : UIKit.Muted;
                if (refs.Status != null && refs.Status[i] != null)
                {
                    refs.Status[i].text = on ? "Conseguida" : "Bloqueada";
                    refs.Status[i].color = on ? UIKit.Nutri : UIKit.Muted;
                }
            }
        }

        // ============================================================
        // 7C - COLECCIÓN AR (estampita por cada uno de los 12 marcadores, accedida desde Progreso)
        // ============================================================
        RectTransform BuildColeccion()
        {
            var p = Panel("7C_Coleccion");
            bool land = Land;
            BuildHeaderBar(p, "Colección AR", UIKit.Scan, spIconScan, () => ShowOnly(pProgreso));

            var subtitle = UIKit.Text(p, "", 34, UIKit.Navy, TextAlignmentOptions.Center, FontStyles.Bold);
            UIKit.Frac(subtitle, 0.06f, land ? 0.795f : 0.825f, 0.94f, land ? 0.850f : 0.870f);

            // Columna = libro, fila = tema: se lee como un álbum de estampitas, un libro por columna.
            const float S = 2.77f;
            const int cols = 2, rows = 6, n = 12;
            float x0 = 0.08f, x1 = 0.92f, y0 = 0.03f, y1 = land ? 0.74f : 0.79f;
            var iconBgs = new Image[n];
            var titles = new TMP_Text[n];
            var status = new TMP_Text[n];
            var cardBook = new int[n];
            var cardTopic = new int[n];

            for (int topic = 0; topic < 4; topic++)
            {
                for (int book = 0; book < 3; book++)
                {
                    int i = topic * cols + book;
                    var rc = UIKit.Cell(i, cols, rows, x0, y0, x1, y1, 0.03f, 0.035f);
                    var b = books[book];
                    var card = UIKit.Box(p, UIKit.Card, Mathf.RoundToInt(18f * S), "Stamp" + book + "_" + topic);
                    UIKit.Frac(card.rectTransform, rc.xMin, rc.yMin, rc.xMax, rc.yMax);

                    // Tamaño fijo en píxeles (no Frac), mismo motivo que en Insignias: la tarjeta
                    // no es cuadrada y con preserveAspect un Frac no-1:1 encoge el círculo.
                    var iconBg = UIKit.Img(card.transform, UIKit.Circle(), "IconBg"); iconBg.color = b.Accent;
                    var ibrt = R(iconBg);                     ibrt.anchorMin = ibrt.anchorMax = new Vector2(0.5f, 0.80f);
                    ibrt.pivot = new Vector2(0.5f, 0.5f); ibrt.anchoredPosition = Vector2.zero; ibrt.sizeDelta = new Vector2(42f * S, 42f * S);
                    Sprite topicIcon = (b.TopicIcon != null && topic < b.TopicIcon.Length) ? b.TopicIcon[topic] : null;
                    var iconImg = UIKit.Img(iconBg.transform, topicIcon, "Icon"); iconImg.color = Color.white;
                    UIKit.Stretch(iconImg.rectTransform, 16);

                    string title = (b.TopicTitle != null && topic < b.TopicTitle.Length) ? b.TopicTitle[topic] : "Tema";
                    var t = UIKit.Text(card.transform, title, 14f * S, UIKit.Navy, TextAlignmentOptions.Center, FontStyles.Bold);
                    UIKit.Frac(t, 0.03f, 0.30f, 0.97f, 0.54f);
                    var sub = UIKit.Text(card.transform, b.Title, 11f * S, UIKit.Muted, TextAlignmentOptions.Center, FontStyles.Normal);
                    UIKit.Frac(sub, 0.02f, 0.16f, 0.98f, 0.38f);
                    var st = UIKit.Text(card.transform, "", 11f * S, UIKit.Muted, TextAlignmentOptions.Center, FontStyles.Bold);
                    UIKit.Frac(st, 0.05f, 0.02f, 0.95f, 0.18f);

                    UIKit.AddShadow(card, Mathf.RoundToInt(18f * S), 0.10f, -4, 4);

                    iconBgs[i] = iconBg; titles[i] = t; status[i] = st; cardBook[i] = book; cardTopic[i] = topic;
                }
            }

            p.gameObject.AddComponent<ColeccionRefs>().Set(subtitle, iconBgs, titles, status, cardBook, cardTopic);
            return p;
        }

        void RefreshColeccion()
        {
            if (pColeccion == null) return;
            var refs = pColeccion.GetComponent<ColeccionRefs>();
            if (refs == null) return;
            if (refs.Subtitle != null) refs.Subtitle.text = AppState.MarkersScannedCount() + "/12 páginas coleccionadas";
            for (int i = 0; i < 12; i++)
            {
                int book = refs.CardBook[i], topic = refs.CardTopic[i];
                bool on = AppState.IsMarkerScanned(book, topic);
                var accent = books[book].Accent;
                if (refs.IconBgs != null && refs.IconBgs[i] != null)
                    refs.IconBgs[i].color = on ? accent : Color.Lerp(accent, UIKit.Muted, 0.75f);
                if (refs.Titles != null && refs.Titles[i] != null)
                    refs.Titles[i].color = on ? UIKit.Navy : UIKit.Muted;
                if (refs.Status != null && refs.Status[i] != null)
                {
                    refs.Status[i].text = on ? "Coleccionada" : "Sin escanear";
                    refs.Status[i].color = on ? UIKit.Nutri : UIKit.Muted;
                }
            }
        }
    }
}
