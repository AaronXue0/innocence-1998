﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class LightSwitch : IGameplay
    {
        [SerializeField] GameObject returnButton;
        [SerializeField] float coldDuration = 0.5f;

        Animator animator;

        private int clickCounts = 0;
        private bool isDelaying = false;

        private void SetDelayFinished() => isDelaying = false;

        #region Override
        public override void PuzzleSolved()
        {
            StartCoroutine(PuzzleSolvedCoroutine(() => returnButton.SetActive(true)));
        }
        public override void GameplaySetup()
        {
            returnButton.SetActive(false);
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