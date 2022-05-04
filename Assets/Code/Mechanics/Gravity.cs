using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class Gravity : MonoBehaviour
    {
        public static readonly float Value = -9.807f;
        public static Vector3 CenterOfMass = new Vector3(0,0,0);

        private new Rigidbody rigidbody;

        public void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                Debug.LogError(name+" missing rigidbody.");
            }
        }

        public void FixedUpdate()
        {
            rigidbody.AddForce(Value* GetGravityVector(transform.position), ForceMode.Acceleration);
        }

        public static Vector3 GetGravityVector(Vector3 position)
        {
            return (position- CenterOfMass).normalized;
        }
    }
}
