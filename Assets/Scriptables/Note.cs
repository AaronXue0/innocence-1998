using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "Note", menuName = "Innocene/Note", order = 4)]
    public class Note : ScriptableObject
    {
        public int currentIndex = 0;
        [SerializeField] List<NoteItem> inStoreageNoteItems;

        public void Reset()
        {
            inStoreageNoteItems = new List<NoteItem>();
        }
        public void StoreItem(NoteItem item)
        {
            inStoreageNoteItems.Add(item);
        }

        public List<int> GetNotesID()
        {
            List<int> items = new List<int>();
            foreach (NoteItem item in inStoreageNoteItems)
            {
                items.Add(item.id);
            }
            return items;
        }

        public NoteItemContent CurrentPage()
        {
            return inStoreageNoteItems[currentIndex].GetContent;
        }
        public NoteItemContent NextPage()
        {
            if (currentIndex + 1 < inStoreageNoteItems.Count)
            {
                currentIndex++;
            }
            return inStoreageNoteItems[currentIndex].GetContent;
        }
        public NoteItemContent PrevPage()
        {
            if (currentIndex - 1 >= 0)
            {
                currentIndex--;
            }
            return inStoreageNoteItems[currentIndex].GetContent;
        }

    }
}