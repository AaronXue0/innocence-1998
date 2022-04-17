using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;

namespace Innocence
{
    public class AudioPlayer : MonoBehaviour
    {
        public AudioSource bgmSource;
        public AudioContent[] contents;

        #region SoundEffects
        public void PlaySFX(AudioClip clip)
        {
            StartCoroutine(SFXCoroutine(clip));
        }
        IEnumerator SFXCoroutine(AudioClip clip)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.Play();

            while (source.isPlaying)
            {
                yield return null;
            }

            Destroy(source);
        }
        #endregion

        public void StopPlaying()
        {
            bgmSource.Stop();
        }

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
            List<AudioContent> allContents = contents.ToList().FindAll(x => x.requiredScene == sceneName);
            AudioContent content = null;//= allContents.Find(x => x.requiredProgress == GameManager.instance.Progress);

            for (int i = allContents.Count - 1; i >= 0; i--)
            {
                if (allContents[i].requiredProgress >= GameManager.instance.Progress)
                {
                    content = allContents[i];
                    break;
                }
            }
            // Debug.Log("Content: " + content);

            if (content != null)
            {
                Debug.Log("Change audio by scene");
                ChangeBGM(content.clip);
            }
        }
        public void ChangeBGM(AudioClip newClip)
        {
            if (newClip == bgmSource.clip)
                return;
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