using UnityEngine;
using System.Collections;
using System.Linq;

namespace Innocence
{
    public class GameDataManager : MonoBehaviour
    {
        [SerializeField] GameDatas gameDatas;
        [SerializeField] GameItem[] gameItems;
        [SerializeField] LightData[] lightDatas;
        [HideInInspector]
        [SerializeField] ItemProp[] itemProps;
        [SerializeField] Bag bag;
        [HideInInspector]
        [SerializeField] LightProp[] lightProps;
        [SerializeField] PlayerData playerData;
        [SerializeField] Vector2[] spawnPoints;

        public System.Action<int> onProgressChanged;
        public int progress { get { return gameDatas.progress; } set { gameDatas.progress = value; } }
        private int currentProgress;

        private BagManager bagManager;

        private void Awake()
        {
            currentProgress = progress;
            bagManager = GetComponentInChildren<BagManager>();
        }

        private void Update()
        {
            if (currentProgress != progress)
            {
                currentProgress = progress;
                onProgressChanged(currentProgress);
            }
        }

        #region Getter
        #region PlayerData
        public PlayerData GetPlayerData() => playerData;
        public Vector2 GetPlayerPos() => spawnPoints[gameDatas.progress];
        #endregion

        #region Item
        public bool IsItemComplete(int id) => GetItemContent(id).completed;
        public ItemProp GetItemProp(int id) => itemProps.ToList().Find(x => x.id == id);
        public GameItem GetGameItem(int id) => gameItems.ToList().Find(x => x.id == id);
        public ItemContent GetItemContent(int id) => gameItems.ToList().Find(x => x.id == id).GetContent;
        public Dialogues GetDialogues(int id) => gameItems.ToList().Find(x => x.id == id).GetContent.dialogues;
        #endregion

        #region Light
        public LightData GetLightData(int id) => lightDatas.ToList().Find(x => x.id == id);
        public LightProp GetLightProp(int id) => lightProps.ToList().Find(x => x.id == id);
        #endregion
        #endregion

        #region APIs
        #region Reset
        public void Reset()
        {
            StartCoroutine(ResetCoroutine());
        }
        public void Reset(System.Action callback)
        {
            StartCoroutine(ResetCoroutine(callback));
        }
        IEnumerator ResetCoroutine(System.Action callback = null)
        {
            playerData.lastAnimator = "";

            foreach (GameItem item in gameItems)
            {
                item.currentState = 0;
                foreach (ItemContent content in item.stateContents)
                {
                    content.completed = false;
                }
            }

            foreach (LightData lightData in lightDatas)
            {
                lightData.currentState = 0;
            }

            gameDatas.progress = 0;
            gameDatas.chapter = 0;

            bag.Reset();

            yield return null;

            if (callback != null)
                callback();
        }
        #endregion

        #region Bag
        public Bag GetBag()
        {
            return bag;
        }
        public bool IsItemInBag(int id)
        {
            return bag.CheckInStorage(id);
        }
        public void ObtainItem(int id)
        {
            GameItem item = GetGameItem(id);
            item.GetContent.completed = true;
            item.AddCurrentState();
            SetItemState(id, item.currentState);
            bag.StoreItem(item);
            bagManager.GetItem(id);
        }
        public void ItemUsage(int id)
        {
            GameItem item = GetGameItem(id);
            bag.RemoveItem(item);
        }
        #endregion 
        public void SetAllStatesInScene()
        {
            itemProps = FindObjectsOfType<ItemProp>();
            if (itemProps != null)
            {
                foreach (ItemProp prop in itemProps)
                {
                    int id = prop.id;
                    Debug.Log(id + ", " + prop.name);
                    int state = GetGameItem(id).currentState;
                    SetItemState(id, state);
                }
            }

            lightProps = FindObjectsOfType<LightProp>();
            if (lightProps != null)
            {
                foreach (LightProp prop in lightProps)
                {
                    int id = prop.id;
                    LightData content = GetLightData(id);
                    prop.LightSwitch(content.GetContent.isActive);
                }
            }
        }
        public void SetItemComplete(int id)
        {
            ItemContent item = GetItemContent(id);
            item.completed = true;
        }
        public void SetItemState(int id, int state)
        {
            GameItem item = GetGameItem(id);
            ItemProp prop = GetItemProp(id);
            GameObject go = null;

            if (prop != null)
                go = prop.gameObject;
            else
            {
                Debug.Log("Itempro, id: " + id + " not found.");
                return;
            }

            item.currentState = state;
            ItemContent content = item.GetContent;
            prop.item = content;
            prop.SetHintSprite(content.hintSprite);

            go.SetActive(content.isActive);
            if (content.isActive == false)
                content.completed = true;
            else
                go.GetComponent<BoxCollider2D>().enabled = content.isClickAble;

            if (content.animtorTriggerName != "")
                go.GetComponent<Animator>().SetTrigger(content.animtorTriggerName);

            if (content.sprite)
                go.GetComponent<SpriteRenderer>().sprite = content.sprite;

            if (content.soundClip)
                go.GetComponent<AudioSource>().clip = content.soundClip;
        }
        public void ItemDialoguesFinished(int id)
        {
            ItemContent content = GetItemContent(id);
            content.completed = true;

            Debug.Log("Item Dialogue Finished, id: " + id);
            switch (content.finishedResult)
            {
                case FinishedResult.CheckForTimeline:
                    CheckCurrentTimelineCondition(content);
                    break;
            }
        }
        public void SetLightState(int id, int state)
        {
            LightData lightData = GetLightData(id);
            LightProp prop = GetLightProp(id);

            lightData.currentState = state;
            prop.LightSwitch(lightData.GetContent.isActive);
        }
        #endregion

        #region Private
        private void CheckCurrentTimelineCondition(ItemContent content)
        {
            foreach (int id in content.nestItemsID)
            {
                if (GetItemContent(id).completed)
                    continue;

                return;
            }
            progress++;
            Debug.Log("ProgressAdded");
        }
        #endregion
    }
}