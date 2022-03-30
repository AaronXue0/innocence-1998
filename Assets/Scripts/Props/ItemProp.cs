using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    public class ItemProp : MonoBehaviour
    {
        public int id;

        private GameManager gm = GameManager.instance;
        private bool ableToClick = true;
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
            gm.DisplayDialogues(id, DialoguesFinished);
        }

        private void DialoguesFinished()
        {
            isPlaying = false;
            gm.ItemDialoguesFinished(id);
        }
    }
}