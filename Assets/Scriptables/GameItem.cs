using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "Item", menuName = "Innocene/GameItem", order = 0)]
    public class GameItem : ScriptableObject
    {
        public int id;
        public int currentState;
        [SerializeField] ItemContent[] stateContainers;

        public ItemContent GetContent { get { return stateContainers[currentState]; } }
    }
    [System.Serializable]
    public class ItemContent
    {
        [Header("GameObject")]
        public Sprite sprite = null;
        public bool isActive = true;

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