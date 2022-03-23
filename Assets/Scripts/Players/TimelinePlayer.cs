using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Game
{
    public class TimelinePlayer : MonoBehaviour
    {
        PlayableDirector currentDirector;

        public void PauseTimeline(PlayableDirector director)
        {
            director.Pause();
        }
    }
}