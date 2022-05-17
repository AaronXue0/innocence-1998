using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Innocence
{
    public class LightSwitch : IGameplay
    {
        [SerializeField] GameObject returnButton;
        [SerializeField] BoxCollider2D[] switches;

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
            foreach (BoxCollider2D collider2D in switches)
            {
                collider2D.enabled = false;
            }
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
                foreach (BoxCollider2D collider2D in switches)
                {
                    collider2D.enabled = false;
                }
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
            if (EventSystem.current.IsPointerOverGameObject())
                return;

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