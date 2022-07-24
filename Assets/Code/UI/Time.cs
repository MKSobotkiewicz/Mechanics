using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    class Time : MonoBehaviour
    {
        public Text PausedText;
        public Text SpeedText;
        public Text HourText;
        public Text DateText;
        public Project.Time.Time TimeGameObject;

        private uint lastSpeed = 1;
        private bool paused = true;

        public void Update()
        {
            HourText.text = TimeGameObject.Hour.ToString ("D2") + ":" + TimeGameObject.Minute.ToString("D2");
            var date = TimeGameObject.Date();
            DateText.text = date.Item1.ToString("D2") + "-" + date.Item2.ToString("D2") + "-" + TimeGameObject.Year.ToString("D4");
        }

        public void SwitchPaused()
        {
            if (paused)
            {
                PausedText.gameObject.SetActive(false);
                TimeGameObject.Speed = lastSpeed;
                paused = false;
                return;
            }
            paused = true;
            lastSpeed = TimeGameObject.Speed;
            TimeGameObject.Speed = 0;
            PausedText.gameObject.SetActive(true);
        }

        public void Slower()
        {
            if (paused&& lastSpeed != 1)
            {
                SwitchPaused();
            }
            switch (TimeGameObject.Speed)
            {
                case 1:
                    SwitchPaused();
                    break;
                case 2:
                    TimeGameObject.Speed = 1;
                    SpeedText.text = ">";
                    break;
                case 4:
                    TimeGameObject.Speed = 2;
                    SpeedText.text = ">>";
                    break;
                case 8:
                    TimeGameObject.Speed = 4;
                    SpeedText.text = ">>>";
                    break;
                case 16:
                    TimeGameObject.Speed = 8;
                    SpeedText.text = ">>>>";
                    break;
                case 32:
                    TimeGameObject.Speed = 16;
                    SpeedText.text = ">>>>>";
                    break;
            }
        }

        public void Faster()
        {
            if (paused)
            {
                SwitchPaused();
            }
            switch (TimeGameObject.Speed)
            {
                case 0:
                    TimeGameObject.Speed = 1;
                    SpeedText.text = ">";
                    break;
                case 1:
                    TimeGameObject.Speed = 2;
                    SpeedText.text = ">>";
                    break;
                case 2:
                    TimeGameObject.Speed = 4;
                    SpeedText.text = ">>>";
                    break;
                case 4:
                    TimeGameObject.Speed = 8;
                    SpeedText.text = ">>>>";
                    break;
                case 8:
                    TimeGameObject.Speed = 16;
                    SpeedText.text = ">>>>>";
                    break;
                case 16:
                    TimeGameObject.Speed = 32;
                    SpeedText.text = ">>>>>>";
                    break;
            }
        }
    }
}
