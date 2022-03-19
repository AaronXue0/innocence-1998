using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    enum StateSelector
    {
        FoceToDone, Lock, ShowDirectly,
    }
    public class DialougeInteract : MonoBehaviour
    {
        [SerializeField] string msg;
        [SerializeField] float duration, displaySecs, fadeoutDuration;
        [SerializeField] StateSelector toDoAfterAnimationDone;
        bool isLocked = false;
        bool isPlaying = false;

        public void PlayText(string msg, float duration, float displaySecs, float fadeoutDuration) => GameManager.instance.DoText(msg, duration, displaySecs, fadeoutDuration);
        public void PlayText(string msg, float duration, float displaySecs, float fadeoutDuration, System.Action callback) => GameManager.instance.DoText(msg, duration, displaySecs, fadeoutDuration, callback);

        public void SetLocked() => isLocked = true;
        public void AnimationDone() => isPlaying = false;


        private void OnMouseDown()
        {
            if (isLocked)
            {
                return;
            }

            if (toDoAfterAnimationDone == StateSelector.FoceToDone && isPlaying)
            {
                return;
            }

            Debug.Log(toDoAfterAnimationDone);

            isPlaying = true;

            switch (toDoAfterAnimationDone)
            {
                case StateSelector.FoceToDone:
                    PlayText(msg, duration, displaySecs, fadeoutDuration, AnimationDone);
                    break;
                case StateSelector.Lock:
                    PlayText(msg, duration, displaySecs, fadeoutDuration, SetLocked);
                    break;
                case StateSelector.ShowDirectly:
                    PlayText(msg, 0, displaySecs, fadeoutDuration, AnimationDone);
                    break;
            }
        }
    }
}