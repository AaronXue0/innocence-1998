using UnityEngine;
using UnityEngine.UI;
using CustomInput;
using Innocence;

namespace CustomDrag
{
    public class DragManager : MonoBehaviour
    {
        public static DragManager Instance { get; private set; }
        public static bool IsDragging { get; private set; }
        public static bool Enable { get; set; }

        [SerializeField]
        private RectTransform dragCanvas;
        [SerializeField]
        private GameObject draggingObject;
        [SerializeField]
        private TargetList targetList;

        private RectTransform draggingObjectRect;
        private Image draggingObjectImage;

        private void Awake()
        {
            Instance = this;
            IsDragging = false;
            Enable = true;

            draggingObject.SetActive(false);
            draggingObjectRect = draggingObject.GetComponent<RectTransform>();
            draggingObjectImage = draggingObject.GetComponent<Image>();
        }

        private void Update()
        {
            if (IsDragging) Dragging();
        }

        public void StartDrag(Sprite sprite)
        {
            if (Enable)
            {
                draggingObjectImage.sprite = sprite;
                draggingObjectImage.SetNativeSize();
                draggingObjectRect.anchoredPosition = InputManager.Instance.GetMousePositionInUI(dragCanvas.sizeDelta);
                draggingObject.SetActive(true);
                IsDragging = true;
            }
        }

        public void EndDrag(string eventName)
        {
            if (IsDragging)
            {
                Target target = targetList.FindTarget(eventName);
                if (target != null && target.IsInsideTarget(draggingObjectRect.anchoredPosition))
                    TriggerEvent(eventName);
                draggingObject.SetActive(false);
                IsDragging = false;
            }
        }

        private void Dragging()
        {
            draggingObjectRect.anchoredPosition = InputManager.Instance.GetMousePositionInUI(dragCanvas.sizeDelta);
        }

        private void TriggerEvent(string eventName)
        {
            Debug.Log("Drag Event: " + eventName);
            string scene = GameManager.instance.currentScene;
            switch (eventName)
            {
                // 置物櫃鑰匙
                case "Item13":
                    if (scene == "???")
                    {
                        Debug.Log("Test event");
                    }
                    break;
                // 電話卡
                case "Item16":
                    if (scene == "01_51 公共電話特寫")
                    {
                        Debug.Log("電話卡");
                        GameManager.instance.SetProgress(18);
                        int itemID = 16;
                        GameManager.instance.UsaItem(itemID);
                        BagManager.Instance.DeleteItem(itemID);
                    }
                    break;
            }
        }
    }
}