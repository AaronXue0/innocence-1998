using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomInput
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        [SerializeField]
        private bool multiTouchEnabled = false;

        private Camera mainCamera;
        private Vector2 pointRate, mousePosition;

        private void Awake()
        {
            if (Instance != null)
                return;

            Instance = this;
            Input.multiTouchEnabled = multiTouchEnabled;
        }

        public static bool TouchDown { get { return Input.GetMouseButtonDown(0); } }
        public static bool TouchUp { get { return Input.GetMouseButtonUp(0); } }

        public bool IsPointerOverGameObject()
        {
            if (!EventSystem.current) return false; //多加這行
#if UNITY_EDITOR
            return EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#else
        return EventSystem.current.IsPointerOverGameObject();
#endif
        }
        public Vector2 GetMousePositionRatio()
        {
            return GetMainCamera().ScreenToViewportPoint(Input.mousePosition);
        }

        public Vector2 GetMousePositionInUI(Vector2 canvasSize)
        {
            pointRate = GetMainCamera().ScreenToViewportPoint(Input.mousePosition);

            mousePosition.x = pointRate.x * canvasSize.x - canvasSize.x / 2f;
            mousePosition.y = pointRate.y * canvasSize.y - canvasSize.y / 2f;

            return mousePosition;
        }

        public Camera GetMainCamera()
        {
            if (!mainCamera) mainCamera = Camera.main;
            return mainCamera;
        }
    }
}