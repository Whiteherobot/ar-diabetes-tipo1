using UnityEngine;
using TMPro;

namespace ARDiabetes
{
    class ConfigRefs : MonoBehaviour
    {
        public TMP_Text ProfileSub, MuteSub;
        public RectTransform ResetModal;
        public void Set(TMP_Text profileSub, TMP_Text muteSub, RectTransform resetModal = null)
        { ProfileSub = profileSub; MuteSub = muteSub; ResetModal = resetModal; }
    }
}
