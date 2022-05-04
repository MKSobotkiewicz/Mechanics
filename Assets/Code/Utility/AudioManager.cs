using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Utility
{
    public class AudioManager : MonoBehaviour
    {
        public List<AudioClip> Clips;
        public float MinPitch=1;
        public float MaxPitch=1;

        private static readonly System.Random random = new System.Random();

        public void Start()
        {
            var source = GetComponent<AudioSource>();
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
            source.clip = Clips[random.Next(Clips.Count - 1)];
            source.pitch = (float)random.NextDouble()* (MaxPitch- MinPitch)+ MinPitch;
            source.Play();
        }
    }
}
