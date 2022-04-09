using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CustomDrag
{
    public class DraggableImage : DraggableObject, IPointerDownHandler, IPointerUpHandler
    {
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            StartDrag(image.sprite);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            EndDrag();
        }

        protected override void Hide()
        {
            image.color = new Color(1, 1, 1, 0);
        }

        protected override void Show()
        {
            image.color = new Color(1, 1, 1, 1);
        }
    }
}