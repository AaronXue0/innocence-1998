using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ItemInteract : MonoBehaviour
    {
        public int id;
        public bool locked = false;
        [SerializeField] ObjectData data;
        private DialogueItem item;

        bool ableToClick = true;
        public void AbleToClick() => ableToClick = true;

        private void Awake()
        {
            int state = data.currentState;
            item = data.items[state].dialogues;

            TextAnimationCallbackSelector mode = item.GetAnimationSelector;
            switch (mode)
            {
                case TextAnimationCallbackSelector.None:
                    item.DoneCallback = None;
                    break;
                case TextAnimationCallbackSelector.GetItem:
                    item.DoneCallback = GetItem;
                    break;
            }
        }
        private void None()
        {
        }
        private void GetItem()
        {
            locked = true;
        }
        private void OnMouseDown()
        {
            if (locked)
                return;
            if (ableToClick == false)
                return;

            DelayClick();
            GameManager.instance.PlayText(item);
        }
        private void DelayClick()
        {
            ableToClick = false;
            Invoke("AbleToClick", 0.1f);
        }
    }
}