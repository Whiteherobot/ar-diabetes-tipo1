using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    class InsigniasRefs : MonoBehaviour
    {
        public TMP_Text Subtitle; public Image[] IconBgs; public TMP_Text[] Titles; public TMP_Text[] Status;
        public void Set(TMP_Text subtitle, Image[] iconBgs, TMP_Text[] titles, TMP_Text[] status)
        { Subtitle = subtitle; IconBgs = iconBgs; Titles = titles; Status = status; }
    }
}
