using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ARDiabetes
{
    // Arrastre horizontal sobre la pantalla de AR para rotar la figura (real o de respaldo).
    // Un toque sin arrastre (click) se pasa aparte, para los modelos interactivos.
    class DragRotate : MonoBehaviour, IDragHandler, IPointerClickHandler
    {
        public Action<float> OnDragX;
        public Action<Vector2, Camera> OnTap;
        public void OnDrag(PointerEventData e) => OnDragX?.Invoke(e.delta.x);
        public void OnPointerClick(PointerEventData e) { if (!e.dragging) OnTap?.Invoke(e.position, e.pressEventCamera); }
    }
}
