using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Innocence
{
    [RequireComponent(typeof(PlayableDirector))]
    public class TimelineProp : MonoBehaviour
    {
        public static TimelineProp instance;

        [SerializeField] TimelinePropContent[] timelineContents;

        private GameManager gm = GameManager.instance;
        private PlayableDirector director;

        #region Unity APIS
        private void Awake()
        {
            if (instance != null)
                Destroy(instance);
            else
                instance = this;

            director = GetComponent<PlayableDirector>();
        }
        private void Start()
        {
            if (gm == null)
                gm = GameManager.instance;

            int progress = GameManager.instance.Progress;
            TimelinePropContent content = timelineContents.ToList().Find(x => x.requiredProgress == progress);
            if (content != null)
            {
                Debug.Log(content.requiredProgress + " , " + progress);
                if (content.conition == TimelineCondition.Awake)
                    Invoke(content.asset);
            }
        }

        #endregion

        #region APIs
        public void Invoke(int progress)
        {
            TimelinePropContent content = timelineContents.ToList().Find(x => x.requiredProgress == progress);
            if (content != null)
            {
                director.playableAsset = content.asset;

                gm.SetTimelinePlaying();
                gm.Play();
            }
        }
        public void Invoke(TimelineAsset asset)
        {
            if (asset != null)
            {
                director.playableAsset = asset;

                gm.SetTimelinePlaying();
                gm.Play();
            }
        }
        #endregion
    }

    [System.Serializable]
    public class TimelinePropContent
    {
        public int requiredProgress = 0;
        public TimelineAsset asset = null;
        public TimelineCondition conition = TimelineCondition.Certain;
    }

    public enum TimelineCondition { Awake, Certain }
}