using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class Resource : MonoBehaviour
    {
        public Text NameText;
        public Text ValueText;
        public Image Image;

        private Resources.Resource resource;
        private long value=0;

        public Resource Init(Resources.Resource _resource,long value)
        {
            Debug.Log(value);
            resource = _resource;
            Image.sprite = resource.Icon;
            NameText.text = resource.name;
            SetValue(value);
            return this;
        }

        public void SetValue(long newValue)
        {
            value = newValue;
            ValueText.text = Utility.IntParser.Parse(value);
            if (newValue == 0)
            {
                ValueText.color = new Color(1,1,0,1);
                return;
            }
            if (newValue < 0)
            {
                ValueText.color = new Color(1, 0, 0, 1);
                return;
            }
            if (newValue > 0)
            {
                ValueText.color = new Color(0, 1, 0, 1);
                return;
            }
        }
    }
}
