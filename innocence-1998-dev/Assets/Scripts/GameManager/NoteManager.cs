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
        private GameObject noteDisplay, flipRight, flipLeft;
        [SerializeField]
        private Image image;

        private Note note;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            RefreshItems();
        }

        private void Update()
        {
            if (note.currentIndex == note.InStoreCounts - 1 && flipRight.activeSelf == true)
            {
                flipRight.SetActive(false);
                MouseCursor.Instance.OnUIExit();
            }
            else if (note.currentIndex < note.InStoreCounts - 1 && flipRight.activeSelf == false)
            {
                flipRight.SetActive(true);
            }

            if (note.currentIndex == 0 && flipLeft.activeSelf == true)
            {
                flipLeft.SetActive(false);
                MouseCursor.Instance.OnUIExit();
            }
            else if (note.currentIndex > 0 && flipLeft.activeSelf == false)
            {
                flipLeft.SetActive(true);
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