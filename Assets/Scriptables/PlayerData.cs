using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Innocene/PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        public string lastAnimator = "";
        public List<SceneSpwanPos> sceneSpwanPos;
        public List<SceneEntrance> sceneEntrances;

        public (Vector2, Vector2) GetPlayerVectors(string toScene, string fromScene)
        {
            SceneEntrance entrance = sceneEntrances.Find(x => x.currentScene == toScene);
            return entrance.GetPlayerVectors(fromScene);
        }
    }
    [Serializable]
    public class SceneSpwanPos
    {
        public Vector2 firstPos;
        public Vector2 currentSpwanPos { get; set; }
    }
    [Serializable]
    public class SceneEntrance
    {
        public string currentScene;
        public List<SceneContent> sceneContents;

        public (Vector2, Vector2) GetPlayerVectors(string name)
        {
            SceneContent sceneContent = sceneContents.Find(x => x.fromWhatScene == name);
            Debug.Log(sceneContent.pos);
            return (sceneContent.pos, sceneContent.localScale);
        }
    }

    [Serializable]
    public class SceneContent
    {
        public Vector2 pos, localScale = new Vector2(1, 1);
        public string fromWhatScene;
    }
}