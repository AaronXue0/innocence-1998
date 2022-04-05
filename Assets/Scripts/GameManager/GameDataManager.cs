using UnityEngine;
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
        [HideInInspector]
        [SerializeField] LightProp[] lightProps;
        [SerializeField] PlayerData playerData;
        [SerializeField] Vector2[] spawnPoints;

        public System.Action<int> onProgressChanged;
        public int progress { get { return gameDatas.progress; } set { gameDatas.progress = value; } }
        private int currentProgress;

        private void Awake()
        {
            currentProgress = progress;
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
        public void Reset()
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
        }
        public void Reset(System.Action callback)
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

            gameDatas.progress = 0;
            gameDatas.chapter = 0;
            callback();
        }
        public void SetAllStatesInScene()
        {
            itemProps = FindObjectsOfType<ItemProp>();
            if (itemProps != null)
            {
                foreach (ItemProp prop in itemProps)
                {
                    int id = prop.id;

                    ItemContent content = GetItemContent(id);
                    prop.SetHintSprite(content.hintSprite);
                    GameObject go = prop.gameObject;

                    go.SetActive(content.isActive);
                    go.GetComponent<BoxCollider2D>().enabled = content.isClickAble;
                    Debug.Log(id + ", " + prop.name + ", " + go.GetComponent<BoxCollider2D>().enabled);

                    if (content.sprite)
                        go.GetComponent<SpriteRenderer>().sprite = content.sprite;
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
            {
                content.completed = true;
            }
            else
            {
                go.GetComponent<BoxCollider2D>().enabled = content.isClickAble;
            }

            if (content.sprite)
            {
                go.GetComponent<SpriteRenderer>().sprite = content.sprite;
            }
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
                case FinishedResult.GetItem:
                    break;
                case FinishedResult.None:
                default:
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
            TimelineProp.instance.Invoke(progress);
        }
        #endregion
    }
}