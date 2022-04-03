using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Innocence
{
    [Serializable]
    public class DataClipBehaviour : PlayableBehaviour
    {
        [SerializeField] int newProgress;
        [SerializeField] DataClipContent[] dataClipContents;
        [SerializeField] LightClipContent[] lightClipContents;

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
                    foreach (DataClipContent clip in dataClipContents)
                    {
                        GameManager.instance.SetItemState(clip.targetData.id, clip.setNewState);
                    }
                    foreach (LightClipContent clip in lightClipContents)
                    {
                        GameManager.instance.SetLightState(clip.lightData.id, clip.setNewState);
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
    public class DataClipContent
    {
        public GameItem targetData;
        public int setNewState = 0;
    }
    [System.Serializable]
    public class LightClipContent
    {
        public LightData lightData;
        public int setNewState = 0;
    }
}