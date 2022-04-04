using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class ItemProp : MonoBehaviour
    {
        public int id;

        [SerializeField] bool isPlaying = false;
        [SerializeField] bool ableToClick = false;

        [Header("Hint")]
        [SerializeField] SpriteRenderer hintSR;
        [SerializeField] float distance;
        BoxCollider2D boxCollider2D;

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
            if (boxCollider2D.enabled && hintSR != null)
            {
                if (Vector2.Distance(transform.position, Movement.instance.transform.position) < distance)
                {
                    hintSR.gameObject.SetActive(true);
                    ableToClick = true;
                }
                else
                {
                    ableToClick = false;
                    hintSR.gameObject.SetActive(false);
                }
            }
            else if (boxCollider2D.enabled == false && hintSR != null)
            {
                ableToClick = false;
                hintSR.gameObject.SetActive(false);
            }
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