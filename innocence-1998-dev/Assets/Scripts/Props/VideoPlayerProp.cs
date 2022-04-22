using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Innocence
{
    public class VideoPlayerProp : MonoBehaviour
    {
        [SerializeField] string sceneName;

        VideoPlayer player;

        private void Awake()
        {
            player = GetComponent<VideoPlayer>();
            player.loopPointReached += OnFinished;
        }

        private void OnFinished(VideoPlayer videoPlayer)
        {
            GameManager.instance.ChangeScene(sceneName);
        }
    }
}