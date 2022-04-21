using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Innocence
{
    public class NoteManager : MonoBehaviour
    {
        public static NoteManager Instance;

        [SerializeField]
        private GameObject noteDisplay;
        [SerializeField]
        private Image image;

        private Note note;

        private void Awake()
        {
            if (Instance != null)
            {
                Instance = this;
                image = GetComponent<Image>();
            }
        }

        public void SetNoteDisplay(bool state)
        {
            noteDisplay.SetActive(state);
        }

        public void RefreshItems()
        {
            note = GameManager.instance.GetNote;
            image.sprite = note.CurrentPage().sprite;
        }

        public void NextPage()
        {
            image.sprite = note.NextPage().sprite;
        }

        public void PrevPage()
        {
            image.sprite = note.PrevPage().sprite;
        }
    }
}