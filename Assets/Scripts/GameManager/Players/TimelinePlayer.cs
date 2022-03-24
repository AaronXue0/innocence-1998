using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Game
{
    public class TimelinePlayer : MonoBehaviour
    {
        PlayableDirector currentDirector;

        public void PlayWithAsset(TimelineAsset asset)
        {
            SetupAsset(asset);
            currentDirector.Play();
        }
        public void SetupDirector(PlayableDirector director) => currentDirector = director;
        public void SetupAsset(TimelineAsset asset) => currentDirector.playableAsset = asset;
        public void ResumeTimeline() => currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);
        public void PauseTimeline() => currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        public void ResumeTimeline(PlayableDirector director)
        {
            currentDirector = director;
            currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);
        }
        public void PauseTimeline(PlayableDirector director)
        {
            currentDirector = director;
            currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        }
    }
}