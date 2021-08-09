using System.Collections;
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

        public float ZoomRotation = 1f;
        public float CameraZoomTarget = 40.0F;
        public float CameraZoomSpeed = 1000F;
        public float MaxZoom = -8000f;

        public float CameraOrtZoomTarget = 2000F;
        public float CameraOrtZoomSpeed = 50F;
        public float MaxOrtZoom = 200f;
        
        private new UnityEngine.Camera camera;
        private float distanceClippingPlaneDiffrence;

        void Start()
        {
            camera = GetComponentInChildren<UnityEngine.Camera>();
            if (camera == null)
            {
                Debug.LogError(name+" missing Camera.");
            }
            distanceClippingPlaneDiffrence = -camera.farClipPlane-camera.transform.localPosition.z;
            CameraZoomTarget = camera.transform.localPosition.z;
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
            Debug.Log(Input.GetAxis("Vertical"));
            float tiltX = tiltAroundX - (Input.GetAxis("Vertical")+ Input.GetAxis("Zoom")* ZoomRotation) * XAngleSpeed * CameraZoomTarget;
            if (tiltX < 90 && tiltX > -120)
            {
                tiltAroundX = tiltX;
            }
            tiltAroundZ += Input.GetAxis("Horizontal") * ZAngleSpeed * CameraZoomTarget;
            Quaternion target = Quaternion.Euler(tiltAroundX, tiltAroundZ, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, UnityEngine.Time.fixedDeltaTime * smooth);

            if (camera.orthographic)
            {
                CameraOrtZoomTarget = CameraOrtZoomTarget - Input.GetAxis("Zoom") * CameraOrtZoomSpeed;
                if (CameraOrtZoomTarget < MaxOrtZoom) CameraOrtZoomTarget = MaxOrtZoom;
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, CameraOrtZoomTarget, UnityEngine.Time.fixedDeltaTime * smooth);
            }
            else
            {
                CameraZoomTarget = CameraZoomTarget + Input.GetAxis("Zoom") * CameraZoomSpeed;
                if (CameraZoomTarget > MaxZoom) CameraZoomTarget = MaxZoom;
                //camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, CameraZoomTarget, UnityEngine.Time.fixedDeltaTime * smooth);
                camera.transform.localPosition = Vector3.Lerp(new Vector3(0,0,camera.transform.localPosition.z), new Vector3(0, 0, CameraZoomTarget), UnityEngine.Time.fixedDeltaTime * smooth);
                camera.farClipPlane =- camera.transform.localPosition.z-distanceClippingPlaneDiffrence;
            }
        }
    }
}