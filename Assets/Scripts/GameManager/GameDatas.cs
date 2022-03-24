using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;

namespace Game
{
    public class GameDatas : MonoBehaviour
    {
        [SerializeField] ObjectData[] items;
        [SerializeField] TimelineAsset[] timelineClips;

        public TimelineAsset GetTimelineAssetWithClipID(int id) => timelineClips[id];

        public ObjectData GetItemWithID(int id) => items.ToList().Find(x => x.id == id);

        public bool CheckItemClicked(int id)
        {
            ObjectData item = GetItemWithID(id);
            if (item == null)
                return false;

            if (item.states[item.currentState].missionFinished)
            {
                return true;
            }
            return false;
        }
    }
}
