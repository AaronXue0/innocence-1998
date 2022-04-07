using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Innocence
{
    public class PhonePassword : IGameplay
    {
        [SerializeField] List<int> password = new List<int>();
        [SerializeField] TMP_Text phoneDialDisplay;
        [SerializeField] GameObject returnButton;
        [SerializeField] float dialFailedAnimationDuration, dialingAnimationDuration;

        private Animator animator;
        private SoundProp soundProp;
        private List<int> dialPassword = new List<int>();
        private bool ableToClick = true, isPickedUp = false, isInAnimation = false;

        #region Getter/Setter
        public bool InactiveClick { get { return ableToClick == false || isSolved; } }
        #endregion

        #region Override
        public override void GameplaySetup()
        {
            dialPassword = new List<int>();
            ableToClick = true;
        }
        public override void PuzzleSolvedCallback()
        {

        }
        #endregion

        #region Unity
        private void Awake()
        {
            animator = GetComponent<Animator>();
            soundProp = GetComponent<SoundProp>();
            phoneDialDisplay.text = "";
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
            if (InactiveClick || isInColdDuration)
                return;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);
            if (hit && Input.GetMouseButtonDown(0) && isPickedUp)
            {
                if (hit.collider.name == gameObject.name)
                    return;
                ColdDurationFunc();

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
        private void OnMouseDown()
        {
            if (InactiveClick || isInColdDuration)
                return;

            ColdDurationFunc();
            PickPhone();
        }
        #endregion

        public void PickPhone()
        {
            isPickedUp = !isPickedUp;
            switch (isPickedUp)
            {
                case true:
                    animator.SetTrigger("up");
                    break;
                case false:
                    animator.SetTrigger("down");
                    ResetPhone();
                    break;
            }
        }

        public void Dial(int num)
        {
            if (InactiveClick)
                return;

            dialPassword.Add(num);
            soundProp.ChoseAndPlayClip(num);
            phoneDialDisplay.text += num;

            if (dialPassword.Count == 10)
            {
                CheckPassword();
            }
        }

        public void ResetPhone()
        {
            phoneDialDisplay.text = "";
            dialPassword.Clear();
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
            StartCoroutine(DialFailedAnimationCoroutine());
        }

        public void AnimationFinished()
        {
            isInAnimation = false;
        }

        IEnumerator DialFailedAnimationCoroutine()
        {
            isInAnimation = true;
            ableToClick = false;
            returnButton.SetActive(false);

            yield return null;

            animator.SetTrigger("failed");
            isPickedUp = false;

            while (isInAnimation)
            {
                yield return null;
            }

            ResetPhone();
            ableToClick = true;
            returnButton.SetActive(true);
        }
    }
}