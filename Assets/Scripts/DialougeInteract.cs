using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    enum StateSelector
    {
        FoceToDone, Lock
    }
    public class DialougeInteract : MonoBehaviour
    {
        [SerializeField] string msg;
        [SerializeField] float duration, displaySecs, fadeoutDuration;
        [SerializeField] StateSelector toDoAfterAnimationDone;
        bool isLocked = false;
        bool isPlaying = false;

        public void PlayText() => GameManager.instance.DoText(msg, duration, displaySecs, fadeoutDuration);
        public void PlayText(System.Action callback) => GameManager.instance.DoText(msg, duration, displaySecs, fadeoutDuration, callback);

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
                    PlayText(AnimationDone);
                    break;
                case StateSelector.Lock:
                    SetLocked();
                    PlayText();
                    break;
            }
        }
    }
}