using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Camera
{
    public class Rangefinder:MonoBehaviour
    {
        private Text text;
        private new UnityEngine.Camera camera;
        private static readonly float maxDistance = 2000;

        public void Start()
        {
            var tc = GetComponentInParent<Mechanics.TurretController>();
            if (tc == null)
            {
                return;
            }
            camera = tc.GetComponentInChildren<UnityEngine.Camera>();
            if (camera == null)
            {
                return;
            }
            var texts = camera.GetComponentsInChildren<Text>();
            foreach(var t in texts)
            {
                if (t.tag == "RangefinderText")
                {
                    text = t;
                    break;
                }
            }
        }

        public void Update()
        {
            if (camera == null|| text==null)
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, maxDistance))
            {
                text.text = hit.distance.ToString("0000", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                text.text = "XXXX";
            }
        }
    }
}
