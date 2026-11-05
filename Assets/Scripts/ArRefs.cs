using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    class ArRefs : MonoBehaviour
    {
        public RawImage Raw; public Image Bg; public TMP_Text Hint, Title, Toast;
        public void Set(RawImage raw, Image bg, TMP_Text hint, TMP_Text title, TMP_Text toast)
        { Raw = raw; Bg = bg; Hint = hint; Title = title; Toast = toast; }
    }
}
