using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

namespace Innocence
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        [SerializeField] bool isTestMode = false;
        [SerializeField] float timeSpeed = 1f;

        private GameDataManager gameData;
        private TimelinePlayer timeplyer;
        private TextPlayer textPlayer;
        private SceneTransition sceneTransition;

        private void Awake()
        {
            Time.timeScale = timeSpeed;
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
                sceneTransition = GetComponent<SceneTransition>();
            }
        }
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void Update()
        {
            if (isTestMode && Input.GetKeyDown(KeyCode.R))
            {
                timeplyer.StopPlaying();
                textPlayer.StopTextPlaying();
                gameData.Reset(() => sceneTransition.ChangeScene("01_00 小吃部"));
            }
        }
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        #region Listener
        public void OnProgressChanged(int newProgress)
        {
            Debug.Log("Progress is on: " + newProgress);
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            gameData.SetAllStatesInScene();
        }
        #endregion

        #region Scene
        public void ChangeScene(string name) => sceneTransition.ChangeScene(name);
        #endregion

        #region GameData
        public int Progress { get { return gameData.progress; } }
        public void SetProgress(int progress) => gameData.progress = progress;
        public void SetItemState(int id, int state) => gameData.SetItemState(id, state);
        public GameItem GetGameItem(int id) => gameData.GetGameItem(id);
        public ItemContent GetItemContent(int id) => gameData.GetItemContent(id);
        public Dialogues GetDialogues(int id) => gameData.GetDialogues(id);
        public void ItemDialoguesFinished(int id) => gameData.ItemDialoguesFinished(id);
        public void SetLightState(int id, int state) => gameData.SetLightState(id, state);
        public LightData GetLightData(int id) => gameData.GetLightData(id);
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
        public void StopTimelinePlaying() => textPlayer.StopTimelinePlaying();
        public void DisplayDialogues(int id, System.Action result) => textPlayer.DisplayDialogues(GetDialogues(id), result);
        public void DisplayDialogues(Dialogues dialogues) => textPlayer.DisplayDialogues(dialogues);
        public void DisplayText(string msg) => textPlayer.DisplayText(msg);
        #endregion
    }
}