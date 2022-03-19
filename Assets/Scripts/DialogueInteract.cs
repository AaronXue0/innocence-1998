using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DialogueInteract : MonoBehaviour
    {
        [SerializeField] DialogueItem item;

        public bool locked = false;
        bool ableToClick = true;
        public void AbleToClick() => ableToClick = true;

        private void Awake()
        {
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
        void None()
        {
            Debug.Log("None");
        }
        void GetItem()
        {
            Debug.Log("GetItem");
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