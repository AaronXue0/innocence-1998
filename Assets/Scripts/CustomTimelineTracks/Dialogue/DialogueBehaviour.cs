using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Game
{
    [Serializable]
    public class DialogueBehaviour : PlayableBehaviour
    {
        public DialogueItem dialogueItem;
        public float duration;
        public bool isAuatoPlay;

        public bool hasToPause = false;

        private bool clipPlayed = false;
        private bool pauseScheduled = false;
        private PlayableDirector director;

        public override void OnPlayableCreate(Playable playable)
        {
            director = (playable.GetGraph().GetResolver() as PlayableDirector);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (!clipPlayed
                && info.weight > 0f)
            {
                GameManager.instance.SetupDirector(director);
                if (isAuatoPlay)
                    GameManager.instance.TimelinePlay(dialogueItem, duration);
                else
                    GameManager.instance.PlayText(dialogueItem);

                if (Application.isPlaying)
                {
                    if (hasToPause)
                    {
                        pauseScheduled = true;
                    }
                }

                clipPlayed = true;
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (pauseScheduled)
            {
                pauseScheduled = false;
                GameManager.instance.PauseTimeline(director);
            }
            else
            {
                GameManager.instance.HideText();
            }

            clipPlayed = false;
        }
    }

}