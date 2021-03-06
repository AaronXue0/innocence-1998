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
        [SerializeField] Note note;
        [HideInInspector]
        [SerializeField] LightProp[] lightProps;
        [SerializeField] PlayerData playerData;

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

        #region PlayerData
        public PlayerData GetPlayerData() => playerData;
        public Vector2 GetPlayerPos(int index) => playerData.sceneSpwanPos[index].currentSpwanPos;
        public (Vector2, Vector2) GetPlayerVectors(string to, string from) => playerData.GetPlayerVectors(to, from);
        public void SavePlayerPos(int index, Vector2 pos)
        {
            playerData.sceneSpwanPos[index].currentSpwanPos = pos;
        }
        #endregion

        #region Item
        public bool IsItemComplete(int id) => GetItemContent(id).completed;
        public ItemProp GetItemProp(int id) => itemProps.ToList().Find(x => x.id == id);
        public GameItem GetGameItem(int id) => gameItems.ToList().Find(x => x.id == id);
        public ItemContent GetItemContent(int id) => gameItems.ToList().Find(x => x.id == id).GetContent;
        public Dialogues GetDialogues(int id) => gameItems.ToList().Find(x => x.id == id).GetContent.dialogues;
        public void StopAllItemPropCoroutines()
        {
            foreach (ItemProp itemProp in itemProps)
            {
                itemProp.StopAllCoroutines();
            }
        }
        #endregion

        #region Light
        public LightData GetLightData(int id) => lightDatas.ToList().Find(x => x.id == id);
        public LightProp GetLightProp(int id) => lightProps.ToList().Find(x => x.id == id);
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

            foreach (SceneSpwanPos pos in playerData.sceneSpwanPos)
            {
                pos.currentSpwanPos = pos.firstPos;
            }

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

            foreach (SceneSpwanPos sceneSpwanPos in playerData.sceneSpwanPos)
            {
                sceneSpwanPos.currentSpwanPos = sceneSpwanPos.firstPos;
            }

            bag.Reset();
            BagManager.Instance.RefreshItems();

            note.Reset();
            NoteManager.Instance.RefreshItems();

            yield return new WaitForSeconds(1.5f);

            if (callback != null)
                callback();
        }
        public void ResetOnEditor()
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
        }
        #endregion

        #region Note
        public Note GetNote()
        {
            return note;
        }
        public void ObtainNote(int id)
        {
            GameItem item = GetGameItem(id);
            ItemContent itemContent = item.GetContent;

            itemContent.completed = true;
            item.AddCurrentState();
            SetItemState(id, item.currentState);
            item.GetContent.completed = true;

            note.StoreItem(item.id);
            note.SelectLastestNote();

            AudioClip clip = note.GetNote(id).onGetSound;

            if (clip != null)
            {
                GameManager.instance.PlaySFX(clip);
            }

            NoteManager.Instance.RefreshItems();

            BagManager.Instance.CheckItem(9);
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
        public void ObtainItem(int id, bool doseCheckItem = true)
        {
            GameItem item = GetGameItem(id);
            int[] nestItems = item.GetContent.nestItemsID;
            SetItemStateContent[] afterGetAllNestItemsAndSetItemsState = item.GetContent.afterGetAllNestItemsAndSetItemsState;
            int newProgress = item.GetContent.afterGetAllNestItemsSetProgress;

            item.GetContent.completed = true;
            item.AddCurrentState();
            SetItemState(id, item.currentState);
            item.GetContent.completed = true;


            CheckNestItemsObtained(nestItems, afterGetAllNestItemsAndSetItemsState, newProgress);

            bag.StoreItem(item);
            bagManager.ObtainedItem(id, doseCheckItem);
        }
        public void ObtainNoneInstanceItem(int id, bool doseCheckItem = true)
        {
            GameItem item = GetGameItem(id);
            bag.StoreItem(item);
            bagManager.ObtainedItem(id, doseCheckItem);
        }
        public void CheckNestItemsObtained(int[] nestItems, SetItemStateContent[] afterGetAllNestItemsAndSetItemsState, int newProgress)
        {
            bool allItemsGet = true;
            foreach (int id in nestItems)
            {
                ItemContent item = GetGameItem(id).GetContent;
                if (item.completed)
                {
                    continue;
                }

                allItemsGet = false;
                break;
            }

            if (allItemsGet)
            {
                foreach (SetItemStateContent s in afterGetAllNestItemsAndSetItemsState)
                {
                    GetGameItem(s.id).currentState = s.newState;
                }

                if (newProgress > 0)
                    gameDatas.progress = newProgress;
                else
                    gameDatas.progress++;
            }
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

            item.currentState = state;

            if (prop != null)
                go = prop.gameObject;
            else
            {
                Debug.Log("Item prop of id: " + id + " not found.");
                return;
            }

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
                case FinishedResult.SetItemState:
                    foreach (SetItemStateContent sc in content.setItemsState)
                    {
                        SetItemState(sc.id, sc.newState);
                    }
                    break;
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