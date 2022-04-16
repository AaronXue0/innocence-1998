using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class ElectShakingHorse : IGameplay
    {
        [SerializeField] GameObject returnBtn;
        [SerializeField] int targetIndex = 3;

        private BoxCollider2D boxCollider2D;

        private int currentIndex = 0;

        private float delayCounter = 0;

        private bool isStartedPlaying;

        #region Override
        public override void GameplaySetup()
        {
            returnBtn.SetActive(false);
        }
        public override void PuzzleSolvedCallback()
        {

        }
        #endregion

        private void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
        }
        private void Start()
        {
            GameplaySetup();
        }
        private void Update()
        {
            if (currentIndex >= targetIndex)
            {
                PuzzleSolved();
            }

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
        }

        IEnumerator TimelineFinishedCoroutine()
        {
            yield return null;
            currentIndex++;
            switch (currentIndex)
            {
                case 1:
                    GameManager.instance.ObtainItem(31, false);
                    break;
                case 2:
                    GameManager.instance.ObtainItem(32, false);
                    break;
            }
            isStartedPlaying = false;
            boxCollider2D.enabled = true;
        }
    }
}