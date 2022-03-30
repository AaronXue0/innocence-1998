using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "GameDatas", menuName = "Innocene/GameDatas", order = -1)]
    public class GameDatas : ScriptableObject
    {
        public int chapter;
        public int progress;
    }
}