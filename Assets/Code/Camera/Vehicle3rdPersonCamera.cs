using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Project.Camera
{
    public class Vehicle3rdPersonCamera : MonoBehaviour
    {
        public bool Locked = true;

        public float XRotationValue = 0.0F;
        public float YRotationValue = 0.0F;

        public float CameraZoomTarget = 40F;
        public float CameraZoomSpeed = 50.0F;
        public float RotationSpeed = 10.0F;

        public float MaxCameraZoomin = 20.0F;
        public float MaxCameraZoomout = 50.0F;
        public float MinRotation = -180.0F;
        public float MaxRotation = 180.0F;

        private new UnityEngine.Camera camera;

        void Start()
        {
            camera = GetComponentInChildren<UnityEngine.Camera>();
            if (camera == null)
            {
                throw new System.ArgumentNullException("camera", "Missing camera object.");
            }
        }

        void Update()
        {
            if (Locked)
            {
                return;
            }
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, CameraZoomTarget, UnityEngine.Time.deltaTime * 10);

            CameraZoomTarget = CameraZoomTarget - Input.GetAxis("Zoom") * CameraZoomSpeed;
            if (CameraZoomTarget < MaxCameraZoomin)
            {
                CameraZoomTarget = MaxCameraZoomin;
            }
            if (CameraZoomTarget > MaxCameraZoomout)
            {
                CameraZoomTarget = MaxCameraZoomout;
            }

            XRotationValue += Input.GetAxis("Mouse Y") * RotationSpeed;
            YRotationValue += Input.GetAxis("Mouse X") * RotationSpeed;
            XRotationValue %= 360;
            YRotationValue %= 360;
            if (XRotationValue < MinRotation)
            {
                XRotationValue = MinRotation;
            }
            else if (XRotationValue > MaxRotation)
            {
                XRotationValue = MaxRotation;
            }
            var targetVector = new Vector3(XRotationValue,
                                          YRotationValue,
                                          transform.localEulerAngles.z);
            transform.localEulerAngles = targetVector;
        }
    }
}
