using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Game
{
    /// <summary>
    /// 1. Gamemanger progress setted up on timeline finished/other events
    /// 2. Gameobject States setup in diffrent progress
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        GameDatas gameDatas;
        TextPlayer textPlayer;
        TimelinePlayer timelinePlayer;

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                gameDatas = GetComponent<GameDatas>();
                gameDatas.progressChangedCallback = OnProgressChanged;

                textPlayer = GetComponent<TextPlayer>();
                timelinePlayer = GetComponent<TimelinePlayer>();
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            OnProgressChanged();
        }

        public bool isTestMode = false;
        private void Update()
        {
            if (isTestMode)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    gameDatas.ResetData();
                }
            }
        }

        #region Listener
        public void OnProgressChanged()
        {
            int id = gameDatas.progress;
            TimelineData timelineData = gameDatas.GetTimelineDataWithClipID(id);
            TimelineAsset asset = gameDatas.GetTimelineAssetWithClipID(id);

            if (timelineData.condition == TimelineCondition.Awake)
            {
                timelinePlayer.PlayWithAsset(asset);
            }
            else
            {
                timelinePlayer.SetupAsset(asset);
            }
        }
        #endregion

        #region Progress
        public void SetProgress(int state) => gameDatas.SetProgress(state);
        #endregion

        #region Item
        public void SetAllItemStates() => gameDatas.SetAllItemStates();
        public void SetItemState(int id, int state) => gameDatas.SetItemState(id, state);
        public ObjectData GetItemWithID(int id) => gameDatas.GetItemWithID(id);
        public bool CheckItemClicked(int id) => gameDatas.CheckItemClicked(id);
        public TimelineAsset GetTimelineAssetWithID(int id) => gameDatas.GetTimelineAssetWithClipID(id);
        #endregion

        #region Timeline
        public void PlayNewAssetWithClipID(int id) => timelinePlayer.PlayWithAsset(GetTimelineAssetWithID(id));
        public void SetupDirector(PlayableDirector director) => timelinePlayer.SetupDirector(director);
        public void SetupAsset(TimelineAsset asset) => timelinePlayer.SetupAsset(asset);
        public void PauseTimeline() => timelinePlayer.PauseTimeline();
        public void ResumeeTimeline() => timelinePlayer.ResumeTimeline();
        public void PauseTimeline(PlayableDirector director) => timelinePlayer.PauseTimeline(director);
        public void ResumeeTimeline(PlayableDirector director) => timelinePlayer.ResumeTimeline(director);
        #endregion

        #region Dialogue
        public void HideText() => textPlayer.HideText();
        public void PlayText(DialogueItem item) => textPlayer.Play(item);
        public void TimelinePlay(DialogueItem item, float duration, bool isAutoPlaying) => textPlayer.TimelinePlay(item, duration, isAutoPlaying);
        #endregion
    }
}