using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Resources
{
    public class MenuItem : MonoBehaviour
    {
        public List<RectTransform> SelectItems;
        public GameObject AffectedObject;
        public Material.CameraOverlay CameraOverlay;
        public Menu Menu;
        public AudioSource AudioSource;

        private bool turnOff = false;

        private bool selected;

        public void Awake()
        {
            Unselect();
            if (AudioSource == null)
            {
                AudioSource = GetComponentInParent<AudioSource>();
            }
        }

        public void Select(bool jitter = true, bool playClip = true)
        {
            if (playClip)
            {
                PlayClip(Menu.SelectAudioClip);
            }
            selected = true;
            foreach (var selectItem in SelectItems)
            {
                selectItem.gameObject.SetActive(true);
            }
            if (jitter)
            {
                CameraOverlay.SetJitter(0.5f, 0.05f);
            }
        }

        public void Unselect()
        {
            selected = false;
            foreach (var selectItem in SelectItems)
            {
                selectItem.gameObject.SetActive(false);
            }
        }

        public void Enter()
        {
            PlayClip(Menu.EnterAudioClip);
            Menu.TextConsole.PushBack("Entering " + GetComponent<Text>().text);
            CameraOverlay.SetJitter(1f,0.1f);
            AffectedObject.SetActive(true);
            Menu.Deactivate();
        }

        public void Click()
        {
            if (!selected)
            {
                Menu.SelectNew(this);
            }
            else
            {
                Enter();
            }
        }

        private void PlayClip(AudioClip audioClip)
        {
            AudioSource.Stop();
            AudioSource.clip = audioClip;
            AudioSource.Play();
        }
    }
}
