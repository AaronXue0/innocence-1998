using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Innocence
{
    public class CloseNoteUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] public Sprite exitSprite;
        public void OnPointerClick(PointerEventData eventData)
        {
            MouseCursor.Instance.OnUIExit();
            NoteManager.Instance.SetNoteDisplay(false);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            MouseCursor.Instance.OnUIEnter(exitSprite);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            MouseCursor.Instance.OnUIExit();
        }
    }
}