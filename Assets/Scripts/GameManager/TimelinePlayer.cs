using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Innocence
{
    public class TimelinePlayer : MonoBehaviour
    {
        PlayableDirector currentDirector;

        public void SetMainDirector()
        {
            PlayableDirector mainDirector = currentDirector = GameObject.FindWithTag("MainDirector").GetComponent<PlayableDirector>();
            if (mainDirector != null)
            {
                currentDirector = mainDirector;
            }
            else
            {
                Debug.Log("MainDirector is missing in current scene, \ncheckout if director's tag is attached");
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