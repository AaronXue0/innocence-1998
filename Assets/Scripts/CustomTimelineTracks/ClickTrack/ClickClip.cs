using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Innocence
{
    [Serializable]
    public class ClickClip : PlayableAsset, ITimelineClipAsset
    {
        public ClipBehaviour template = new ClipBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<ClipBehaviour>.Create(graph, template);

            return playable;
        }
    }

}