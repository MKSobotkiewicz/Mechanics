using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class CreateCity : MonoBehaviour
    {
        public InputField InputName;
        public Canvas Canvas;

        private Map.Area area;
        private Vector3 size;
        private Map.CitiesGenerator citiesGenerator;
        private LTDescr tween;

        public void Start()
        {
            size = transform.localScale;
            transform.localScale = new Vector3(0, 0, 0);
            var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                citiesGenerator = rootObject.GetComponentInChildren<Map.CitiesGenerator>();
                if (citiesGenerator != null)
                {
                    break;
                }
            }
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
                transform.localScale = new Vector3(0, 0, 0);
                LeanTween.scale(gameObject, size, 0.2f).setEaseInOutSine();
                Canvas.enabled = visible;
            }
            else
            {
                tween = LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.2f).setEaseInOutSine().setOnComplete(() => SetInvisible());
            }
        }

        public void Create()
        {
            citiesGenerator.CreateCity(InputName.text,area);
            SetVisible(false);
        }

        public void SetArea(Map.Area _area)
        {
            area = _area;
        }

        private void SetInvisible()
        {
            Canvas.enabled = false;
        }
    }
}
