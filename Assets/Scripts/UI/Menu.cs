using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Resources
{
    public class Menu : MonoBehaviour
    {
        public List<MenuItem> MenuItems;
        public UnityEngine.Rendering.HighDefinition.CustomPassVolume CustomPassVolume;
        public UI.TextConsole TextConsole;
        public AudioClip SelectAudioClip;
        public AudioClip EnterAudioClip;
        private int selectedIndex;
        private float dropTimer;
        private float minStrength=0.03f;

        public void Start()
        {
            foreach (var menuIntem in MenuItems)
            {
                menuIntem.Menu = this;
            }
            MenuItems[0].Select(false,false);
            selectedIndex = 0;
        }

        public void Update()
        {
            CheckKeys();
            if (dropTimer > 0)
            {
                dropTimer -= UnityEngine.Time.deltaTime;
                if (dropTimer <= 0)
                {
                    dropTimer = 0;
                }
            }
        }

        private void CheckKeys()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                var newIndex = selectedIndex-1;
                if (newIndex < 0)
                {
                    newIndex = MenuItems.Count - 1;
                }
                SelectNew(newIndex);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                var newIndex = selectedIndex+1;
                if (newIndex >= MenuItems.Count)
                {
                    newIndex = 0;
                }
                SelectNew(newIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                MenuItems[selectedIndex].Enter();
            }
        }

        public void Deactivate()
        {
            var delayedTextWritings = GetComponentsInChildren<UI.DelayedTextWriting>();
            foreach (var delayedTextWriting in delayedTextWritings)
            {
                delayedTextWriting.Reset();
            }
            gameObject.SetActive(false);
        }

        public void SelectNew(int newSelectedIndex)
        {
            if (newSelectedIndex == selectedIndex)
            {
                return;
            }
            MenuItems[selectedIndex].Unselect();
            MenuItems[newSelectedIndex].Select();
            TextConsole.PushBack("Selected "+MenuItems[newSelectedIndex].GetComponent<Text>().text);
            selectedIndex = newSelectedIndex;
        }

        public void SelectNew(MenuItem newSelected)
        {
            if (newSelected == MenuItems[selectedIndex])
            {
                return;
            }
            MenuItems[selectedIndex].Unselect();
            newSelected.Select();
            TextConsole.PushBack("Selected " + newSelected.GetComponent<Text>().text);
            selectedIndex = MenuItems.IndexOf(newSelected);
        }
    }
}
