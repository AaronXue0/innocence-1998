using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class LightSwitch : IGameplay
    {
        [SerializeField] GameObject returnButton;

        Animator animator;

        private int clickCounts = 0;
        private bool isDelaying = false;

        private void SetDelayFinished() => isDelaying = false;

        #region Override
        public override void GameplaySetup()
        {
            returnButton.SetActive(false);
        }
        public override void PuzzleSolvedCallback()
        {
            returnButton.SetActive(true);
        }
        #endregion

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            if (IsComplete == false)
            {
                GameplaySetup();
            }
            else
            {
                isSolved = true;
            }
        }
        private void Update()
        {
            if (clickCounts >= 2 && isSolved == false)
            {
                isSolved = true;
                PuzzleSolved();
            }
        }
        private void OnMouseDown()
        {
            if (isSolved)
                return;

            if (clickCounts < 2 && isDelaying == false)
            {
                isDelaying = true;
                animator.SetTrigger("switch");
                clickCounts++;
                Invoke("SetDelayFinished", coldDuration);
            }
        }
    }
}