using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class CityView : MonoBehaviour,Time.IDaily
    {
        public CityGeneratorsList CityGeneratorsList;
        public CityResourcesList CityResourcesList;
        public float slideValue;
        public Time.Time Time;

        private Map.Area area;
        private Vector2 position;
        private Canvas canvas;
        private LTDescr tween;
        private RectTransform rectTransform;

        public void Start()
        {
            Time.AddDaily(this);
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponent<Canvas>();
            position = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition += new Vector2(-slideValue,0);
        }

        public void DailyUpdate()
        {
            CityGeneratorsList.Init(area);
            CityResourcesList.Init(area);
        }

        public uint Priority()
        {
            return 23;
        }

        public void SetVisible(bool visible)
        {
            if (visible)
            {
                if (tween != null)
                {
                    if (LeanTween.isTweening(tween.id))
                    {
                        LeanTween.cancel(tween.id, true);
                    }
                }
                rectTransform.anchoredPosition = new Vector2(-slideValue, 0);
                LeanTween.move(rectTransform, position, 0.2f).setEaseInOutSine();
                canvas.enabled = visible;
            }
            else
            {
                tween = LeanTween.move(rectTransform, rectTransform.anchoredPosition+new Vector2(-slideValue, 0), 0.2f).setEaseInOutSine().setOnComplete(() => SetInvisible());
            }
        }

        public void SetArea(Map.Area _area)
        {
            area = _area;
            CityGeneratorsList.Init(area);
            CityResourcesList.Init(area);
        }

        private void SetInvisible()
        {
            canvas.enabled = false;
        }
    }
}
