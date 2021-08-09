using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Utility
{
    public class Selector : MonoBehaviour
    {
        private new UnityEngine.Camera camera;

        public void Start()
        {
            camera = GetComponent<UnityEngine.Camera>();
            if (camera == null)
            {
                Debug.LogError(name+ " missing camera");
            }
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 50000))
                {
                    var clickable = hit.transform.gameObject.GetComponent<Clickable>();
                    if (clickable != null)
                    {
                        clickable.Click();
                    }
                }
            }
        }
    }
}
