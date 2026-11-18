using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    class DetalleRefs : MonoBehaviour
    {
        public Image Band, Img; public TMP_Text Title, Desc;
        public RectTransform Content, Learn, LearnShadow, Coach, Interact, InteractShadow;
        public RectTransform BandShadow; public TMP_Text BandLabel; public Color Accent;
        public float AnchoColumna;
        public Sprite ImgPorDefecto; // imagen del bloque DESCUBRE cuando el tema no trae ilustración
        public void Set(Image band, Image img, TMP_Text title, TMP_Text desc) { Band = band; Img = img; Title = title; Desc = desc; }
    }
}
