using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        public uint AIValue;
        public bool Unextracted { get; private set; }

        private void OnValidate()
        {
            if (name.Contains("[unextracted]"))
            {
                Unextracted = true;
            }
            else
            {
                Unextracted = false;
                var regexItem = new Regex("^[a-zA-Z ]*$");

                if (!regexItem.IsMatch(name))
                {
                    Debug.LogWarning(name + " name contains forbidden characters.");
                }
            }
        }

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
