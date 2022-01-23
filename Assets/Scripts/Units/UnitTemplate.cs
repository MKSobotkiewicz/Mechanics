using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Project.Units
{
    [Serializable]
    public class UnitTemplate : MonoBehaviour
    {
        public Attack Attack { get; private set; }
        public Defense Defense { get; private set; }
        public float Speed { get; set; }
        public uint MaxManpower { get; set; } = 100;
        public uint MaxCohesion { get; set; } = 100;

    }
}
