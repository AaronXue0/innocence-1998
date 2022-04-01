using UnityEngine;
using System.Linq;

namespace Innocence
{
    public class GameDataManager : MonoBehaviour
    {
        [SerializeField] GameDatas gameDatas;
        [SerializeField] GameItem[] gameItems;
        [HideInInspector]
        [SerializeField] ItemProp[] itemProps;

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

        public ItemProp GetItemProp(int id) => itemProps.ToList().Find(x => x.id == id);
        public GameItem GetGameItem(int id) => gameItems.ToList().Find(x => x.id == id);
        public ItemContent GetItemContent(int id) => gameItems.ToList().Find(x => x.id == id).GetContent;
        public Dialogues GetDialogues(int id) => gameItems.ToList().Find(x => x.id == id).GetContent.dialogues;

        public void Reset()
        {
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
        }
        public void Reset(System.Action callback)
        {
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

        public void SetAllItemsStateInScene()
        {
            itemProps = FindObjectsOfType<ItemProp>();
            if (itemProps == null)
                return;

            foreach (ItemProp prop in itemProps)
            {
                int id = prop.id;

                ItemContent content = GetItemContent(id);
                GameObject go = prop.gameObject;

                go.SetActive(content.isActive);
                go.GetComponent<BoxCollider2D>().enabled = content.isClickAble;

                if (content.sprite)
                    go.GetComponent<SpriteRenderer>().sprite = content.sprite;
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

            go.SetActive(content.isActive);
            go.GetComponent<BoxCollider2D>().enabled = content.isClickAble;

            if (content.sprite)
            {
                go.GetComponent<SpriteRenderer>().sprite = content.sprite;
            }
        }

        public void ItemDialoguesFinished(int id)
        {
            ItemContent content = GetItemContent(id);
            content.completed = true;
            Debug.Log(content.finishedResult);

            switch (content.finishedResult)
            {
                case FinishedResult.CheckForTimeline:
                    Debug.Log("CheckForTimeline");
                    CheckCurrentTimelineCondition(content);
                    break;
                case FinishedResult.GetItem:
                    break;
                case FinishedResult.None:
                default:
                    break;
            }
        }

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
    }
}