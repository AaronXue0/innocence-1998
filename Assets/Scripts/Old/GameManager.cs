using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Old
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        [SerializeField] TMP_Text broadcast;

        bool isAnimationPlaying = false;

        #region Display text animation
        public void DoText(string msg, Action animationDone)
        {
            if (isAnimationPlaying)
                return;

            ForceToDone(msg);
            animationDone();
        }
        public void DoText(string msg, float duration, float displaySecs, float fadeoutDuration, Action animationDone)
        {
            if (isAnimationPlaying)
            {
                ForceToDone(msg);
                return;
            }

            isAnimationPlaying = true;
            StopAllCoroutines();
            StartCoroutine(DoTextCoroutine(msg, duration, displaySecs, fadeoutDuration, animationDone));
        }
        public void ForceToDone(string msg)
        {
            StopCoroutine("DoTextCoroutine");
            broadcast.text = msg;
            isAnimationPlaying = false;
        }
        #endregion


        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                if (broadcast == null)
                {
                    broadcast = GetComponentInChildren<TMP_Text>();
                    broadcast.text = "";
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // StartCoroutine(DoTextCoroutine("外面風雨很大,我最好先待在室內", 0.1f, 3, 1.5f));
            }
        }

        IEnumerator DoTextCoroutine(string msg, float duration, float displaySecs, float fadeoutDuration, Action animationDone)
        {
            broadcast.text = "";
            broadcast.color = new Color(1, 1, 1, 1);

            foreach (char c in msg)
            {
                broadcast.text += c;
                yield return new WaitForSeconds(duration);
            }

            yield return new WaitForSeconds(displaySecs);

            float elapsedTime = 0f;
            while (elapsedTime < fadeoutDuration)
            {
                elapsedTime += Time.deltaTime;
                broadcast.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), elapsedTime / fadeoutDuration);
                yield return null;
            }

            animationDone();
            isAnimationPlaying = false;
        }
    }
}