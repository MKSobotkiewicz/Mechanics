using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Project.UI
{
    public class Damage : MonoBehaviour
    {
        public Text Text;
        public RectTransform RectTransform;

        public void Init(int value)
        {
            Text.text = "-" + value;
        }

        public void Start()
        { 
            LeanTween.alphaText(Text.rectTransform,0,2).setEaseOutSine();
            LeanTween.moveLocalY(RectTransform.gameObject, RectTransform.anchoredPosition.y+100, 2).setEaseOutSine().setOnComplete(() => Destroy(gameObject));
        }
    }
}
