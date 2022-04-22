using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomInput
{
    enum Direction { Vertical, Horizontal };

    [RequireComponent(typeof(BoxCollider2D))]
    public class LoopScrollView : MonoBehaviour
    {
        public bool Enable { get; set; }
        public System.Action PasswordChangedAction { get; set; }

        [SerializeField]
        private bool autoSetCollider = true;

        [Space(10)]
        [SerializeField]
        private float sensitivity = 10;
        [SerializeField]
        private Direction scrollDirection;
        [SerializeField]
        private bool mirror;

        [Space(10)]
        [Tooltip("Loop sprite list")]
        [SerializeField]
        private List<Sprite> spriteList;

        [Header("Transform")]
        [SerializeField]
        private Transform preTransform;
        [SerializeField]
        private Transform middleTransform;
        [SerializeField]
        private Transform nextTransform;

        [Header("SpriteRenderer")]
        [SerializeField]
        private SpriteRenderer preSpriteRenderer;
        [SerializeField]
        private SpriteRenderer middleSpriteRenderer;
        [SerializeField]
        private SpriteRenderer nextSpriteRenderer;

        //Custom
        private Vector2 MousePointRatio { get { return InputManager.Instance.GetMousePositionRatio(); } }
        private bool IsTouchedUI { get { return InputManager.Instance.IsPointerOverGameObject(); } }
        //

        private int CurrentIndex { get; set; }
        private int PreIndex
        {
            get
            {
                if (!mirror) return ((CurrentIndex == 0) ? spriteList.Count - 1 : CurrentIndex - 1);
                else return ((CurrentIndex + 1 == spriteList.Count) ? 0 : CurrentIndex + 1);
            }
        }
        private int NextIndex
        {
            get
            {
                if (!mirror) return ((CurrentIndex + 1 == spriteList.Count) ? 0 : CurrentIndex + 1);
                else return ((CurrentIndex == 0) ? spriteList.Count - 1 : CurrentIndex - 1);
            }
        }

        private Vector2 spriteSize;
        private bool isScrolling = false;
        private float preRatio;
        private float ratioDelta;

        private BoxCollider2D boxCollider;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Enable = true;
            SetSpriteSize();
            ResetPosition();

            if (autoSetCollider) SetCollider();
        }

        private void SetSpriteSize()
        {
            if (spriteList != null && spriteList.Count != 0)
                spriteSize = spriteList[0].rect.size / spriteList[0].pixelsPerUnit;
        }

        private void SetCollider()
        {
            if (!boxCollider) boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.offset = new Vector2(0, 0);
            boxCollider.size = spriteSize;
        }

        private void Update()
        {
            if (isScrolling) Scroll();
        }

        private void OnMouseDown()
        {
            if (Enable)
            {
                StopAllCoroutines();
                StartCoroutine(OnMouseDownHandle());
            }
        }

        IEnumerator OnMouseDownHandle()
        {
            yield return new WaitForEndOfFrame();

            if (!IsTouchedUI)
            {
                preRatio = (scrollDirection == Direction.Horizontal) ? MousePointRatio.x : MousePointRatio.y;
                isScrolling = true;
            }
        }

        private void OnMouseUp()
        {
            isScrolling = false;
            ResetPosition();
            PasswordChangedAction();
        }

        private void Scroll()
        {
            if (scrollDirection == Direction.Horizontal)
            {
                ratioDelta = MousePointRatio.x - preRatio;

                middleTransform.localPosition += new Vector3(sensitivity * ratioDelta, 0, 0);
                IndexUpdateDetect();

                preTransform.localPosition = middleTransform.localPosition - new Vector3(spriteSize.x, 0, 0);
                nextTransform.localPosition = middleTransform.localPosition + new Vector3(spriteSize.x, 0, 0);

                preRatio = MousePointRatio.x;
            }
            else
            {
                ratioDelta = MousePointRatio.y - preRatio;

                middleTransform.localPosition += new Vector3(0, sensitivity * ratioDelta, 0);
                IndexUpdateDetect();

                preTransform.localPosition = middleTransform.localPosition + new Vector3(0, spriteSize.y, 0);
                nextTransform.localPosition = middleTransform.localPosition - new Vector3(0, spriteSize.y, 0);

                preRatio = MousePointRatio.y;
            }
        }

        private void IndexUpdateDetect()
        {
            if (scrollDirection == Direction.Horizontal)
            {
                if (middleTransform.localPosition.x >= spriteSize.x / 2f) //Right
                    MoveToPrevious();
                else if (middleTransform.localPosition.x <= -spriteSize.x / 2f) //Left
                    MoveToNext();
            }
            else
            {
                if (middleTransform.localPosition.y >= spriteSize.y / 2f) //Up
                    MoveToNext();
                else if (middleTransform.localPosition.y <= -spriteSize.y / 2f) //Down
                    MoveToPrevious();
            }
        }

        private void MoveToNext()
        {
            Transform tempTransform;
            tempTransform = preTransform;
            preTransform = middleTransform;
            middleTransform = nextTransform;
            nextTransform = tempTransform;

            SpriteRenderer tempSpriteRenderer;
            tempSpriteRenderer = preSpriteRenderer;
            preSpriteRenderer = middleSpriteRenderer;
            middleSpriteRenderer = nextSpriteRenderer;
            nextSpriteRenderer = tempSpriteRenderer;

            if (mirror) IndexDecrease();
            else IndexIncrease();
        }

        private void MoveToPrevious()
        {
            Transform tempTransform;
            tempTransform = nextTransform;
            nextTransform = middleTransform;
            middleTransform = preTransform;
            preTransform = tempTransform;

            SpriteRenderer tempSpriteRenderer;
            tempSpriteRenderer = nextSpriteRenderer;
            nextSpriteRenderer = middleSpriteRenderer;
            middleSpriteRenderer = preSpriteRenderer;
            preSpriteRenderer = tempSpriteRenderer;

            if (mirror) IndexIncrease();
            else IndexDecrease();
        }

        private void IndexIncrease()
        {
            CurrentIndex += 1;
            if (CurrentIndex == spriteList.Count) CurrentIndex = 0;

            SetCurrentIndex(CurrentIndex);
        }

        private void IndexDecrease()
        {
            CurrentIndex -= 1;
            if (CurrentIndex == -1) CurrentIndex = spriteList.Count - 1;

            SetCurrentIndex(CurrentIndex);
        }

        private void ResetPosition()
        {
            if (scrollDirection == Direction.Horizontal)
            {
                preTransform.localPosition = new Vector3(-spriteSize.x, 0, 0);
                middleTransform.localPosition = new Vector3(0, 0, 0);
                nextTransform.localPosition = new Vector3(spriteSize.x, 0, 0);
            }
            else
            {
                preTransform.localPosition = new Vector3(0, spriteSize.y, 0);
                middleTransform.localPosition = new Vector3(0, 0, 0);
                nextTransform.localPosition = new Vector3(0, -spriteSize.y, 0);
            }
        }

        //API
        public void SetSpriteList(List<Sprite> sprites)
        {
            spriteList = sprites;

            SetSpriteSize();
            ResetPosition();
            SetCurrentIndex(CurrentIndex);
        }

        public void SetCurrentIndex(int middleIndex)
        {
            CurrentIndex = middleIndex;

            preSpriteRenderer.sprite = spriteList[PreIndex];
            middleSpriteRenderer.sprite = spriteList[CurrentIndex];
            nextSpriteRenderer.sprite = spriteList[NextIndex];
        }

        public int GetCurrentIndex()
        {
            return CurrentIndex;
        }
    }
}