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
        [SerializeField] GameObject bagObject;
        [SerializeField] GameObject resetPanel;

        [HideInInspector]
        public string currentScene;

        private GameDataManager gameData;
        private TimelinePlayer timeplyer;
        private TextPlayer textPlayer;
        private SceneTransition sceneTransition;
        private AudioPlayer audioPlayer;

        #region Unity APIs
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
                audioPlayer = GetComponent<AudioPlayer>();
            }
        }
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void Update()
        {
            if ((IsDialoguePlaying || IsTimelinePlaying) && bagObject.activeSelf == true)
            {
                bagObject.SetActive(false);
            }
            else if (bagObject.activeSelf == false && (IsDialoguePlaying || IsTimelinePlaying) == false)
            {
                bagObject.SetActive(true);
            }
            if (isTestMode && Input.GetKeyDown(KeyCode.R))
            {
                resetPanel.SetActive(true);
                audioPlayer.bgmSource.clip = null;
                timeplyer.StopPlaying();
                textPlayer.StopTextPlaying();
                gameData.Reset(() => sceneTransition.ChangeScene("01_00 小吃部", () => resetPanel.SetActive(false)));
            }
        }
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        #endregion

        #region Listener
        public void OnTimelineFinished()
        {
            switch (currentScene)
            {
                case "01_00 小吃部":
                    if (Movement.instance)
                        gameData.SavePlayerPos(0, Movement.instance.transform.position);
                    break;
                case "01_50 電話亭過場":
                    if (Movement.instance)
                        gameData.SavePlayerPos(1, Movement.instance.transform.position);
                    break;
                case "02_00 騎樓":
                    if (Movement.instance)
                        gameData.SavePlayerPos(2, Movement.instance.transform.position);
                    break;
            }
        }
        public void OnProgressChanged(int newProgress)
        {
            Debug.Log("Progress changed: " + newProgress);

            if (newProgress == 50)
            {
                sceneTransition.ChangeScene("GameOver");
            }

            audioPlayer.ChangeMusicDectector(newProgress);
            if (TimelineProp.instance != null)
                TimelineProp.instance.Invoke(newProgress);
        }
        private void BeforeSceneChanging(string scene)
        {
            switch (scene)
            {
                case "01_00 小吃部":
                    if (Movement.instance)
                        gameData.SavePlayerPos(0, Movement.instance.transform.position);
                    break;
                case "01_50 電話亭過場":
                    if (Movement.instance)
                        gameData.SavePlayerPos(1, Movement.instance.transform.position);
                    break;
                case "02_00 騎樓":
                    if (Movement.instance)
                        gameData.SavePlayerPos(2, Movement.instance.transform.position);
                    break;
            }
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Scene loaded: " + scene.name);
            currentScene = scene.name;
            audioPlayer.ChangeMusicDectector(scene.name);
            gameData.SetAllStatesInScene();

            BagManager.Instance.OnSceneLoadeed(currentScene);

            switch (currentScene)
            {
                case "MainMenu":
                    audioPlayer.StopPlaying();
                    break;
                // Scene that player ables to move
                case "01_00 小吃部":
                    if (Movement.instance)
                        Movement.instance.SetPosition(gameData.GetPlayerPos(0));
                    break;
                case "01_50 電話亭過場":
                    if (Movement.instance)
                        Movement.instance.SetPosition(gameData.GetPlayerPos(1));
                    break;
                case "02_00 騎樓":
                    if (Movement.instance)
                        Movement.instance.SetPosition(gameData.GetPlayerPos(2));
                    break;
            }
        }
        #endregion

        #region Scene
        public void ChangeScene(string name)
        {
            BeforeSceneChanging(currentScene);
            sceneTransition.ChangeScene(name);
        }
        #endregion

        #region Menu
        public void NewGame()
        {
            gameData.Reset();
            sceneTransition.ChangeScene("01_00 小吃部");
        }
        public void ExitGame()
        {
            Application.Quit();
        }
        #endregion

        #region GameData
        public int Progress { get { return gameData.progress; } }

        public bool IsItemComplete(int id) => gameData.IsItemComplete(id);

        public void SetProgress(int progress) => gameData.progress = progress;
        public void SetItemState(int id, int state) => gameData.SetItemState(id, state);
        public void ItemDialoguesFinished(int id) => gameData.ItemDialoguesFinished(id);
        public void SetLightState(int id, int state) => gameData.SetLightState(id, state);
        public void SetItemComplete(int id) => gameData.SetItemComplete(id);
        public void ObtainItem(int id, bool doseCheckItem = true) => gameData.ObtainItem(id, doseCheckItem);
        public void ObtainNoneInstanceItem(int id, bool doseCheckItem = true) => gameData.ObtainNoneInstanceItem(id, doseCheckItem);
        public void UsaItem(int id) => gameData.ItemUsage(id);

        public Bag GetBag { get { return gameData.GetBag(); } }

        // public Vector2 GetPlayerPos() => gameData.GetPlayerPos();
        public PlayerData GetPlayerData() => gameData.GetPlayerData();
        public GameItem GetGameItem(int id) => gameData.GetGameItem(id);
        public ItemContent GetItemContent(int id) => gameData.GetItemContent(id);
        public Dialogues GetDialogues(int id) => gameData.GetDialogues(id);
        public LightData GetLightData(int id) => gameData.GetLightData(id);
        #endregion

        #region SFX
        public void PlaySFX(AudioClip clip) => audioPlayer.PlaySFX(clip);
        // public void ChangeBGM() => ChangeBGM();
        #endregion

        #region Timeline
        public bool IsTimelinePlaying { get { return timeplyer.IsPlaying; } }

        public void SetMainDirector() => timeplyer.SetMainDirector();
        public void Play() => timeplyer.Play();
        public void Play(TimelineAsset timelineAsset) => timeplyer.Play(timelineAsset);
        public void Pause() => timeplyer.Pause();
        public void Pause(PlayableDirector director) => timeplyer.Pause(director);
        public void Resume() => timeplyer.Resume();
        public void Resume(PlayableDirector director) => timeplyer.Resume(director);
        #endregion

        #region Text
        public bool IsDialoguePlaying { get { return textPlayer.IsPlaying; } }

        public void SetTimelinePlaying() => textPlayer.SetTimelinePlaying();
        public void StopTimelinePlaying() => textPlayer.StopTimelinePlaying();
        public void DisplayDialogues(int id, System.Action result) => textPlayer.DisplayDialogues(GetDialogues(id), result);
        public void DisplayDialogues(Dialogues dialogues) => textPlayer.DisplayDialogues(dialogues);
        public void DisplayText(string msg) => textPlayer.DisplayText(msg);
        #endregion
    }
}