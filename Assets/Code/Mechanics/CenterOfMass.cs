using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class CenterOfMass : MonoBehaviour
    {
        public Vector3 Position = new Vector3(0,0,0);

        public void Start()
        {
            GetComponent<Rigidbody>().centerOfMass = Position;
        }
    }
}
