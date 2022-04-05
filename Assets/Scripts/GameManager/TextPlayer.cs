using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Innocence
{
    public class TextPlayer : MonoBehaviour
    {
        [SerializeField] TMP_Text display;
        [SerializeField] GameObject raycastIsolation;

        public bool IsPlaying { get; set; }
        public bool IsTimelinePlaying { get; set; }

        Dialogues CurrentDialogues { get; set; }
        bool ForceToDone { get; set; }
        bool WaitForContinue { get; set; }
        bool IsFading { get; set; }
        bool IsAppearing { get; set; }
        Action Pause, Resume, Result;

        public void Init(Action Pause, Action Resume)
        {
            ForceToDone = false;
            IsPlaying = false;
            IsAppearing = false;
            IsFading = false;
            WaitForContinue = false;
            IsTimelinePlaying = false;

            this.Pause = Pause;
            this.Resume = Resume;
        }

        // Update is called once per frame
        private void Update()
        {
            if (WaitForContinue)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    WaitForContinue = false;
                }
            }
        }

        #region APIs
        public void SetTimelinePlaying() => IsTimelinePlaying = true;
        public void StopTimelinePlaying() => IsTimelinePlaying = false;
        public void StopTextPlaying()
        {
            StopAllCoroutines();
            DisplayText("");
            EnablePlayerInteractWithScene();
            CurrentDialogues = null;
            IsPlaying = false;
            IsTimelinePlaying = false;
        }
        public void DisplayText(string msg) => display.text = msg;
        public void DisplayDialogues(Dialogues dialogues, Action result)
        {
            this.Result = result;
            DisplayDialogues(dialogues);
        }
        public void DisplayDialogues(Dialogues dialogues)
        {
            if (CurrentDialogues)
            {
                if (IsAppearing)
                {
                    IsAppearing = false;
                    ForceToDone = true;
                }
                else if (WaitForContinue)
                {
                    WaitForContinue = false;
                }
            }
            else
            {
                StopAllCoroutines();
                CurrentDialogues = dialogues;
                StartCoroutine(PlayTextCorotuine(dialogues));
            }
        }
        #endregion

        #region Private
        private void DisablePlayerInteractWithScene() => raycastIsolation.SetActive(true);
        private void EnablePlayerInteractWithScene() => raycastIsolation.SetActive(false);
        #endregion

        private IEnumerator PlayTextCorotuine(Dialogues dialogues)
        {
            if (IsTimelinePlaying == false)
                Movement.isLocked = true;

            if (dialogues != null)
            {
                IsPlaying = true;
                DisablePlayerInteractWithScene();

                string[] msg = dialogues.GetMsg;
                float appearGap = dialogues.GetAppearGap;
                float displaySec = dialogues.GetDisplaySec;
                float fadeoutDuration = dialogues.GetFadeoutDuration;

                yield return StartCoroutine(DialoguingCoroutine(msg, appearGap));
                yield return new WaitForSeconds(displaySec);
                yield return StartCoroutine(FadeoutCoroutine(fadeoutDuration));

            }

            if (Result != null)
            {
                Debug.Log(Result);
                Result();
            }

            if (IsTimelinePlaying == false)
            {
                Movement.isLocked = false;
            }
            CurrentDialogues = null;
            IsPlaying = false;
            IsTimelinePlaying = false;
            EnablePlayerInteractWithScene();
        }
        private IEnumerator DialoguingCoroutine(string[] msg, float appearGap)
        {
            foreach (string str in msg)
            {
                display.text = "";
                display.color = new Color(1, 1, 1, 1);
                if (appearGap < 0)
                {
                    DisplayText(str);
                }
                else
                {
                    yield return StartCoroutine(TextAppearCoroutine(str, appearGap));
                }

                WaitForContinue = true;
                while (WaitForContinue)
                {
                    if (IsTimelinePlaying)
                    {
                        Pause();
                    }

                    yield return null;
                }

                if (IsTimelinePlaying)
                    Resume();
            }
        }
        private IEnumerator TextAppearCoroutine(string msg, float appearGap)
        {
            IsAppearing = true;
            foreach (char c in msg)
            {
                if (ForceToDone)
                {
                    DisplayText(msg);
                    ForceToDone = false;
                    break;
                }

                display.text += c;
                yield return new WaitForSeconds(appearGap);
            }
            IsAppearing = false;
        }
        private IEnumerator FadeoutCoroutine(float fadeoutDuration)
        {
            IsFading = true;
            float elapsedTime = 0f;
            while (elapsedTime <= fadeoutDuration)
            {
                elapsedTime += Time.deltaTime;
                display.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), elapsedTime / fadeoutDuration);
                yield return null;
            }
            IsFading = false;
        }
    }
}