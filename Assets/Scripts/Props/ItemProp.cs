using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ItemProp : MonoBehaviour
    {
        public int id;
        public bool isGameplayItem = false;
        public bool isObtainedItem = false;
        public ObtainedItemAndSetStates[] obtainedItemAndSetStates;

        [HideInInspector]
        public ItemContent item;

        [Header("Hint")]
        [SerializeField] SpriteRenderer hintSR;
        [SerializeField] float distance;
        BoxCollider2D boxCollider2D;

        private bool isPlaying = false;
        private bool ableToClick = false;

        // private bool neverExited;

        private void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            InvokeRepeating("DetectHint", 0f, 0.1f);
        }

        private void Start()
        {
            item = GameManager.instance.GetItemContent(id);
        }

        public void DetectHint()
        {
            if (GameManager.instance.IsTimelinePlaying || GameManager.instance.IsDialoguePlaying)
            {
                SetClickable(false);
                return;
            }

            if (boxCollider2D.enabled && hintSR != null)
            {
                float _distance = Vector2.Distance(transform.position, Movement.instance.transform.position);

                if (_distance < this.distance)
                    SetClickable(true);
                else
                    SetClickable(false);
            }
            else if (boxCollider2D.enabled == false && hintSR != null)
            {
                SetClickable(false);
            }
        }

        public void SetClickable(bool state)
        {
            ableToClick = state;
            if (hintSR != null)
                hintSR.gameObject.SetActive(state);
        }

        public void SetHintSprite(Sprite sprite)
        {
            if (hintSR != null && sprite != null)
            {
                hintSR.sprite = sprite;
                Debug.Log("Change sprite");
            }
        }

        private void OnMouseDown()
        {
            if (isGameplayItem || isPlaying)
                return;
            Debug.Log("onmousedown on: " + name);

            if (ableToClick || isObtainedItem)
                ClickEvent();
        }

        private void ClickEvent()
        {
            if (Movement.instance != null)
                Movement.instance.StopMovingInPos();

            isPlaying = true;

            Debug.Log("Result: " + item.finishedResult);

            switch (item.finishedResult)
            {
                case FinishedResult.ChangeScene:
                    GameManager.instance.ChangeScene(item.targetSceneName);
                    break;
                case FinishedResult.GetItem:
                    GameManager.instance.ObtainItem(id);
                    foreach (ObtainedItemAndSetStates o in obtainedItemAndSetStates)
                    {
                        GameManager.instance.SetItemState(o.id, o.newStaet);
                    }
                    break;
                case FinishedResult.None:
                case FinishedResult.CheckForTimeline:
                default:
                    GameManager.instance.DisplayDialogues(id, DialoguesFinished);
                    break;
            }
        }

        private void DialoguesFinished()
        {
            isPlaying = false;
            GameManager.instance.ItemDialoguesFinished(id);
        }
    }

    [System.Serializable]
    public class ObtainedItemAndSetStates
    {
        public int id;
        public int newStaet;
    }
}