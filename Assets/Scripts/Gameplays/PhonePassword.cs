using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class PhonePassword : IGameplay
    {
        [SerializeField] List<int> password = new List<int>();
        [SerializeField] GameObject returnButton;
        [SerializeField] float dialFailedAnimationDuration, dialingAnimationDuration;

        private Animator animator;
        private List<int> dialPassword = new List<int>();
        private bool isPickedUp = false;
        private bool isInAnimation;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (IsComplete)
                isSolved = true;
            else
                GameplaySetup();
        }

        private void Update()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);
            if (hit && Input.GetMouseButtonDown(0) && isPickedUp)
            {
                if (hit.collider.name == gameObject.name)
                    return;

                int num;
                if (int.TryParse(hit.collider.name, out num))
                {
                    Dial(num);
                }
                else
                {
                    Debug.Log("Not a valid int");
                }
            }
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
            if (isSolved || isInAnimation)
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

            if (dialPassword.Count == 1)
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
            dialPassword.Clear();
            StartCoroutine(DialFailedAnimationCoroutine());
        }

        IEnumerator DialFailedAnimationCoroutine()
        {
            animator.SetTrigger("failed");
            returnButton.SetActive(false);
            yield return new WaitForSeconds(dialFailedAnimationDuration);
            returnButton.SetActive(true);
        }
    }
}