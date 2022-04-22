using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "Light", menuName = "Innocene/LightData", order = 0)]
    public class LightData : ScriptableObject
    {
        public int id;
        public int currentState;
        public LightContent[] stateContents;

        public LightContent GetContent { get { return stateContents[currentState]; } }
    }
    [System.Serializable]
    public class LightContent
    {
        public bool isActive;
    }
}