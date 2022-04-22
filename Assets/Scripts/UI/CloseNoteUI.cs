using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Innocence
{
    public class CloseNoteUI : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            NoteManager.Instance.SetNoteDisplay(false);
        }
    }
}