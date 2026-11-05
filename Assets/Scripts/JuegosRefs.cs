using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    class JuegosRefs : MonoBehaviour
    {
        public TMP_Text[] Count; public Image[] Fill;
        public void Set(TMP_Text[] count, Image[] fill) { Count = count; Fill = fill; }
    }
}
