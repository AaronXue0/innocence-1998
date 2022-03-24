using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Game
{
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
                textPlayer = GetComponent<TextPlayer>();
                timelinePlayer = GetComponent<TimelinePlayer>();
            }
        }

        #region Item
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
        public void TimelinePlay(DialogueItem item, float duration) => textPlayer.TimelinePlay(item, duration);
        #endregion
    }
}