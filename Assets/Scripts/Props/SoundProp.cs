using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Innocence
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundProp : MonoBehaviour
    {
        [SerializeField] AudioClip clip;
        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.playOnAwake = false;
        }
        public void PlayClip()
        {
            audioSource.Play();
        }
    }
}