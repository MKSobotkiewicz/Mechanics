using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Camera
{
    public class TurretCameraBehaviour : MonoBehaviour
    {
        public RenderTexture RenderTexture { get;private set; }

        private Mechanics.Cannon cannon;

        public void Start()
        {
            var tc = GetComponentInParent<Mechanics.TurretController>();
            if (tc == null)
            {
                return;
            }
            cannon = tc.GetComponentInChildren<Mechanics.Cannon>();
            var camera = GetComponent<UnityEngine.Camera>();
            camera.enabled = false;
        }

        public void Update()
        {
            //transform.localEulerAngles = cannonRotator.transform.localEulerAngles;
            RaycastHit hit;
            if (Physics.Raycast(cannon.transform.position + cannon.transform.forward * 2, cannon.transform.forward, out hit, 2000, ~(1 << 10)))
            {
                var target=Quaternion.LookRotation(hit.point - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, UnityEngine.Time.deltaTime * 10);
                //transform.LookAt(hit.point);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, cannon.transform.rotation, UnityEngine.Time.deltaTime*10);
            }
        }
    }
}
