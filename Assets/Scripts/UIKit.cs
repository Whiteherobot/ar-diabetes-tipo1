using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace ARDiabetes
{
    /// <summary>
    /// Kit de UI: paleta, generadores de sprites (rounded rect, círculo, degradado)
    /// y fábricas de widgets (cajas redondeadas, botones con feedback, textos).
    /// Todo por código para que las pantallas sean reproducibles y versionables.
    /// </summary>
    public static class UIKit
    {
        // ---------- Paleta ----------
        public static readonly Color Sky      = Hex("EDF8FF");
        public static readonly Color SkyTop   = Hex("D8ECFB");
        public static readonly Color Card     = Hex("FFFFFF");
        public static readonly Color Navy     = Hex("20365A");
        public static readonly Color Blue     = Hex("3289EC");
        public static readonly Color BlueDark = Hex("2470C9");
        public static readonly Color Muted    = Hex("64748B");
        public static readonly Color Soft     = Hex("F1F6FB");
        public static readonly Color Fisio    = Hex("8370DA");
        public static readonly Color Nutri    = Hex("2FAE75");
        public static readonly Color Clin     = Hex("3289EC");
        public static readonly Color Scan     = Hex("FF9138");
        public static readonly Color Juegos   = Hex("EE5997");
        public static readonly Color Prog     = Hex("FFB830");
        public static readonly Color Coral    = Hex("E94F58");

        public static TMP_FontAsset Font;
        public static float TextScale = 1f;

        // ---------- Sprites generados ----------
        static readonly Dictionary<int, Sprite> _rounded = new Dictionary<int, Sprite>();
        static Sprite _circle;

        public static Sprite Rounded(int radius)
        {
            if (_rounded.TryGetValue(radius, out var s)) return s;
            int r = Mathf.Max(2, radius);
            int tex = r * 2 + 4;
            var t = new Texture2D(tex, tex, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[tex * tex];
            for (int y = 0; y < tex; y++)
                for (int x = 0; x < tex; x++)
                {
                    float cx = Mathf.Clamp(x, r, tex - 1 - r);
                    float cy = Mathf.Clamp(y, r, tex - 1 - r);
                    float d = Mathf.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
                    float a = Mathf.Clamp01(r + 0.5f - d);
                    px[y * tex + x] = new Color32(255, 255, 255, (byte)(a * 255));
                }
            t.SetPixels32(px); t.Apply();
            s = Sprite.Create(t, new Rect(0, 0, tex, tex), new Vector2(0.5f, 0.5f), 100f, 0,
                              SpriteMeshType.FullRect, new Vector4(r, r, r, r));
            _rounded[radius] = s;
            return s;
        }

        public static Sprite Circle()
        {
            if (_circle != null) return _circle;
            int tex = 128; float rad = tex / 2f;
            var t = new Texture2D(tex, tex, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[tex * tex];
            for (int y = 0; y < tex; y++)
                for (int x = 0; x < tex; x++)
                {
                    float d = Mathf.Sqrt((x - rad + 0.5f) * (x - rad + 0.5f) + (y - rad + 0.5f) * (y - rad + 0.5f));
                    float a = Mathf.Clamp01(rad - d);
                    px[y * tex + x] = new Color32(255, 255, 255, (byte)(a * 255));
                }
            t.SetPixels32(px); t.Apply();
            _circle = Sprite.Create(t, new Rect(0, 0, tex, tex), new Vector2(0.5f, 0.5f), 100f);
            return _circle;
        }

        public static Sprite VerticalGradient(Color top, Color bottom)
        {
            int h = 256;
            var t = new Texture2D(1, h, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp };
            for (int y = 0; y < h; y++)
                t.SetPixel(0, y, Color.Lerp(bottom, top, y / (float)(h - 1)));
            t.Apply();
            return Sprite.Create(t, new Rect(0, 0, 1, h), new Vector2(0.5f, 0.5f), 100f);
        }

        // ---------- Nodos ----------
        public static RectTransform Node(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return (RectTransform)go.transform;
        }

        public static Image Img(Transform parent, Sprite s = null, string name = "Img")
        {
            var rt = Node(name, parent);
            var im = rt.gameObject.AddComponent<Image>();
            im.sprite = s; im.raycastTarget = false; im.preserveAspect = s != null;
            return im;
        }

        public static Image Box(Transform parent, Color color, int radius = 32, string name = "Box")
        {
            var rt = Node(name, parent);
            var im = rt.gameObject.AddComponent<Image>();
            im.sprite = Rounded(radius);
            im.type = Image.Type.Sliced;
            im.color = color;
            im.raycastTarget = false;
            return im;
        }

        const int blur = 60;

        // Sombra difusa como el box-shadow del diseño (no una copia sólida de la tarjeta).
        // grow se conserva por compatibilidad: el alcance lo da el difuminado.
        public static Image AddShadow(Image box, int radius, float alpha = 0.14f, float dy = -8f, float grow = 10f)
        {
            var brt = box.rectTransform;
            var sh = Img(brt.parent, Difuminado(radius, blur), "Shadow");
            sh.type = Image.Type.Sliced;
            sh.color = new Color(0.08f, 0.16f, 0.30f, alpha * .6f);
            var srt = sh.rectTransform;
            srt.anchorMin = brt.anchorMin; srt.anchorMax = brt.anchorMax; srt.pivot = brt.pivot;
            float baja = dy * 2.5f; // las sombras del diseño caen hacia abajo
            srt.offsetMin = brt.offsetMin + new Vector2(-blur, -blur + baja);
            srt.offsetMax = brt.offsetMax + new Vector2(blur, blur + baja);
            sh.raycastTarget = false;
            srt.SetSiblingIndex(brt.GetSiblingIndex());
            return sh;
        }

        static readonly Dictionary<long, Sprite> _difuminados = new Dictionary<long, Sprite>();

        // Reubica la sombra cuando la tarjeta cambia de lugar o de alto (pasar el mismo dy de AddShadow).
        public static void AjustarSombra(RectTransform sombra, RectTransform box, float dy)
        {
            sombra.anchorMin = box.anchorMin; sombra.anchorMax = box.anchorMax; sombra.pivot = box.pivot;
            sombra.sizeDelta = box.sizeDelta + new Vector2(blur * 2f, blur * 2f);
            sombra.anchoredPosition = box.anchoredPosition + new Vector2(0, blur + dy * 2.5f);
        }

        static Sprite Difuminado(int radius, int blur)
        {
            long clave = ((long)radius << 16) | (uint)blur;
            if (_difuminados.TryGetValue(clave, out var s)) return s;
            int r = Mathf.Max(2, radius), borde = r + blur, tex = borde * 2 + 4;
            var t = new Texture2D(tex, tex, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp };
            var px = new Color32[tex * tex];
            for (int y = 0; y < tex; y++)
                for (int x = 0; x < tex; x++)
                {
                    float cx = Mathf.Clamp(x, borde, tex - 1 - borde);
                    float cy = Mathf.Clamp(y, borde, tex - 1 - borde);
                    float d = Mathf.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy)) - r;
                    // como box-shadow: en el borde de la tarjeta ya va a la mitad
                    float a = .5f * (1f - Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(d / blur)));
                    px[y * tex + x] = new Color32(255, 255, 255, (byte)(a * 255));
                }
            t.SetPixels32(px); t.Apply();
            s = Sprite.Create(t, new Rect(0, 0, tex, tex), new Vector2(0.5f, 0.5f), 100f, 0,
                              SpriteMeshType.FullRect, new Vector4(borde, borde, borde, borde));
            _difuminados[clave] = s;
            return s;
        }

        public static TMP_Text Text(Transform parent, string s, float size, Color c,
                                    TextAlignmentOptions al = TextAlignmentOptions.Center,
                                    FontStyles style = FontStyles.Bold)
        {
            var rt = Node("Text", parent);
            var t = rt.gameObject.AddComponent<TextMeshProUGUI>();
            if (Font != null) t.font = Font;
            t.text = s; t.color = c; t.alignment = al;
            // "Bold" equivale al peso 800 del diseño; la tabla de pesos de Nunito elige el archivo.
            if ((style & FontStyles.Bold) != 0) { t.fontStyle = style & ~FontStyles.Bold; t.fontWeight = FontWeight.Heavy; }
            else t.fontStyle = style;
            t.enableWordWrapping = true; t.raycastTarget = false;
            // Auto-ajuste: el texto nunca desborda su caja (clave para portrait/landscape).
            t.enableAutoSizing = true;
            float scaled = size * TextScale;
            t.fontSizeMax = scaled;
            t.fontSizeMin = Mathf.Max(10f, scaled * 0.35f);
            return t;
        }

        public static Button Button(Transform parent, string label, Color bg, Color fg, int radius,
                                    UnityAction onClick, int fontSize = 46, bool press = true)
        {
            var box = Box(parent, bg, radius, "Button");
            box.raycastTarget = true;
            var b = box.gameObject.AddComponent<Button>();
            b.targetGraphic = box;
            b.transition = Selectable.Transition.None;
            if (press) box.gameObject.AddComponent<PressEffect>();
            if (onClick != null) b.onClick.AddListener(onClick);
            if (!string.IsNullOrEmpty(label))
            {
                var t = Text(box.transform, label, fontSize, fg);
                Stretch(t.rectTransform);
            }
            return b;
        }

        // Copia oscura de la imagen, como el drop-shadow de los robots del diseño: separa al robot
        // de una tarjeta del mismo color.
        public static void SombraImagen(Image img, float alpha, float dy)
        {
            var sombra = Img(img.rectTransform.parent, img.sprite, "SombraRobot");
            sombra.color = new Color(0.1f, 0.16f, 0.25f, alpha); sombra.preserveAspect = true;
            var o = img.rectTransform; var d = sombra.rectTransform;
            d.anchorMin = o.anchorMin; d.anchorMax = o.anchorMax; d.pivot = o.pivot;
            d.sizeDelta = o.sizeDelta; d.anchoredPosition = o.anchoredPosition + new Vector2(0, -dy);
            d.SetSiblingIndex(o.GetSiblingIndex());
        }

        // Tamaño fijo, para poder medir el texto y acomodar lo que viene debajo.
        public static void TamanoFijo(TMP_Text t, float size)
        {
            t.enableAutoSizing = false;
            t.fontSize = size * TextScale;
        }

        public static float Alto(TMP_Text t, string s, float ancho) => t.GetPreferredValues(s, ancho, 0f).y;

        public static void Scroll(RectTransform viewport, RectTransform content)
        {
            var scroll = viewport.gameObject.AddComponent<ScrollRect>();
            scroll.content = content; scroll.viewport = viewport;
            scroll.horizontal = false; scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Elastic; scroll.inertia = true;
            scroll.scrollSensitivity = 8f;
        }

        // Texto en bloque como en CSS: arriba a la izquierda y con line-height relativo a la fuente.
        // 1.364 es la altura de línea propia de Nunito.
        public static void Interlinea(TMP_Text t, float alturaCss)
        {
            t.alignment = TextAlignmentOptions.TopLeft;
            t.lineSpacing = (alturaCss - 1.364f) * 100f;
        }

        // ---------- Layout helpers ----------
        // Anclaje por fracciones del padre (0..1). y: 0 abajo, 1 arriba. Responsivo por diseño.
        public static void Frac(RectTransform rt, float x0, float y0, float x1, float y1)
        {
            rt.anchorMin = new Vector2(x0, y0);
            rt.anchorMax = new Vector2(x1, y1);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        // Overload cómodo para Image / TMP_Text / cualquier Component con RectTransform.
        public static void Frac(Component c, float x0, float y0, float x1, float y1)
            => Frac((RectTransform)c.transform, x0, y0, x1, y1);

        // Celda i de una grilla dentro de la región [x0,y0]-[x1,y1] (fracciones), fila 0 arriba.
        public static Rect Cell(int i, int cols, int rows, float x0, float y0, float x1, float y1, float gx, float gy)
        {
            int c = i % cols, r = i / cols;
            float w = (x1 - x0 - gx * (cols - 1)) / cols;
            float h = (y1 - y0 - gy * (rows - 1)) / rows;
            float cx0 = x0 + c * (w + gx);
            float cyTop = y1 - r * (h + gy);
            return Rect.MinMaxRect(cx0, cyTop - h, cx0 + w, cyTop);
        }

        public static RectTransform Stretch(RectTransform rt, float pad = 0)
        {
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(pad, pad); rt.offsetMax = new Vector2(-pad, -pad);
            return rt;
        }

        // Ancla arriba-centro; y se mide desde el borde superior en px.
        public static void Top(RectTransform rt, float x, float yFromTop, float w, float h)
        {
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(x, -yFromTop);
            rt.sizeDelta = new Vector2(w, h);
        }

        // Ancla arriba-izquierda; x e y se miden desde esa esquina en px (como en Figma).
        public static void TopLeft(RectTransform rt, float x, float yFromTop, float w, float h)
        {
            rt.anchorMin = rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = new Vector2(x, -yFromTop);
            rt.sizeDelta = new Vector2(w, h);
        }

        public static void Center(RectTransform rt, float x, float y, float w, float h)
        {
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(x, y);
            rt.sizeDelta = new Vector2(w, h);
        }

        public static void Bottom(RectTransform rt, float x, float yFromBottom, float w, float h)
        {
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0f);
            rt.pivot = new Vector2(0.5f, 0f);
            rt.anchoredPosition = new Vector2(x, yFromBottom);
            rt.sizeDelta = new Vector2(w, h);
        }

        public static Color Hex(string hex)
        {
            ColorUtility.TryParseHtmlString("#" + hex, out var c);
            return c;
        }
    }
}
