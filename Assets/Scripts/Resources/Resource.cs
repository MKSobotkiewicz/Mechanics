using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Resources
{
    [System.Serializable]
    public class Resource : MonoBehaviour
    {
        public Sprite Icon;
        public EUnit Unit;
        public bool Volatile;

        public void Start()
        {
        }

        public enum EUnit
        {
            KilowattHour,
            Kilogram,
            Integer
        }
    }
}
