using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class PhonePassword : IGameplay
    {
        [SerializeField] List<int> password = new List<int>();

        private Animator animator;
        private List<int> dialPassword = new List<int>();
        private bool isPickedUp = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (IsComplete)
                GameplaySetup();
            else
                isSolved = true;
        }

        public override void GameplaySetup()
        {
            dialPassword = new List<int>();
        }

        public override void PuzzleSolved()
        {
            StartCoroutine(PuzzleSolvedCoroutine(null));
        }

        private void OnMouseDown()
        {
            if (isSolved)
                return;

            isPickedUp = !isPickedUp;
            switch (isPickedUp)
            {
                case true:
                    animator.SetTrigger("up");
                    break;
                case false:
                    animator.SetTrigger("down");
                    break;
            }
        }

        public void Dial(int num)
        {
            if (isSolved || isPickedUp == false)
                return;

            dialPassword.Add(num);
            // View Update
            if (dialPassword.Count == 10)
            {
                CheckPassword();
            }
        }

        public void CheckPassword()
        {
            bool isPasswordCorrect = true;
            for (int i = 0; i < password.Count; i++)
            {
                if (dialPassword[i] == password[i])
                {
                    continue;
                }
                isPasswordCorrect = false;
                break;
            }

            if (isPasswordCorrect)
            {
                PuzzleSolved();
            }
            else
            {
                PasswordIncorrect();
            }
        }

        public void PasswordIncorrect()
        {
            //View update
            dialPassword = new List<int>();
        }
    }
}