using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    [CreateAssetMenu(fileName = "NoteItemList", menuName = "Innocene/NoteItemList", order = 6)]
    public class NoteInfoList : ScriptableObject
    {
        public List<NoteItem> items;

        public List<int> GetInBagNotesID
        {
            get
            {
                Note note = GameManager.instance.GetNote;
                return note.GetNotesID();
            }
        }
        public NoteItem GetNoteWithID(int id)
        {
            return items.Find(x => x.id == id);
        }
    }
}