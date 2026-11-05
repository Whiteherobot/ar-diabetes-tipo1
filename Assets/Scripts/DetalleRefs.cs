using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    class DetalleRefs : MonoBehaviour
    {
        public Image Band, Img; public TMP_Text Title, Desc;
        public RectTransform Content, Learn, LearnShadow, Coach, Interact, InteractShadow;
        public float AnchoColumna;
        public void Set(Image band, Image img, TMP_Text title, TMP_Text desc) { Band = band; Img = img; Title = title; Desc = desc; }
    }
}
