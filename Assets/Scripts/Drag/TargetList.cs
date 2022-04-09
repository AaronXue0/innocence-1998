using System.Collections.Generic;
using UnityEngine;

namespace CustomDrag
{
    [CreateAssetMenu(fileName = "TargetList", menuName = "Game/Drag/Target List")]
    public class TargetList : ScriptableObject
    {
        public List<Target> targets;

        public Target FindTarget(string eventName)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].eventName == eventName) return targets[i];
            }
            return null;
        }
    }
}