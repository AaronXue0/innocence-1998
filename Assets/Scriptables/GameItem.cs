using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "Item", menuName = "Innocene/GameItem", order = 0)]
    public class GameItem : ScriptableObject
    {
        public int id;
        public int currentState;
        public ItemContent[] stateContents;

        public ItemContent GetContent { get { return stateContents[currentState]; } }
    }
    [System.Serializable]
    public class ItemContent
    {
        [Header("GameObject")]
        public Sprite sprite = null;
        public bool isActive = true;
        public bool isClickAble = true;

        [Header("Dialogues")]
        public Dialogues dialogues = null;
        public FinishedResult finishedResult;
        public int[] nestItemsID;
        public int[] awardItemsID;

        [Header("Game Storage")]
        public bool completed = false;
    }
    public enum FinishedResult { None, CheckForTimeline, GetItem }
}