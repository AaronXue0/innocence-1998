using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Innocence
{
    [Serializable]
    public class ClipBehaviour : PlayableBehaviour
    {
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
            if (info.weight > 0f)
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.Pause();
                }

                if (Application.isPlaying)
                {
                    if (hasToPause)
                    {
                        pauseScheduled = true;
                    }
                }
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (pauseScheduled)
            {
                pauseScheduled = false;

                if (GameManager.instance != null)
                    GameManager.instance.Pause(director);
            }
        }
    }

}