using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Innocence;

namespace Game
{
    enum TextState { Dialogue, Quote }
    public class TextPlayer : MonoBehaviour
    {
        [SerializeField] TMP_Text broadcast;

        DialogueItem CurrentItem { get; set; }
        bool ForceToDone { get; set; }
        bool WaitForContinue { get; set; }
        bool IsPlaying { get; set; }
        bool IsFading { get; set; }
        bool IsAppearing { get; set; }
        bool AutoPlay { get; set; }
        bool IsTimelinePlaying { get; set; }


        private float autoPlayDuration = 0;

        private void Awake()
        {
            ForceToDone = false;
            IsPlaying = false;
            IsAppearing = false;
            IsFading = false;
            WaitForContinue = false;
            IsTimelinePlaying = false;

            if (broadcast == null)
            {
                broadcast = GetComponentInChildren<TMP_Text>();
            }
            broadcast.text = "";
        }

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

        public void DisplayMsg(string msg)
        {
            broadcast.text = msg;
        }

        public void HideText()
        {
            broadcast.text = "";
            broadcast.color = new Color(1, 1, 1, 1);
        }

        #region GamePlay
        public void TimelinePlay(DialogueItem item, float duration, bool isAutoPlaying)
        {
            IsTimelinePlaying = true;
            AutoPlay = isAutoPlaying;
            CurrentItem = item;
            autoPlayDuration = duration;
            StartCoroutine(PlayCoroutine(CurrentItem));
        }
        public void Play(DialogueItem item)
        {
            if (CurrentItem)
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
                CurrentItem = item;
                IsTimelinePlaying = false;
                StartCoroutine(PlayCoroutine(CurrentItem));
            }
        }
        IEnumerator PlayCoroutine(DialogueItem item)
        {
            Movement.isLocked = true;
            IsPlaying = true;
            string[] msg = item.GetMsg;
            float appearGap = item.GetAppearGap;
            float displaySec = item.GetDisplaySec;
            float fadeoutDuration = item.GetFadeoutDuration;
            Action callback = item.DoneCallback;

            yield return StartCoroutine(DialogueCoroutine(msg, appearGap));

            yield return new WaitForSeconds(displaySec);

            yield return StartCoroutine(FadeoutCoroutine(fadeoutDuration));

            CurrentItem = null;
            IsPlaying = false;

            if (item.DoneCallback != null)
                item.DoneCallback();

            AutoPlay = false;
            IsTimelinePlaying = false;
            Movement.isLocked = false;
        }
        #endregion

        IEnumerator DialogueCoroutine(string[] msg, float appearGap)
        {
            foreach (string str in msg)
            {
                broadcast.text = "";
                broadcast.color = new Color(1, 1, 1, 1);
                if (appearGap < 0)
                {
                    DisplayMsg(str);
                }
                else
                {
                    yield return StartCoroutine(TextAppearCoroutine(str, appearGap));
                }

                WaitForContinue = true;
                while (WaitForContinue)
                {
                    if (IsTimelinePlaying)
                        GameManager.instance.PauseTimeline();
                    if (AutoPlay)
                    {
                        yield return new WaitForSeconds(autoPlayDuration);
                        break;
                    }
                    yield return null;
                }
                if (IsTimelinePlaying)
                    GameManager.instance.ResumeeTimeline();
            }
        }
        IEnumerator TextAppearCoroutine(string msg, float appearGap)
        {
            IsAppearing = true;
            foreach (char c in msg)
            {
                if (ForceToDone)
                {
                    DisplayMsg(msg);
                    ForceToDone = false;
                    break;
                }

                broadcast.text += c;
                yield return new WaitForSeconds(appearGap);
            }
            IsAppearing = false;
        }
        IEnumerator FadeoutCoroutine(float fadeoutDuration)
        {
            IsFading = true;
            float elapsedTime = 0f;
            while (elapsedTime <= fadeoutDuration)
            {
                elapsedTime += Time.deltaTime;
                broadcast.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), elapsedTime / fadeoutDuration);
                yield return null;
            }
            IsFading = false;
        }
    }
}