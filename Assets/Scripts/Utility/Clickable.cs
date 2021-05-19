using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Utility
{
    public class Clickable : MonoBehaviour
    {
        public List<MonoBehaviour> IClicables = new List<MonoBehaviour>();

        public void OnMouseDown()
        {
            foreach (var clicable in IClicables)
            {
                ((IClicable)clicable).Click();
            }
        }
    }
}
