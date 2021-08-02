using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class TextSwitcher : MonoBehaviour
    {
        public List<string> Strings;
        public float Time;

        private Text text;
        private int index = 0;
        private float timer = 0;

        public void Awake()
        {
            text = GetComponentInChildren<Text>();
            timer = Time;
        }

        public void OnEnable()
        {
            index = 0;
            timer = Time;
            text.text = Strings[index];
        }

        public void Update()
        {
            timer -= UnityEngine.Time.deltaTime;
            if (timer <= 0)
            {
                timer=Time;
                Switch();
            }
        }

        private void Switch()
        {
            index++;
            if (index >= Strings.Count)
            {
                index = 0;
            }
            text.text = Strings[index];
        }
    }
}