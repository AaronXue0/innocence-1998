using UnityEngine;
using UnityEditor;

namespace Innocence
{
    [CustomEditor(typeof(GameDataManager))]
    public class GameDataManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GameDataManager gameDataManager = (GameDataManager)target;

            if (GUILayout.Button("Reset"))
            {
                Debug.Log("Reset");
                gameDataManager.ResetOnEditor();
            }
        }
    }

}