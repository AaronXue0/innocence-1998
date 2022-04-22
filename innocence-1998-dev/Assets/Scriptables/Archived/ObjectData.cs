using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DialogueItem", menuName = "Archived/Data", order = 0)]
    public class ObjectData : ScriptableObject
    {
        public int id;
        public int currentState;
        public ObjectContainer[] states;
    }
    [System.Serializable]
    public class ObjectContainer
    {
        #region FixedDatas
        public bool playOnce;
        public Sprite sprite;
        public bool isActive = true;
        public DialogueItem dialogues;
        public int targetTimelineID;
        public FinishedResult finishedResult;
        public int[] nestItems;
        #endregion

        #region DynamicDatas
        public int currentDialogueIndex;
        public bool missionFinished;
        #endregion

    }
    public enum FinishedResult { None, CheckForTimeline, GetItem }
}