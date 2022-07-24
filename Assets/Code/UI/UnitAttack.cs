using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Project.UI
{
    public class UnitAttack : MonoBehaviour
    {
        private RectTransform[] unitRectTransform = new RectTransform[2];
        private RectTransform rectTransform;

        public void Init(RectTransform unit1RectTransform, RectTransform unit2RectTransform)
        {
            unitRectTransform[0] = unit1RectTransform;
            unitRectTransform[1] = unit2RectTransform;
            rectTransform = GetComponent<RectTransform>();
        }

        public void Update()
        {
            rectTransform.anchoredPosition = (unitRectTransform[0].anchoredPosition + unitRectTransform[1].anchoredPosition) / 2;
            var vec = unitRectTransform[0].anchoredPosition -unitRectTransform[1].anchoredPosition;
            rectTransform.localEulerAngles = new Vector3(0, 0,Mathf.Atan2(vec.x, -vec.y)* 57.2957795f);
        }
    }
}