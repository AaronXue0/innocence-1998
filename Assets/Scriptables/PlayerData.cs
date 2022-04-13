using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Innocene/PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        public string lastAnimator = "";
        public List<SceneSpwanPos> sceneSpwanPos;
    }

    [System.Serializable]
    public class SceneSpwanPos
    {
        public Vector2 firstPos;
        public Vector2 currentSpwanPos { get; set; }
    }
}