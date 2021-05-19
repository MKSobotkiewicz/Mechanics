using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Camera
{
    public class Zoom : MonoBehaviour
    {
        public float MaxFov = 30;
        public float MinFov = 2;
        public float ZoomSpeed = 5;

        private new UnityEngine.Camera camera;
        private List<Tuple<RectTransform, Vector3>> outerReticles = new List<Tuple<RectTransform, Vector3>>();
        private UnityEngine.UI.Text text;

        private float defaultFov;
        private float currentFov;

        public void Start()
        {
            camera = GetComponent<UnityEngine.Camera>();
            defaultFov = camera.fieldOfView;
            currentFov = defaultFov;
            var rectTransforms = GetComponentsInChildren<RectTransform>();
            foreach (var rectTransform in rectTransforms)
            {
                if (rectTransform.tag == "OuterReticle")
                {
                    outerReticles.Add(new Tuple<RectTransform, Vector3>(rectTransform, rectTransform.localScale * camera.fieldOfView));
                }
            }
            var texts = GetComponentsInChildren<UnityEngine.UI.Text>();
            foreach (var text_ in texts)
            {
                if (text_.tag == "ZoomText")
                {
                    text = text_;
                    break;
                }
            }
        }

        public void Update()
        {
            SetFov();
        }

        private float GetZoomValue()
        {
            return MaxFov / currentFov;
        }

        private void SetFov()
        {
            if (!camera.enabled)
            {
                return;
            }
            currentFov -= Input.GetAxis("Zoom")* ZoomSpeed* currentFov;
            if (currentFov> MaxFov)
            {
                currentFov = MaxFov;
            }
            else if (currentFov < MinFov)
            {
                currentFov = MinFov;
            }
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, currentFov, UnityEngine.Time.deltaTime*10);
            foreach (var outerReticle in outerReticles)
            {
                outerReticle.Item1.localScale = (1 / camera.fieldOfView) * outerReticle.Item2;
            }
            text.text = GetZoomValue().ToString("00.0", System.Globalization.CultureInfo.InvariantCulture)+"x";
        }
    }
}
