using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DialogueItem", menuName = "Interact/Data", order = 0)]
    public class ObjectData : ScriptableObject
    {
        public int id;
        public int currentState;
        public ObjectContainer[] items;
    }
    [System.Serializable]
    public class ObjectContainer
    {
        public Sprite sprite;
        public DialogueItem dialogues;
    }
}