using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class UiBar:MonoBehaviour
    {
        private RectTransform barTransform;
        private Vector2 sizeDelta;
        
        public void Awake()
        {
            barTransform = GetComponent<RectTransform>();
            sizeDelta = barTransform.sizeDelta;
        }

        public void UpdateValue(float value)
        {
            if (value < 0)
            {
                value = 0;
            }
            else if (value > 1)
            {
                value = 1;
            }
            LeanTween.size(barTransform, new Vector2(value*sizeDelta.x, sizeDelta.y),0.5f).setEaseInOutSine();
        }
    }
}
