using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Game
{
    [Serializable]
    public class DataClip : PlayableAsset, ITimelineClipAsset
    {
        public DataClipBehaviour template = new DataClipBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<DataClipBehaviour>.Create(graph, template);

            return playable;
        }
    }

}