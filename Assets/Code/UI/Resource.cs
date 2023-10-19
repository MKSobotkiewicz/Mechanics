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
        public Image OverlayPanel;

        private Resources.Resource resource;
        private long value=0;

        public Resource Init(Resources.Resource _resource,long value)
        {
            Debug.Log(value);
            resource = _resource;
            Image.sprite = resource.Icon;
            NameText.text = resource.name.ToUpper();
            SetValue(value);
            return this;
        }

        public void OverlayOn()
        {
            OverlayPanel.enabled = true;
        }

        public void OverlayOff()
        {
            OverlayPanel.enabled = false;
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
                ValueText.color = new Color(1, 0.5f, 0.5f, 1);
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
