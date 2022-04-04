using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Innocene/PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        public string lastAnimator = "";
    }
}