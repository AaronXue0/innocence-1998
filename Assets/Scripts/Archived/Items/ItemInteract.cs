using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ItemInteract : MonoBehaviour
    {
        public int id;
        public bool isLocked = false;
        public FinishCondition condition;
        public FinishedResult result;

        /// All of items that required to be clicked
        public List<int> nestItem = new List<int>();

        private bool ableToClick = true;
        private bool isPlaying = false;
        private void AbleToClick() => ableToClick = true;

        public bool CheckAllNestClicked
        {
            get
            {
                foreach (int id in nestItem)
                {
                    if (GameManager.instance.CheckItemClicked(id))
                        continue;

                    return false;
                }
                return true;
            }
        }

        private void OnMouseDown()
        {
            if (isLocked || ableToClick == false)
                return;

            ableToClick = false;
            isPlaying = true;
            ClickedEvent();
        }

        private void Update()
        {
            if (isPlaying && isLocked == false)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ObjectData item = GameManager.instance.GetItemWithID(this.id);
                    DialogueItem dialogueItem = item.states[item.currentState].dialogues;
                    dialogueItem.DoneCallback = ClickedResult;
                    GameManager.instance.PlayText(dialogueItem);
                }
            }
        }

        private void ClickedEvent()
        {
            switch (condition)
            {
                case FinishCondition.PlayAllDialogues:
                    ObjectData item = GameManager.instance.GetItemWithID(this.id);
                    Debug.Log(item);
                    Debug.Log("asjop" + item.id);
                    DialogueItem dialogueItem = item.states[item.currentState].dialogues;
                    dialogueItem.DoneCallback = ClickedResult;
                    GameManager.instance.PlayText(dialogueItem);
                    break;
                default:
                    ClickedResult();
                    break;
            }
        }

        private void ClickedResult()
        {
            switch (result)
            {
                case FinishedResult.CheckForTimeline:
                    ObjectData item = GameManager.instance.GetItemWithID(this.id);
                    item.states[item.currentState].missionFinished = true;
                    Debug.Log(CheckAllNestClicked);
                    if (CheckAllNestClicked)
                    {
                        int clipID = item.states[item.currentState].targetTimelineID;
                        GameManager.instance.PlayNewAssetWithClipID(clipID);
                    }
                    isLocked = true;
                    break;
                case FinishedResult.GetItem:
                    break;
                case FinishedResult.None:
                default:
                    break;
            }
        }
    }

    public enum FinishCondition { PlayAllDialogues, };
}