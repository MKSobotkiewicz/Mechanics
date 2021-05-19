using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class TurretVisor : MonoBehaviour
    {
        private Transform cameraTransform;

        public void Start()
        {
            var vehicle3rdPersonCamera = UnityEngine.Camera.main.GetComponentInParent<Camera.Vehicle3rdPersonCamera>();
            if (vehicle3rdPersonCamera == null)
            {
                return;
            }
            cameraTransform = vehicle3rdPersonCamera.transform;
        }

        public void Update()
        {
            transform.rotation=Quaternion.LookRotation(Vector3.Cross(transform.parent.up,cameraTransform.right),transform.parent.up);
            //transform.Rotate(transform.parent.up, 90);
        }
    }
}
