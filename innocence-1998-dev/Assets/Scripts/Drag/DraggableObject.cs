using UnityEngine;

namespace CustomDrag
{
    public class DraggableObject : MonoBehaviour
    {
        public bool enable = true;
        public string eventName;

        public bool IsDragging { get; private set; }

        public void StartDrag(Sprite sprite)
        {
            if (enable)
            {
                IsDragging = true;
                DragManager.Instance.StartDrag(sprite, eventName);
                Hide();
            }
        }

        public void EndDrag()
        {
            if (IsDragging)
            {
                IsDragging = false;
                DragManager.Instance.EndDrag(eventName);
                Show();
            }
        }

        protected virtual void Hide()
        {

        }

        protected virtual void Show()
        {

        }
    }
}