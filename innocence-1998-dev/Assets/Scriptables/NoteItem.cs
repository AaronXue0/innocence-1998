using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "NoteItem", menuName = "Innocene/NoteItem", order = 5)]
    public class NoteItem : ScriptableObject
    {
        public int id;
        public int currentState;
        public NoteItemContent[] stateContents;
        public AudioClip onGetSound;

        public NoteItemContent GetContent { get { return stateContents[currentState]; } }
        public void AddCurrentState() { currentState++; }
    }

    [System.Serializable]
    public class NoteItemContent
    {
        public Sprite sprite;
    }
}