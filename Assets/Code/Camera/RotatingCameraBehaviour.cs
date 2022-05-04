using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Camera
{
    public class RotatingCameraBehaviour : MonoBehaviour
    {
        public bool Locked = true;

        public float XRotationValue = -30.0F;
        public float YRotationValue = -30.0F;

        public float CameraZoomTarget = 40F;
        public float CameraZoomSpeed = 50.0F;
        public float RotationSpeed = 10.0F;

        public float MaxCameraZoomout = 50.0F;
        public float MinRotation = -90.0F;
        public float MaxRotation = 0.0F;

        new private UnityEngine.Camera camera;
        private Transform anchor;

        void Start()
        {
            camera = GetComponent<UnityEngine.Camera>();
            if (camera == null)
            {
                throw new System.ArgumentNullException("camera","Missing camera object.");
            }
            anchor = camera.transform.parent;
            if (anchor == null)
            {
                throw new System.ArgumentNullException("anchor", "Missing camera anchor object.");
            }
        }

        void Update()
        {
            if (Locked)
            {
                return;
            }
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView,CameraZoomTarget, UnityEngine.Time.deltaTime*10);

            CameraZoomTarget = CameraZoomTarget - Input.GetAxis("Mouse ScrollWheel") * CameraZoomSpeed;
            if (CameraZoomTarget < 1)
            {
                CameraZoomTarget = 1f;
            }
            if (CameraZoomTarget > MaxCameraZoomout)
            {
                CameraZoomTarget = MaxCameraZoomout;
            }
            
            if (Input.GetAxis("Middle Mouse Button") > 0)
            {
                YRotationValue += Input.GetAxis("Mouse X") * RotationSpeed;
                XRotationValue += Input.GetAxis("Mouse Y") * RotationSpeed;
                YRotationValue %= 360;
                XRotationValue %= 360;
                if (XRotationValue < MinRotation)
                {
                    XRotationValue = MinRotation;
                }
                else if (XRotationValue > MaxRotation)
                {
                    XRotationValue = MaxRotation;
                }
            }
            anchor.localEulerAngles = new Vector3(XRotationValue,
                                             YRotationValue,
                                             anchor.localEulerAngles.z);
        }
    }
}