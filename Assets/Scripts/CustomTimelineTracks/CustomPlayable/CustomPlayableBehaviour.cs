using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Innocence
{
    [Serializable]
    public class CustomPlayableBehaviour : PlayableBehaviour
    {
        public PlayableAsset asset;
        public bool asDector;

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
                && info.weight > 0f && asDector == false)
            {
                if (GameManager.instance)
                {
                    if (asset != null)
                        GameManager.instance.SubPlay(asset);
                    else
                        GameManager.instance.SubStop();
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
            var duration = playable.GetDuration();
            var count = playable.GetTime() + info.deltaTime;

            if ((count > duration) || playable.GetGraph().GetRootPlayable(0).IsDone())
            {
                // Execute your finishing logic here:
                Debug.Log("Clip done!");
            }

            if (pauseScheduled)
            {
                pauseScheduled = false;

                if (GameManager.instance != null)
                    GameManager.instance.Pause(director);
            }

            clipPlayed = false;
        }
    }

}