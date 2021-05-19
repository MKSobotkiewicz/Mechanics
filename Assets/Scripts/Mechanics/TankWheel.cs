using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class TankWheel : MonoBehaviour,ISide
    {
        public SideE Side = SideE.Right;
        public float Radious = 0;
        public bool Ground = false;
        public bool Engine = false;

        public float Torque { get; private set; }

        private float maxSpeed = 100;
        private new Rigidbody rigidbody;

        public void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                Debug.Log(gameObject.name+" lacks rigidbody");
            }
        }

        public void SetMaxSpeed(float _maxSpeed)
        {
            maxSpeed = _maxSpeed;
        }

        public float AngularVelocity()
        {
            switch (Side)
            {
                case SideE.Right:
                    return transform.InverseTransformDirection(rigidbody.angularVelocity).x;
                case SideE.Left:
                    return -transform.InverseTransformDirection(rigidbody.angularVelocity).x;
            }
            return 0;
        }

        public void Accelerate(float torque)
        {
            Torque = torque;
            switch (Side)
            {
                case SideE.Right:
                    rigidbody.AddRelativeTorque(new Vector3(torque / Radious, 0, 0), ForceMode.Acceleration);
                    break;
                case SideE.Left:
                    rigidbody.AddRelativeTorque(new Vector3(-torque / Radious, 0, 0), ForceMode.Acceleration);
                    break;
            }
        }

        public void Decelerate(float torque)
        {
            Torque = -torque;
            Accelerate(-torque);
        }

        public void Brake(float torque)
        {
            Torque = -torque;
            switch (Side)
            {
                case SideE.Right:
                    if (rigidbody.angularVelocity.x > 0)
                    {
                        rigidbody.AddRelativeTorque(new Vector3(-torque / Radious, 0, 0), ForceMode.Acceleration);
                    }
                    else
                    {
                        rigidbody.AddRelativeTorque(new Vector3(torque / Radious, 0, 0), ForceMode.Acceleration);
                    }
                    break;
                case SideE.Left:
                    if (rigidbody.angularVelocity.x > 0)
                    {
                        rigidbody.AddRelativeTorque(new Vector3(torque / Radious, 0, 0), ForceMode.Acceleration);
                    }
                    else
                    {
                        rigidbody.AddRelativeTorque(new Vector3(-torque / Radious, 0, 0), ForceMode.Acceleration);
                    }
                    break;
            }
        }

        public SideE GetSide()
        {
            return Side;
        }

        public void Stop()
        {
            Torque = 0;
            rigidbody.angularVelocity = new Vector3(0, 0, 0);
        }
    }
}
