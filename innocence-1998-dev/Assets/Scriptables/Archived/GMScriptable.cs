using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GMData", menuName = "Archived/GMData", order = 1)]
    public class GMScriptable : ScriptableObject
    {
        public int progress;
    }
}