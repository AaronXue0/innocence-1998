using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

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
                textPlayer = GetComponent<TextPlayer>();
                timelinePlayer = GetComponent<TimelinePlayer>();
            }
        }

        public void SetupDirector(PlayableDirector director) => timelinePlayer.SetupDirector(director);
        public void PauseTimeline() => timelinePlayer.PauseTimeline();
        public void ResumeeTimeline() => timelinePlayer.ResumeTimeline();
        public void PauseTimeline(PlayableDirector director) => timelinePlayer.PauseTimeline(director);
        public void ResumeeTimeline(PlayableDirector director) => timelinePlayer.ResumeTimeline(director);


        public void HideText() => textPlayer.HideText();
        public void PlayText(DialogueItem item) => textPlayer.Play(item);
        public void TimelinePlay(DialogueItem item, float duration) => textPlayer.TimelinePlay(item, duration);
    }
}