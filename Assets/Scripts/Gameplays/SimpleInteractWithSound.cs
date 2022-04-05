using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    [RequireComponent(typeof(AudioSource))]
    public class SimpleInteractWithSound : MonoBehaviour
    {
        [SerializeField] AudioClip clip;
        [SerializeField] float coldDuration = 1f;
        AudioSource audioSource;

        private bool isDelaying = false;
        private void SetDelayFinish() => isDelaying = false;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.playOnAwake = false;
        }
        public void PlayClip()
        {
            if (isDelaying)
                return;

            isDelaying = true;
            audioSource.Play();
            Invoke("SetDelayFinish", coldDuration);
        }
        private void OnMouseDown()
        {
            PlayClip();
        }
    }
}