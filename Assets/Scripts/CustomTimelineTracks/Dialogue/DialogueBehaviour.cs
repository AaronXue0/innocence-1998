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
                if (GameManager.instance != null)
                {
                    GameManager.instance.SetupDirector(director);
                    GameManager.instance.TimelinePlay(dialogueItem, duration, isAuatoPlay);
                }

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

                if (GameManager.instance != null)
                    GameManager.instance.PauseTimeline(director);
            }
            else
            {
                if (GameManager.instance != null)
                    GameManager.instance.HideText();
            }

            clipPlayed = false;
        }
    }

}