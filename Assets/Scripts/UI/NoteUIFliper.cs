using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Innocence
{
    public class NoteUIFliper : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {
        [SerializeField] Sprite flipSprite;

        public void OnPointerEnter(PointerEventData eventData)
        {
            MouseCursor.Instance.OnUIEnter(flipSprite);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            MouseCursor.Instance.OnUIExit();
        }
        public void OnPointerDown(PointerEventData eventData)
        {

        }
    }
}