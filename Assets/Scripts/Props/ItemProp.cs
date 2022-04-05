using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class ItemProp : MonoBehaviour
    {
        public int id;

        [HideInInspector]
        [SerializeField] bool isPlaying = false;
        [HideInInspector]
        [SerializeField] bool ableToClick = false;

        [Header("Hint")]
        [SerializeField] SpriteRenderer hintSR;
        [SerializeField] float distance;
        BoxCollider2D boxCollider2D;

        // private bool neverExited;

        [HideInInspector]
        public ItemContent item;

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
            if (isPlaying)
                return;

            if (ableToClick)
                ClickEvent();
        }

        private void ClickEvent()
        {
            Movement.instance.StopMovingInPos();
            isPlaying = true;

            Debug.Log("Result: " + item.finishedResult);

            switch (item.finishedResult)
            {
                case FinishedResult.ChangeScene:
                    GameManager.instance.ChangeScene(item.targetSceneName);
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
}