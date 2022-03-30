using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

namespace Innocence
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private GameDataManager gameData;
        private TimelinePlayer timeplyer;
        private TextPlayer textPlayer;

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

                gameData = GetComponent<GameDataManager>();
                gameData.onProgressChanged = OnProgressChanged;
                timeplyer = GetComponent<TimelinePlayer>();
                textPlayer = GetComponent<TextPlayer>();
                textPlayer.Init(Pause, Resume);
            }
        }

        #region Listener
        public void OnProgressChanged(int newProgress)
        {

        }
        #endregion

        #region GameData
        public int GetProgress { get { return gameData.GetProgress; } }
        public GameItem GetGameItem(int id) => gameData.GetGameItem(id);
        public ItemContent GetItemContent(int id) => gameData.GetItemContent(id);
        public Dialogues GetDialogues(int id) => gameData.GetDialogues(id);
        public void ItemDialoguesFinished(int id) => gameData.ItemDialoguesFinished(id);
        #endregion

        #region Timeline
        public void SetMainDirector() => timeplyer.SetMainDirector();
        public void Play() => timeplyer.Play();
        public void Play(TimelineAsset timelineAsset) => timeplyer.Play(timelineAsset);
        public void Pause() => timeplyer.Pause();
        public void Pause(PlayableDirector director) => timeplyer.Pause(director);
        public void Resume() => timeplyer.Resume();
        public void Resume(PlayableDirector director) => timeplyer.Resume(director);
        #endregion

        #region Text
        public void SetTimelinePlaying() => textPlayer.SetTimelinePlaying();
        public void DisplayDialogues(int id, System.Action result) => textPlayer.DisplayDialogues(GetDialogues(id), result);
        public void DisplayDialogues(Dialogues dialogues) => textPlayer.DisplayDialogues(dialogues);
        public void DisplayText(string msg) => textPlayer.DisplayText(msg);
        #endregion
    }
}