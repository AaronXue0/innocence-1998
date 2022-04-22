using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using System.Linq;

namespace Game
{
    public class GameDatas : MonoBehaviour
    {
        [SerializeField] GMScriptable gmData;
        [SerializeField] ObjectData[] items;
        [SerializeField] TimelineData[] timelineClips;
        [SerializeField] ItemInteract[] itemGameObjects;

        public System.Action progressChangedCallback;
        public int progress { get { return gmData.progress; } set { gmData.progress = value; } }
        private int currentProgress;

        private void Awake()
        {
            currentProgress = progress;
        }

        private void Start()
        {
            itemGameObjects = FindObjectsOfType<ItemInteract>();
        }

        private void Update()
        {
            if (currentProgress != progress)
            {
                progressChangedCallback();
            }
        }

        public void OnProgressChanged()
        {
            currentProgress = progress;
        }

        public void SetProgress(int state)
        {
            progress = state;
            Debug.Log("New progress: " + progress);
        }

        public TimelineData GetTimelineDataWithClipID(int id) => timelineClips[id];
        public TimelineAsset GetTimelineAssetWithClipID(int id) => timelineClips[id].asset;

        public ObjectData GetItemWithID(int id)
        {
            if (id < 0)
                return null;
            return items.ToList().Find(x => x.id == id);
        }

        public bool CheckItemClicked(int id)
        {
            ObjectData item = GetItemWithID(id);
            if (item == null)
                return false;

            if (item.states[item.currentState].missionFinished)
            {
                return true;
            }
            return false;
        }

        public void SetAllItemStates()
        {
            foreach (ItemInteract itemInteract in itemGameObjects)
            {
                ObjectData d = items.ToList().Find(x => x.id == itemInteract.id);
                SetItemState(itemInteract.id, d.currentState);
            }
        }

        public void SetItemState(int id, int state)
        {
            ObjectData item = GetItemWithID(id);
            Debug.Log(id + ", " + item);
            item.currentState = state;

            ObjectContainer objectContainer = item.states[state];

            ItemInteract itemInteract = itemGameObjects.ToList().Find(x => x.id == id);
            itemInteract.result = objectContainer.finishedResult;
            itemInteract.nestItem = objectContainer.nestItems.ToList();

            GameObject go = itemGameObjects.ToList().Find(x => x.id == id).gameObject;
            if (objectContainer.isActive)
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
                return;
            }

            if (objectContainer.sprite != null)
            {
                go.GetComponent<SpriteRenderer>().sprite = objectContainer.sprite;
            }
        }

        public void ResetData()
        {
            foreach (ObjectData item in items)
            {
                item.currentState = 0;
                foreach (ObjectContainer oc in item.states)
                {
                    oc.currentDialogueIndex = 0;
                    oc.missionFinished = false;
                }
            }

            SetAllItemStates();

            gmData.progress = 0;
            progressChangedCallback();

            Debug.Log("Datas reset!");
        }
    }
}
