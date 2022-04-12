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
        private LayerMask targetLayerMask;

        private Camera mainCamera;
        private RectTransform draggingObjectRect;
        private Image draggingObjectImage;
        private AudioSource audioSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                IsDragging = false;
                Enable = true;

                mainCamera = Camera.main;
                draggingObject.SetActive(false);
                draggingObjectRect = draggingObject.GetComponent<RectTransform>();
                draggingObjectImage = draggingObject.GetComponent<Image>();
                audioSource = GetComponent<AudioSource>();
            }
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
                if (IsMatchTarget(eventName)) TriggerEvent(eventName);

                draggingObject.SetActive(false);
                IsDragging = false;
            }
        }

        private void Dragging()
        {
            draggingObjectRect.anchoredPosition = InputManager.Instance.GetMousePositionInUI(dragCanvas.sizeDelta);
        }

        private bool IsMatchTarget(string eventName)
        {
            Ray ray = GetMainCamera().ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100, targetLayerMask);
            return (hit && hit.collider.GetComponent<TargetTrigger>().eventName == eventName);
        }

        private Camera GetMainCamera()
        {
            if (!mainCamera) mainCamera = Camera.main;
            return mainCamera;
        }

        private void TriggerEvent(string eventName)
        {
            Debug.Log("Drag Event: " + eventName);
            string scene = GameManager.instance.currentScene;
            int itemID = -1;
            switch (eventName)
            {
                // 置物櫃鑰匙
                case "Item13":
                    itemID = 13;
                    GameManager.instance.SetItemState(12, 1);
                    break;
                // 電話卡
                case "Item16":
                    itemID = 16;
                    GameManager.instance.SetProgress(18);
                    break;
            }

            if (itemID != -1)
            {
                ItemInfo info = BagManager.Instance.GetItem(itemID);
                if (info.onUsedSound != null)
                {
                    audioSource.clip = info.onUsedSound;
                    audioSource.Play();
                }

                GameManager.instance.UsaItem(itemID);
                BagManager.Instance.DeleteItem(itemID);
            }
        }
    }
}