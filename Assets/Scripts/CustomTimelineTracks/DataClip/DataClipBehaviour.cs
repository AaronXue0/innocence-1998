using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Game
{
    [Serializable]
    public class DataClipBehaviour : PlayableBehaviour
    {
        [SerializeField] int newProgress;
        [SerializeField] DataClipContainer[] dataClipContainers;

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
                    foreach (DataClipContainer clip in dataClipContainers)
                    {
                        GameManager.instance.SetItemState(clip.targetData.id, clip.setNewState);
                    }
                    GameManager.instance.SetProgress(newProgress);
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
                // GameManager.instance.PauseTimeline(director);
            }
            else
            {
                // GameManager.instance.HideText();
            }

            clipPlayed = false;
        }
    }

    [System.Serializable]
    public class DataClipContainer
    {
        public ObjectData targetData;
        public int setNewState = 0;
    }
}