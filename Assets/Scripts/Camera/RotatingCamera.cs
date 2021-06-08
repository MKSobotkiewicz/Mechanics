﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Camera
{
    public class RotatingCamera : MonoBehaviour
    {

        public float smooth = 10.0F;
        public float XAngleSpeed = 0.1F;
        public float ZAngleSpeed = 0.1F;

        public float tiltAroundX = 0.0F;
        public float tiltAroundZ = 0.0F;

        public float CameraZoomTarget = 40.0F;
        public float CameraZoomSpeed = 5.0F;
        public float MaxZoom = 0.3f;

        private new UnityEngine.Camera camera;

        void Start()
        {
            camera = GetComponentInChildren<UnityEngine.Camera>();
            if (camera == null)
            {
                Debug.LogError(name+" missing Camera.");
            }
            /*var distances = new float[32];
            distances[0] = 40000;
            distances[1] = 40000;
            distances[2] = 40000;
            distances[3] = 40000;
            distances[4] = 40000;
            distances[5] = 40000;
            camera.layerCullDistances = distances;*/

        }

        void FixedUpdate()
        {
            float tiltX = tiltAroundX + Input.GetAxis("Vertical") * XAngleSpeed * CameraZoomTarget;
            if (tiltX < 45 && tiltX > -120)
                tiltAroundX = tiltX;
            tiltAroundZ -= Input.GetAxis("Horizontal") * ZAngleSpeed * CameraZoomTarget;
            Quaternion target = Quaternion.Euler(tiltAroundX, tiltAroundZ, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, UnityEngine.Time.fixedDeltaTime * smooth);

            CameraZoomTarget = CameraZoomTarget - Input.GetAxis("Zoom") * CameraZoomSpeed;
            if (CameraZoomTarget < MaxZoom) CameraZoomTarget = MaxZoom;
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, CameraZoomTarget, UnityEngine.Time.fixedDeltaTime * smooth);
        }
    }
}