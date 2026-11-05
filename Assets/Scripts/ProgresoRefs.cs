using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    class ProgresoRefs : MonoBehaviour
    {
        public TMP_Text Stats; public TMP_Text Percent; public Image OverallFill; public Image[] BookFill; public TMP_Text[] BookCount;
        public RectTransform PhotoGrid;
        public void Set(TMP_Text stats, Image overallFill, Image[] bookFill, TMP_Text[] bookCount, RectTransform photoGrid, TMP_Text percent)
        { Stats = stats; OverallFill = overallFill; BookFill = bookFill; BookCount = bookCount; PhotoGrid = photoGrid; Percent = percent; }
    }
}
