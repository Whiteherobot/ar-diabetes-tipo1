using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ARDiabetes
{
    // Arrastre horizontal sobre la pantalla de AR para rotar la figura (real o de respaldo).
    class DragRotate : MonoBehaviour, IDragHandler
    {
        public Action<float> OnDragX;
        public void OnDrag(PointerEventData e) => OnDragX?.Invoke(e.delta.x);
    }
}
