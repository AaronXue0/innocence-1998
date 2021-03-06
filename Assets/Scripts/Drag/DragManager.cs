using System.Collections;
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
        private string currentEvent;

        private Material mat;

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
                mat = draggingObjectImage.material;
            }
        }
        private void Update()
        {
            if (IsDragging) Dragging();
        }

        public void StartDrag(Sprite sprite, string eventName)
        {
            if (Enable)
            {
                draggingObjectImage.sprite = sprite;
                draggingObjectImage.SetNativeSize();
                draggingObjectRect.anchoredPosition = InputManager.Instance.GetMousePositionInUI(dragCanvas.sizeDelta);
                draggingObject.SetActive(true);
                IsDragging = true;
                currentEvent = eventName;
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

            if (Movement.instance != null)
                Movement.isLocked = false;
        }

        private void Dragging()
        {
            draggingObjectRect.anchoredPosition = InputManager.Instance.GetMousePositionInUI(dragCanvas.sizeDelta);

            if (Movement.instance != null)
                Movement.isLocked = true;

            MatchEffect();
        }

        private void MatchEffect()
        {
            string shaderName = "PIXELATE_ON";
            if (IsMatchTarget(currentEvent))
            {
                mat.EnableKeyword(shaderName);
            }
            else
                mat.DisableKeyword(shaderName);
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
                // ???????????????
                case "Item13":
                    itemID = 13;
                    GameManager.instance.SetItemState(12, 2);
                    break;
                // ?????????
                case "Item16":
                    itemID = 16;
                    GameManager.instance.SetProgress(18);
                    break;
                // ???????????? 17, 31, 32
                case "Item17":
                    itemID = 17;
                    GameManager.instance.SetProgress(23);
                    break;
                case "Item31":
                    itemID = 31;
                    GameManager.instance.SetProgress(25);
                    break;
                case "Item32":
                    itemID = 32;
                    GameManager.instance.SetProgress(27);
                    break;
                case "Item25":
                    itemID = 25;
                    GameManager.instance.SetItemState(7, 2);
                    GameManager.instance.SetProgress(30);
                    break;
            }

            if (itemID != -1)
            {
                ItemInfo info = BagManager.Instance.GetItem(itemID);
                if (info.onUsedSound != null)
                {
                    GameManager.instance.PlaySFX(info.onUsedSound);
                }

                GameManager.instance.UsaItem(itemID);
                BagManager.Instance.DeleteItem(itemID);
            }
        }
    }
}