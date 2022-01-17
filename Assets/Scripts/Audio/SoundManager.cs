using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Audio
{
    public class SoundManager : MonoBehaviour
    {
        public List<AudioClip> AudioClips=new List<AudioClip>();
        public bool PlayOnEnable=false;

        protected AudioSource audioSource;

        protected static System.Random random = new System.Random();

        void OnEnable()
        {
            audioSource = GetComponent<AudioSource>();
            if (PlayOnEnable)
            {
                Play();
            }
        }

        public void Play()
        {
            if (AudioClips.Count <= 0)
            {
                return;
            }
            audioSource.PlayOneShot(AudioClips[random.Next(0, AudioClips.Count-1)]);
        }
    }
}