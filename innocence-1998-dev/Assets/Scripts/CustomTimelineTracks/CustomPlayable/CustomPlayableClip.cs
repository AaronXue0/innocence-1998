using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Innocence
{
    [Serializable]
    public class CustomPlayableClip : PlayableAsset, ITimelineClipAsset
    {
        public CustomPlayableBehaviour template = new CustomPlayableBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<CustomPlayableBehaviour>.Create(graph, template);

            return playable;
        }
    }

}