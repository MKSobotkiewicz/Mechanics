using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Player
{
    public class Character : MonoBehaviour
    {
        public float ForwardAcceleration = 1;
        public float BackwardsAcceleration = 1;
        public float SideAcceleration = 1;
        public float RotationAcceleration = 1;
        public float JumpForce = 1;

        private new Rigidbody rigidbody;
        private UnityEngine.Camera camera;
        private bool jumpReady = true;
        private float jumpTimer = 0;
        private float jumpTime = 1;

        public void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                Debug.LogError("Character "+name+" is missing rigidbody");
            }
            camera = GetComponentInChildren<UnityEngine.Camera>();
            if (camera == null)
            {
                Debug.LogError("Character " + name + " is missing camera");
            }
        }

        public void Update()
        {
            if (!jumpReady)
            {
                jumpTimer -= UnityEngine.Time.deltaTime;
                if (jumpTimer < 0)
                {
                    jumpTimer = jumpTime;
                    jumpReady = true;
                }
            }
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");
            var jump = Input.GetAxis("Jump");
            if (vertical >= 0)
            {
                rigidbody.AddRelativeForce(new Vector3(0, 0, vertical * ForwardAcceleration), ForceMode.Force);
            }
            else
            {
                rigidbody.AddRelativeForce(new Vector3(0, 0, vertical * BackwardsAcceleration), ForceMode.Force);
            }
            rigidbody.AddRelativeForce(new Vector3(horizontal * SideAcceleration ,0 , 0), ForceMode.Force);

            rigidbody.AddRelativeTorque(new Vector3(0, mouseX*RotationAcceleration, 0), ForceMode.Acceleration);
            camera.transform.Rotate(camera.transform.right,- mouseY * RotationAcceleration* UnityEngine.Time.deltaTime*30,Space.World);

            if (jump > 0&& jumpReady)
            {
                Debug.Log("JUMP");
                jumpReady = false;
                rigidbody.AddRelativeForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
            }
        }
    }
}
