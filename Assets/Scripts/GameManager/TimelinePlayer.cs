using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Innocence
{
    public class TimelinePlayer : MonoBehaviour
    {
        PlayableDirector currentDirector;

        public void StopPlaying()
        {
            if (currentDirector != null)
                currentDirector.Stop();
        }

        public void SetMainDirector()
        {
            if (GameObject.FindWithTag("MainDirector").GetComponent<PlayableDirector>() != null)
            {
                currentDirector = GameObject.FindWithTag("MainDirector").GetComponent<PlayableDirector>();
                currentDirector.stopped += OnPlayableDirectorStopped;
            }
            else
            {
                Debug.Log("MainDirector is missing in current scene, \ncheckout if director's tag is attached");
            }
        }

        void OnPlayableDirectorStopped(PlayableDirector aDirector)
        {
            if (currentDirector == aDirector)
            {
                GameManager.instance.StopTimelinePlaying();
                Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
            }
        }

        #region Play
        public void Play()
        {
            if (currentDirector == null)
                SetMainDirector();

            currentDirector.Play();
        }
        public void Play(TimelineAsset timelineAsset)
        {
            if (currentDirector == null)
                SetMainDirector();

            currentDirector.playableAsset = timelineAsset;
            currentDirector.Play();
        }
        #endregion

        #region Pause
        public void Pause()
        {
            if (currentDirector == null)
                SetMainDirector();

            currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        }
        public void Pause(PlayableDirector director)
        {
            if (currentDirector == null)
                SetMainDirector();

            director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }
        #endregion

        #region Resume
        public void Resume()
        {
            if (currentDirector == null)
                SetMainDirector();

            currentDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);
        }
        public void Resume(PlayableDirector director)
        {
            if (currentDirector == null)
                SetMainDirector();

            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
        #endregion
    }
}