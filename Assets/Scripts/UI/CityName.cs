using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class CityName : MonoBehaviour
    {
        public GameObject Panel;
        public Text Name;

        private RectTransform rectTransform;
        private Map.City city;
        private UnityEngine.Camera mainCamera;

        private bool shown = true;

        public void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            mainCamera = UnityEngine.Camera.main;
        }

        public void Update()
        {
            if (city != null && Panel.activeInHierarchy)
            {
                var newPos = mainCamera.WorldToScreenPoint(city.TextPosition);
                rectTransform.position = new Vector3(newPos.x, newPos.y, 0);
            }
            if (Vector3.Distance(mainCamera.transform.position, city.transform.position) >40000)
            {
                if (shown)
                {
                    Hide();
                }
                return;
            }
            if (mainCamera.fieldOfView > 3)
            {
                if (shown)
                {
                    Hide();
                }
                return;
            }
            if (!shown)
            {
                Show();
            }
        }

        public void SetCity(Map.City _city)
        {
            city = _city;
            Name.text = city.name.ToUpper();
        }

        private void Show()
        {
            shown = true;
            Panel.SetActive(true);
            LeanTween.textAlpha(Name.rectTransform, 1, 0.2f).setEaseInOutSine();
        }

        private void Hide()
        {
            shown = false;
            LeanTween.textAlpha(Name.rectTransform, 0, 0.2f).setEaseInOutSine().setOnComplete(() => HideEnd());
        }

        private void HideEnd()
        {
            Panel.SetActive(false);
        }

        public void Destroy()
        {
            LeanTween.textAlpha(Name.rectTransform, 0, 0.2f).setEaseInOutSine().setOnComplete(() => Remove());
        }

        private void Remove()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
