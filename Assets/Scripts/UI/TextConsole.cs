using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class TextConsole : MonoBehaviour
    {
        public Text TextPrefab;

        private List<Text> textObjects=new List<Text>();
        private float height;
        private int maxCount;

        public void Start()
        {
            height = TextPrefab.rectTransform.sizeDelta.y;
            maxCount = (int)(GetComponent<RectTransform>().sizeDelta.y/ height);
        }

        public void PushBack(string text)
        {
            foreach (var textObject in textObjects)
            {
                textObject.rectTransform.anchoredPosition = new Vector2(textObject.rectTransform.anchoredPosition.x, textObject.rectTransform.anchoredPosition.y+height);
            }
            var newText = Instantiate(TextPrefab,transform);
            newText.rectTransform.anchoredPosition = new Vector2();
            textObjects.Add(newText);
            while (textObjects.Count > maxCount)
            {
                Destroy(textObjects[0].gameObject);
                textObjects.RemoveAt(0);
            }
            var delayedTextWriting = newText.GetComponent<DelayedTextWriting>();
            if (delayedTextWriting == null)
            {
                newText.text = text;
            }
            else
            {
                delayedTextWriting.PushText(text);
            }
        }
    }
}
