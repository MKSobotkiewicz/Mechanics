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
        public bool Enchancement { get; set; } = false;
        public Attack Attack { get; private set; } = new Attack();
        public Defense Defense { get; private set; } = new Defense();
        public float Speed { get; set; }
        public bool IgnoreTerrain { get; set; }
        public uint MaxManpower { get; set; } = 100;
        public uint MaxCohesion { get; set; } = 100;

    }
}
