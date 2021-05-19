using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Camera
{
    public class VelocityFovScaler : MonoBehaviour
    {
        public float ScalingRate = 1;
        public Rigidbody LinkedRigidbody;

        private new UnityEngine.Camera camera;
        private float startingFOV;

        public void Start()
        {
            camera = GetComponent<UnityEngine.Camera>();
            startingFOV = camera.fieldOfView;
        }

        public void Update()
        {
            camera.fieldOfView = startingFOV + ScalingRate * LinkedRigidbody.velocity.magnitude;
        }
    }
}
