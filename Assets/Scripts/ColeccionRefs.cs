using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    class ColeccionRefs : MonoBehaviour
    {
        public TMP_Text Subtitle; public Image[] IconBgs; public TMP_Text[] Titles; public TMP_Text[] Status;
        public int[] CardBook; public int[] CardTopic;
        public void Set(TMP_Text subtitle, Image[] iconBgs, TMP_Text[] titles, TMP_Text[] status, int[] cardBook, int[] cardTopic)
        { Subtitle = subtitle; IconBgs = iconBgs; Titles = titles; Status = status; CardBook = cardBook; CardTopic = cardTopic; }
    }
}
