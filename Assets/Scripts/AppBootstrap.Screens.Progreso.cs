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
            Color oscuro = UIKit.Hex("654D13");
            var header = BuildHeaderBar(p, "Tu progreso", UIKit.Prog, figChart, null);
            foreach (var t in header.GetComponentsInChildren<TMP_Text>(true)) t.color = oscuro;
            foreach (var i in header.GetComponentsInChildren<Image>(true))
                if (i.gameObject.name == "Icon") i.color = oscuro;
            CircleBtn(header, figStar, new Color(92f / 255f, 67f / 255f, 0, .12f), oscuro, () => ShowOnly(pInsignias), 1f, .5f, 28f * S, -59f * S, 0);
            CircleBtn(header, figScan, new Color(92f / 255f, 67f / 255f, 0, .12f), oscuro, () => ShowOnly(pColeccion), 1f, .5f, 28f * S, -26f * S, 0);

            float w = land ? 1000 : 1080, cardW = w - 40f * S, y = 24f * S;
            var content = BuildAreaScroll(p, "Progress", 82f * S, w);
            var title = UIKit.Text(content, "Mi aventura", 28f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Top(title.rectTransform, 0, y, cardW, 38f * S); y += 50f * S;

            float cardH = 230f * S;
            var level = UIKit.Box(content, Color.white, Mathf.RoundToInt(22f * S), "LevelCard");
            UIKit.Top(level.rectTransform, 0, y, cardW, cardH); UIKit.AddShadow(level, Mathf.RoundToInt(22f * S), .14f, -6, 6);
            var avatar = UIKit.Box(level.transform, UIKit.Hex("FFF5D5"), Mathf.RoundToInt(24f * S), "Avatar");
            UIKit.TopLeft(avatar.rectTransform, 22f * S, 22f * S, 92f * S, 100f * S);
            avatar.gameObject.AddComponent<Mask>();
            var robot = UIKit.Img(avatar.transform, PersonajeElegido(), "Robot"); robot.preserveAspect = true;
            UIKit.TopLeft(robot.rectTransform, 5f * S, 12f * S, 82f * S, 99f * S);
            float infoX = 128f * S, infoW = cardW - infoX - 22f * S;
            var statsTxt = UIKit.Text(level.transform, "", 23f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.TopLeft(statsTxt.rectTransform, infoX, 22f * S, infoW, 32f * S);
            var rango = UIKit.Text(level.transform, "Explorador principiante", 15f * S, UIKit.Hex("627790"), TextAlignmentOptions.Left, FontStyles.Normal);
            UIKit.TopLeft(rango.rectTransform, infoX, 54f * S, infoW, 24f * S);
            var stars = UIKit.Box(level.transform, UIKit.Hex("FFF5CE"), Mathf.RoundToInt(14f * S), "Stars");
            UIKit.TopLeft(stars.rectTransform, infoX, 86f * S, infoW, 46f * S);
            var star = UIKit.Img(stars.transform, figStar, "Star"); star.color = UIKit.Prog; star.preserveAspect = true;
            UIKit.TopLeft(star.rectTransform, 10f * S, 11.5f * S, 23f * S, 23f * S);
            var starValue = UIKit.Text(stars.transform, AppState.Stars.ToString(), 17f * S, oscuro, TextAlignmentOptions.Left);
            UIKit.TopLeft(starValue.rectTransform, 38f * S, 8f * S, infoW - 48f * S, 30f * S);
            var percent = UIKit.Text(level.transform, "0%", 18f * S, UIKit.Hex("B97D00"), TextAlignmentOptions.Right);
            UIKit.TopLeft(percent.rectTransform, 22f * S, 156f * S, cardW - 44f * S, 26f * S);
            var levelTrack = UIKit.Box(level.transform, UIKit.Hex("E6EDF2"), Mathf.RoundToInt(6.5f * S), "LevelTrack");
            UIKit.TopLeft(levelTrack.rectTransform, 22f * S, 195f * S, cardW - 44f * S, 13f * S);
            var overallFill = UIKit.Box(levelTrack.transform, UIKit.Prog, Mathf.RoundToInt(6.5f * S), "LevelFill");
            UIKit.Frac(overallFill.rectTransform, 0f, 0f, 0f, 1f);
            Anim.Llenar(overallFill, .8f);
            y += cardH + 26f * S;

            var discTitle = UIKit.Text(content, "Mis descubrimientos", 22f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Top(discTitle.rectTransform, 0, y, cardW, 30f * S); y += 43f * S;
            var bookCard = UIKit.Box(content, Color.white, Mathf.RoundToInt(20f * S), "BookProgress");
            UIKit.Top(bookCard.rectTransform, 0, y, cardW, 200f * S); UIKit.AddShadow(bookCard, Mathf.RoundToInt(20f * S), .14f, -6, 6);
            var bookFill = new Image[3]; var bookCount = new TMP_Text[3];
            string[] labels = { "Libro Fisiológico", "Libro Nutricional", "Libro Clínico" };
            Color[] colors = { UIKit.Coral, UIKit.Nutri, UIKit.Clin };
            for (int i = 0; i < 3; i++)
            {
                float by = (22f + i * 59f) * S;
                var label = UIKit.Text(bookCard.transform, labels[i], 16f * S, UIKit.Navy, TextAlignmentOptions.Left);
                UIKit.TopLeft(label.rectTransform, 22f * S, by, cardW - 110f * S, 23f * S);
                bookCount[i] = UIKit.Text(bookCard.transform, "0%", 15f * S, UIKit.Hex("627790"), TextAlignmentOptions.Right);
                UIKit.TopLeft(bookCount[i].rectTransform, cardW - 82f * S, by, 60f * S, 23f * S);
                var bookTrack = UIKit.Box(bookCard.transform, UIKit.Hex("E8EEF3"), Mathf.RoundToInt(5.5f * S), "BookTrack");
                UIKit.TopLeft(bookTrack.rectTransform, 22f * S, by + 27f * S, cardW - 44f * S, 11f * S);
                var bookBar = UIKit.Box(bookTrack.transform, colors[i], Mathf.RoundToInt(5.5f * S), "BookFill");
                UIKit.Frac(bookBar.rectTransform, 0f, 0f, 0f, 1f);
                Anim.Llenar(bookBar, .8f);
                bookFill[i] = bookBar;
            }
            y += 200f * S + 28f * S;

            var photoTitle = UIKit.Text(content, "Mis fotos", 22f * S, UIKit.Navy, TextAlignmentOptions.Left);
            UIKit.Top(photoTitle.rectTransform, 0, y, cardW, 30f * S); y += 43f * S;
            var photoGrid = UIKit.Node("PhotoGrid", content); UIKit.Top(photoGrid, 0, y, cardW, 103f * S);
            content.sizeDelta = new Vector2(w, y + 103f * S + 28f * S);
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
                refs.Stats.text = "Nivel " + AppState.Level;
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
            const float S = 2.77f;
            int n = Mathf.Min(files.Length, 6);
            int filas = n > 3 ? 2 : 1;
            float gap = 13f * S, celdaW = (grid.sizeDelta.x - 2f * gap) / 3f, celdaH = celdaW / 1.05f;
            float altoAntes = grid.sizeDelta.y;
            grid.sizeDelta = new Vector2(grid.sizeDelta.x, filas * celdaH + (filas - 1) * gap);
            var contenido = (RectTransform)grid.parent;
            contenido.sizeDelta += new Vector2(0, grid.sizeDelta.y - altoAntes);

            int casillas = filas * 3;
            for (int i = 0; i < casillas; i++)
            {
                float x = (i % 3) * (celdaW + gap), y = (i / 3) * (celdaH + gap);
                if (i >= n)
                {
                    FotoVacia(grid, x, y, celdaW, celdaH);
                    continue;
                }
                string path = files[i];
                var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                tex.LoadImage(System.IO.File.ReadAllBytes(path));
                var thumb = UIKit.Button(grid, "", UIKit.Hex("D89BAD"), UIKit.Navy, Mathf.RoundToInt(16f * S), () => OpenPhotoViewer(path), 10, false);
                UIKit.TopLeft(R(thumb), x, y, celdaW, celdaH);
                thumb.gameObject.AddComponent<Mask>();
                var raw = new GameObject("Thumb", typeof(RectTransform)).AddComponent<RawImage>();
                raw.transform.SetParent(thumb.transform, false); raw.texture = tex; raw.raycastTarget = false;
                // Recorte centrado para no deformar la captura
                float destino = celdaW / celdaH, origen = (float)tex.width / tex.height;
                raw.uvRect = origen > destino
                    ? new Rect((1f - destino / origen) / 2f, 0, destino / origen, 1)
                    : new Rect(0, (1f - origen / destino) / 2f, 1, origen / destino);
                UIKit.Stretch(raw.rectTransform);
                var cam = UIKit.Img(thumb.transform, figCamera != null ? figCamera : icCamera, "Camara"); cam.color = Color.white; cam.preserveAspect = true;
                UIKit.TopLeft(cam.rectTransform, celdaW - 21f * S, celdaH - 20f * S, 15f * S, 15f * S);
            }
        }

        // Casilla de foto pendiente: fondo claro con borde punteado y cámara en la esquina.
        void FotoVacia(RectTransform grid, float x, float y, float w, float h)
        {
            const float S = 2.77f;
            int radio = Mathf.RoundToInt(16f * S);
            var casilla = UIKit.Box(grid, new Color(1, 1, 1, .78f), radio, "FotoVacia");
            UIKit.TopLeft(casilla.rectTransform, x, y, w, h);
            Color borde = UIKit.Hex("BDCBD7");
            float grueso = 2f * S, trazo = 6f * S, paso = 11f * S;
            for (float d = radio; d + trazo <= w - radio; d += paso)
                foreach (float fila in new[] { 0f, h - grueso })
                {
                    var marca = UIKit.Box(casilla.transform, borde, 2, "Trazo");
                    UIKit.TopLeft(marca.rectTransform, d, fila, trazo, grueso);
                }
            for (float d = radio; d + trazo <= h - radio; d += paso)
                foreach (float col in new[] { 0f, w - grueso })
                {
                    var marca = UIKit.Box(casilla.transform, borde, 2, "Trazo");
                    UIKit.TopLeft(marca.rectTransform, col, d, grueso, trazo);
                }
            var cam = UIKit.Img(casilla.transform, figCamera != null ? figCamera : icCamera, "Camara"); cam.color = UIKit.Hex("8298AB"); cam.preserveAspect = true;
            UIKit.TopLeft(cam.rectTransform, w - 28f * S, h - 27f * S, 22f * S, 22f * S);
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
        // Tarjeta común de Insignias y Colección: ícono arriba y textos centrados, sin encimarse.
        Image TarjetaVitrina(RectTransform content, string nombre, int i, float cardW, float cardH, float yBase, Color color, Sprite icono,
                             string titulo, string detalle, float detalleH, out TMP_Text tituloTxt, out TMP_Text estado)
        {
            const float S = 2.77f;
            float gap = 12f * S, x = (i % 2 == 0 ? -1f : 1f) * (cardW + gap) / 2f;
            float y = yBase + (i / 2) * (cardH + gap);
            var card = UIKit.Box(content, UIKit.Card, Mathf.RoundToInt(18f * S), nombre);
            UIKit.Top(card.rectTransform, x, y, cardW, cardH);
            UIKit.AddShadow(card, Mathf.RoundToInt(18f * S), .10f, -4, 4);
            var iconBg = UIKit.Img(card.transform, UIKit.Circle(), "IconBg"); iconBg.color = color;
            UIKit.TopLeft(iconBg.rectTransform, (cardW - 52f * S) / 2f, 14f * S, 52f * S, 52f * S);
            var iconImg = UIKit.Img(iconBg.transform, icono, "Icon"); iconImg.color = Color.white; iconImg.preserveAspect = true;
            UIKit.Stretch(iconImg.rectTransform, 12f * S);
            tituloTxt = UIKit.Text(card.transform, titulo, 15f * S, UIKit.Navy);
            tituloTxt.lineSpacing = -20f;
            UIKit.TopLeft(tituloTxt.rectTransform, 10f * S, 72f * S, cardW - 20f * S, 40f * S);
            var det = UIKit.Text(card.transform, detalle, 13f * S, UIKit.Hex("61758D"), TextAlignmentOptions.Top, FontStyles.Normal);
            det.lineSpacing = -15f;
            UIKit.TopLeft(det.rectTransform, 10f * S, 114f * S, cardW - 20f * S, detalleH);
            estado = UIKit.Text(card.transform, "", 13f * S, UIKit.Muted);
            UIKit.TopLeft(estado.rectTransform, 10f * S, cardH - 30f * S, cardW - 20f * S, 20f * S);
            return iconBg;
        }

        RectTransform BuildInsignias()
        {
            const float S = 2.77f;
            var p = Panel("7B_Insignias");
            float columnW = Land ? 1000f : 1080f, cardW = (columnW - 40f * S - 12f * S) / 2f, cardH = 200f * S;
            BuildHeaderBar(p, "Insignias", UIKit.Prog, figStar, () => ShowOnly(pProgreso));
            var content = BuildAreaScroll(p, "Badges", 0f, columnW);
            var subtitle = UIKit.Text(content, "", 16f * S, UIKit.Navy);
            UIKit.Top(subtitle.rectTransform, 0, 20f * S, columnW - 40f * S, 26f * S);

            // Mismo ícono que ya usa cada libro/sección en el resto de la app (nada nuevo que cargar).
            Sprite[] icons = { spIconFisio, spIconNutri, spIconClinico, spStar,
                                spIconJuegos, spIconJuegos, spIconJuegos, spIconScan, icCalendar };

            var badges = AppState.Badges;
            var iconBgs = new Image[badges.Length];
            var titles = new TMP_Text[badges.Length];
            var status = new TMP_Text[badges.Length];
            for (int i = 0; i < badges.Length; i++)
                iconBgs[i] = TarjetaVitrina(content, "Badge" + i, i, cardW, cardH, 60f * S, badges[i].Color, icons[i],
                    badges[i].Title, badges[i].Desc, 52f * S, out titles[i], out status[i]);
            int filas = Mathf.CeilToInt(badges.Length / 2f);
            content.sizeDelta = new Vector2(columnW, 60f * S + filas * (cardH + 12f * S) + 16f * S);

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
            const float S = 2.77f;
            var p = Panel("7C_Coleccion");
            float columnW = Land ? 1000f : 1080f, cardW = (columnW - 40f * S - 12f * S) / 2f, cardH = 176f * S;
            BuildHeaderBar(p, "Colección AR", UIKit.Scan, figScan, () => ShowOnly(pProgreso));
            var content = BuildAreaScroll(p, "Stamps", 0f, columnW);
            var subtitle = UIKit.Text(content, "", 16f * S, UIKit.Navy);
            UIKit.Top(subtitle.rectTransform, 0, 20f * S, columnW - 40f * S, 26f * S);

            const int n = 12;
            var iconBgs = new Image[n];
            var titles = new TMP_Text[n];
            var status = new TMP_Text[n];
            var cardBook = new int[n];
            var cardTopic = new int[n];
            // Se lee como un álbum: primero los 4 temas de cada libro, en orden.
            for (int book = 0; book < 3; book++)
                for (int topic = 0; topic < 4; topic++)
                {
                    int i = book * 4 + topic;
                    var b = books[book];
                    iconBgs[i] = TarjetaVitrina(content, "Stamp" + book + "_" + topic, i, cardW, cardH, 60f * S, b.Accent, b.TopicIcon[topic],
                        b.TopicTitle[topic], b.Title, 24f * S, out titles[i], out status[i]);
                    cardBook[i] = book; cardTopic[i] = topic;
                }
            content.sizeDelta = new Vector2(columnW, 60f * S + 6 * (cardH + 12f * S) + 16f * S);

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
