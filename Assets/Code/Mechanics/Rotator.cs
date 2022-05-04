using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class Rotator : MonoBehaviour
    {
        public bool Locked = false;
        public float Power = 0;
        [Range (-1,1)]
        public float Signal = 0;
        public AxisE X = AxisE.Disabled;
        public AxisE Y = AxisE.Disabled;
        public AxisE Z = AxisE.Disabled;
        
        private new Rigidbody rigidbody;
        private ConfigurableJoint joint;

        private bool lockSwitch=false;

        public void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            joint = GetComponent<ConfigurableJoint>();
        }

        public void FixedUpdate()
        {
            Rotate(UnityEngine.Time.fixedDeltaTime);
        }

        public void SetSignal(float signal)
        {
            Signal = Math.General.Clamp(signal, -1,1);
        }

        void Rotate(float time)
        {
            if (Locked&& !lockSwitch)
            {
                lockSwitch = true;
                rigidbody.angularVelocity = new Vector3(0,0,0);
                joint.angularXMotion = ConfigurableJointMotion.Locked;
                joint.angularYMotion = ConfigurableJointMotion.Locked;
                joint.angularZMotion = ConfigurableJointMotion.Locked;
                return;
            }
            if (!Locked&&lockSwitch)
            {
                lockSwitch = false;
                if (X == AxisE.Enabled)
                {
                    joint.angularXMotion = ConfigurableJointMotion.Free;
                }
                if (Y == AxisE.Enabled)
                {
                    joint.angularYMotion = ConfigurableJointMotion.Free;
                }
                if (Z == AxisE.Enabled)
                {
                    joint.angularZMotion = ConfigurableJointMotion.Free;
                }
            }
            rigidbody.angularVelocity= transform.TransformDirection(new Vector3( -Signal * Power * UnityEngine.Time.fixedDeltaTime* (float)X,
                                                    -Signal * Power * UnityEngine.Time.fixedDeltaTime* (float)Y,
                                                    -Signal * Power * UnityEngine.Time.fixedDeltaTime* (float)Z));
            /*rigidbody.AddRelativeTorque(new Vector3(-Signal * Power * Time.fixedDeltaTime * (float)X,
                                                    -Signal * Power * Time.fixedDeltaTime * (float)Y,
                                                    -Signal * Power * Time.fixedDeltaTime * (float)Z),ForceMode.Impulse);*/
        }

        public enum AxisE
        {
            Enabled=1,
            Disabled=0
        }
    }

}