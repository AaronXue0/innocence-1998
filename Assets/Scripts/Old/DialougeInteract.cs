using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Old
{
    enum StateSelector
    {
        FoceToDone, Lock, ShowDirectly,
    }
    public class DialougeInteract : MonoBehaviour
    {
        [SerializeField] string[] msg;
        [SerializeField] float duration, displaySecs, fadeoutDuration;
        [SerializeField] StateSelector toDoAfterAnimationDone;
        bool isLocked = false;
        bool isPlaying = false;
        bool ableToClick = true;
        byte msgIndex = 0;

        public void PlayText(string msg, System.Action callback) => GameManager.instance.DoText(msg, callback);
        public void PlayText(string msg, float duration, float displaySecs, float fadeoutDuration, System.Action callback) => GameManager.instance.DoText(msg, duration, displaySecs, fadeoutDuration, callback);

        public void SetLocked() => isLocked = true;
        public void AnimationDone() => isPlaying = false;

        public void SetAbleToClick() => ableToClick = true;

        public void ClickDelay()
        {
            ableToClick = false;
            Invoke("SetAbleToClick", 0.1f);
        }


        private void OnMouseDown()
        {
            if (isLocked || ableToClick == false)
            {
                return;
            }
            // Debug.Log(toDoAfterAnimationDone);

            string msg = this.msg[msgIndex];

            ClickDelay();

            if (msgIndex > msg.Length)
            {
                SetLocked();
                return;
            }

            if (isPlaying)
            {
                PlayText(msg, AnimationDone);
                return;
            }

            isPlaying = true;

            switch (toDoAfterAnimationDone)
            {
                case StateSelector.FoceToDone:
                    PlayText(msg, duration, displaySecs, fadeoutDuration, AnimationDone);
                    break;
                case StateSelector.ShowDirectly:
                    PlayText(msg, 0, displaySecs, fadeoutDuration, AnimationDone);
                    break;
            }

            msgIndex++;
        }
    }
}