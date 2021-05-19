using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class TurretControllerType : MonoBehaviour
    {
        public float RotationDelay = 0f;
        public Math.PIDController.PIDGains PIDGains;
        public bool Predictive = false;
    }
}
