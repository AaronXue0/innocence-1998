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

        public void PauseTimeline(PlayableDirector director) => timelinePlayer.PauseTimeline(director);


        public void HideText() => textPlayer.HideText();
        public void PlayText(DialogueItem item) => textPlayer.Play(item);
        public void TimelinePlay(DialogueItem item, float duration) => textPlayer.TimelinePlay(item, duration);
    }
}