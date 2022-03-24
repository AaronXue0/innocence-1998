using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DialogueItem", menuName = "Interact/Data", order = 0)]
    public class ObjectData : ScriptableObject
    {
        public int id;
        public int currentState;
        public ObjectContainer[] states;
    }
    [System.Serializable]
    public class ObjectContainer
    {
        public bool playOnce;
        public Sprite sprite;

        public DialogueItem dialogues;
        public int currentDialogueIndex;

        public int targetTimelineID;
        public bool missionFinished;
    }
}