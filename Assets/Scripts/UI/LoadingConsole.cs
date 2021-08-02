using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class LoadingConsole : MonoBehaviour
    {
        public TextConsole TextConsole;

        public TextAsset FluffTextFile;
        public float Time = 1;

        private List<string> names;
        private float timer = 0;

        public void Start()
        {
            if (TextConsole == null)
            {
                Debug.LogError(name+ " is missing text console.");
            }
            LoadNames();
            timer = Time;
        }

        public void Update()
        {
            timer-= UnityEngine.Time.deltaTime;
            if (timer <= 0)
            {
                timer = Time;
                TextConsole.PushBack(Utility.ListUtilities.GetRandomObject(names));
            }
        }

        private void LoadNames()
        {
            var parser = new Data.XmlParser(FluffTextFile.text);
            names = parser.Parse("string");
        }
    }
}
