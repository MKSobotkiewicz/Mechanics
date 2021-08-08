using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Utility
{
    public class Clickable : MonoBehaviour
    {
        public List<MonoBehaviour> IClicables = new List<MonoBehaviour>();

        public void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) { return; }
            foreach (var clicable in IClicables)
            {
                ((IClicable)clicable).Click();
            }
        }
    }
}
