using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDrag;

namespace Innocence
{
    public class ElectShakingHorse : IGameplay
    {
        [SerializeField] GameObject returnBtn;

        private BoxCollider2D boxCollider2D;
        private TargetTrigger targetTrigger;

        private int currentIndex = 0;

        private float delayCounter = 0;

        private bool isStartedPlaying;

        #region Override
        public override void GameplaySetup()
        {
            returnBtn.SetActive(false);
            targetTrigger.eventName = "Item17";
        }
        public override void PuzzleSolvedCallback()
        {
            returnBtn.SetActive(true);
        }
        #endregion

        private void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            targetTrigger = GetComponent<TargetTrigger>();
        }
        private void Start()
        {
            if (IsComplete == false)
                GameplaySetup();
            else
                isSolved = true;
        }
        private void Update()
        {
            if (delayCounter < 1f && isStartedPlaying == false)
            {
                delayCounter += Time.deltaTime;
                return;
            }

            if (isStartedPlaying == false)
            {
                if (GameManager.instance.IsTimelinePlaying)
                {
                    boxCollider2D.enabled = false;
                    isStartedPlaying = true;
                }
            }
            else if (isStartedPlaying == true)
            {
                if (GameManager.instance.IsTimelinePlaying == false)
                {
                    StartCoroutine(TimelineFinishedCoroutine());
                }
            }

            if (GameManager.instance.GetGameItem(34).currentState == 2)
            {
                PuzzleSolved();
            }
        }

        IEnumerator TimelineFinishedCoroutine()
        {
            isStartedPlaying = false;
            yield return null;
            currentIndex++;
            switch (currentIndex)
            {
                case 1:
                    targetTrigger.eventName = "Item31";
                    GameManager.instance.ObtainNoneInstanceItem(31, false);
                    boxCollider2D.enabled = true;
                    break;
                case 2:
                    targetTrigger.eventName = "Item32";
                    GameManager.instance.ObtainNoneInstanceItem(32, false);
                    boxCollider2D.enabled = true;
                    break;
                case 3:
                    targetTrigger.eventName = "Null";
                    break;
            }
        }
    }
}