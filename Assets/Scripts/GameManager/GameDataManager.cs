using UnityEngine;
using System.Linq;

namespace Innocence
{
    public class GameDataManager : MonoBehaviour
    {
        [SerializeField] GameDatas gameDatas;
        [SerializeField] GameItem[] gameItems;
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

        public int GetProgress { get { return progress; } }

        public GameItem GetGameItem(int id) => gameItems.ToList().Find(x => x.id == id);
        public ItemContent GetItemContent(int id) => gameItems.ToList().Find(x => x.id == id).GetContent;
        public Dialogues GetDialogues(int id) => gameItems.ToList().Find(x => x.id == id).GetContent.dialogues;

        public void ItemDialoguesFinished(int id)
        {
            ItemContent content = GetItemContent(id);

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

        private void CheckCurrentTimelineCondition(ItemContent content)
        {
            foreach (int id in content.nestItemsID)
            {
                if (GetItemContent(id).completed)
                    continue;

                return;
            }
            progress++;
            TimelineProp.instance.Invoke(progress);
        }
    }
}