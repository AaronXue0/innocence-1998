using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "Note", menuName = "Innocene/Note", order = 4)]
    public class Note : MonoBehaviour
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

        public NoteItem CurrentPage()
        {
            return inStoreageNoteItems[currentIndex];
        }
        public NoteItem NextPage()
        {
            if (currentIndex + 1 < inStoreageNoteItems.Count)
            {
                currentIndex++;
            }
            return inStoreageNoteItems[currentIndex];
        }
        public NoteItem PrevPage()
        {
            if (currentIndex - 1 >= 0)
            {
                currentIndex--;
            }
            return inStoreageNoteItems[currentIndex];
        }

    }
}