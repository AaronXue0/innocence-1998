﻿using System.Linq;
using System.Collections;
using UnityEngine;

namespace Innocence
{
    public class AudioPlayer : MonoBehaviour
    {
        public AudioSource bgmSource;
        public AudioContent[] contents;

        public void ChangeMusicDectector(int progress)
        {
            AudioContent content = contents.ToList().Find(x => x.requiredProgress == progress);
            if (content != null)
            {
                Debug.Log("Change audio by progress");
                ChangeBGM(content.clip);
            }
        }
        public void ChangeMusicDectector(string sceneName)
        {
            AudioContent content = contents.ToList().Find(x => x.requiredScene == sceneName);
            Debug.Log("Content: " + content);
            if (content != null)
            {
                Debug.Log("Change audio by scene");
                ChangeBGM(content.clip);
            }
        }
        public void ChangeBGM(AudioClip newClip)
        {
            StartCoroutine(ChangeBGMCoroutine(newClip));
        }

        IEnumerator ChangeBGMCoroutine(AudioClip newClip)
        {
            if (bgmSource.isPlaying)
            {
                Debug.Log("Audio is playing, fading out...");
                yield return StartCoroutine(WaitForBGMFadeOut());
            }

            bgmSource.clip = newClip;
            bgmSource.Play();
            Debug.Log("New audio, fading in...");
            yield return StartCoroutine(WaitForBGMFadeIn());
        }

        #region Audio Fading 
        private void BGMFadeIn(float duration = 1)
        {
            StartCoroutine(WaitForBGMFadeIn(duration));
        }

        public void BGMFadeOut(float duration = 1)
        {
            StartCoroutine(WaitForBGMFadeOut(duration));
        }
        IEnumerator WaitForBGMFadeIn(float duration = 1)
        {
            bgmSource.volume = 0;
            while (bgmSource.volume < 1)
            {
                bgmSource.volume += 0.05f;
                yield return new WaitForSeconds(duration / 20f);
            }
            bgmSource.volume = 1;
        }
        IEnumerator WaitForBGMFadeOut(float duration = 1)
        {
            bgmSource.volume = 1;
            while (bgmSource.volume > 0)
            {
                bgmSource.volume -= 0.05f;
                yield return new WaitForSeconds(duration / 20f);
            }
            bgmSource.volume = 0;
        }
        #endregion
    }

    [System.Serializable]
    public class AudioContent
    {
        public string requiredScene = "";
        public int requiredProgress = -1;
        public AudioClip clip;
    }
}