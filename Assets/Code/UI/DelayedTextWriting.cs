using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class DelayedTextWriting : MonoBehaviour
    {
        public Text Text;
        public float Speed = 0.1f;

        private float timer;
        private string currentText =  "";
        private string pushedText = "";
        private string remainingText = "";
        private bool done = false;

        public void Start()
        {
            if (Text.text == null)
            {
                return;
            }
            if (Text.text == "ERROR"|| Text.text == "")
            {
                return;
            }
            pushedText = Text.text;
            remainingText = pushedText;
            timer = Speed;
            Text.text = currentText;
        }

        public void PushText(string text)
        {
            pushedText = text;
            remainingText = pushedText;
            timer = Speed;
            currentText = "";
            Text.text = currentText;
        }

        public void Reset()
        {
            Debug.Log("RESET");
            done = false;
            PushText(pushedText);
        }

        public void Update()
        {
            if (remainingText.Length <= 0)
            {
                return;
            }
            if (!done)
            {
                timer -= UnityEngine.Time.deltaTime;
                while (timer <= 0)
                {
                    timer += Speed;
                    currentText += remainingText[0];
                    remainingText=remainingText.Remove(0, 1);
                    Text.text = currentText;
                    if (remainingText.Length == 0)
                    {
                        done = true;
                        return;
                    }
                }
            }
        }
    }
}
