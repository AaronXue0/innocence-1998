using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Innocence
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ItemProp : MonoBehaviour
    {
        public int id;
        public bool isGameplayItem = false;
        public bool isObtainedItem = false;
        public SetItemStateContent[] obtainedItemAndSetStates;

        [HideInInspector]
        public ItemContent item;

        [Header("Hint")]
        [SerializeField] SpriteRenderer hintSR;
        public Sprite GetHintSprite { get { return hintSR.sprite; } }
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
                if (Movement.instance != null)
                {
                    float _distance = Vector2.Distance(transform.position, Movement.instance.transform.position);

                    if (_distance < this.distance)
                    {
                        SetClickable(true);
                    }
                    else
                    {
                        SetClickable(false);
                    }
                }
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
            }
        }

        private void OnMouseDown()
        {
            if (isGameplayItem || isPlaying)
                return;

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (ableToClick || isObtainedItem)
            {
                Movement.instance.FaceToTarget(transform);
                ClickEvent();
            }
            else if (Movement.instance != null)
            {
                GameManager.instance.StopAllItemPropCoroutines();
                StartCoroutine(WaitForPlayer());
            }
        }

        private void ClickEvent()
        {
            isPlaying = true;

            if (Movement.instance != null)
            {
                Movement.instance.StopMovingInPos();
            }

            switch (item.finishedResult)
            {
                case FinishedResult.ChangeScene:
                    GameManager.instance.ChangeScene(item.targetSceneName);
                    break;
                case FinishedResult.GetItem:
                    GameManager.instance.ObtainItem(id);
                    foreach (SetItemStateContent o in obtainedItemAndSetStates)
                    {
                        GameManager.instance.SetItemState(o.id, o.newState);
                    }
                    break;
                case FinishedResult.GetNote:
                    GameManager.instance.ObtainNote(id);
                    break;
                case FinishedResult.None:
                case FinishedResult.CheckForTimeline:
                case FinishedResult.SetItemState:
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

        IEnumerator WaitForPlayer()
        {
            float _distance = Vector2.Distance(transform.position, Movement.instance.transform.position);
            while (_distance > this.distance)
            {
                yield return null;
                _distance = Vector2.Distance(transform.position, Movement.instance.transform.position);
            }

            Movement.instance.FaceToTarget(transform);

            ClickEvent();
        }
    }

    [System.Serializable]
    public class SetItemStateContent
    {
        public int id;
        public int newState;
    }
}