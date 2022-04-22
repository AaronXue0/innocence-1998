using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "Note", menuName = "Innocene/Note", order = 4)]
    public class Note : ScriptableObject
    {
        public int currentIndex = 0;
        [SerializeField] List<NoteItem> defaultNotes;
        [SerializeField] List<NoteItem> inStoreageNoteItems;
        public int InStoreCounts { get { return inStoreageNoteItems.Count; } }
        [SerializeField] NoteInfoList noteInfoList;

        public void Reset()
        {
            inStoreageNoteItems = new List<NoteItem>();
            foreach (NoteItem noteItem in defaultNotes)
            {
                inStoreageNoteItems.Add(noteItem);
            }
            currentIndex = 0;
        }
        public void StoreItem(int id)
        {
            NoteItem item = noteInfoList.GetNoteWithID(id);
            inStoreageNoteItems.Add(item);
        }

        public NoteItem GetNote(int id)
        {
            return noteInfoList.GetNoteWithID(id);
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

        public void SelectLastestNote()
        {
            currentIndex = inStoreageNoteItems.Count - 1;
        }

        public NoteItemContent CurrentPage()
        {
            if (inStoreageNoteItems.Count == 0)
                Reset();

            return inStoreageNoteItems[currentIndex].GetContent;
        }
        public NoteItemContent NextPage()
        {
            if (inStoreageNoteItems.Count == 0)
                Reset();

            if (currentIndex + 1 < inStoreageNoteItems.Count)
            {
                currentIndex++;
            }
            return inStoreageNoteItems[currentIndex].GetContent;
        }
        public NoteItemContent PrevPage()
        {
            if (inStoreageNoteItems.Count == 0)
                Reset();

            if (currentIndex - 1 >= 0)
            {
                currentIndex--;
            }
            return inStoreageNoteItems[currentIndex].GetContent;
        }

    }
}