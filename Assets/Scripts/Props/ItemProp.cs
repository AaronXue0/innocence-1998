using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class ItemProp : MonoBehaviour
    {
        public int id;

        private bool isPlaying = false;

        private void OnMouseDown()
        {
            if (isPlaying)
                return;

            ClickEvent();
        }

        private void ClickEvent()
        {
            isPlaying = true;
            GameManager.instance.DisplayDialogues(id, DialoguesFinished);
        }

        private void DialoguesFinished()
        {
            isPlaying = false;
            GameManager.instance.ItemDialoguesFinished(id);
        }
    }
}