using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Resources
{
    [Serializable]
    public class Resources :  List<ResourceValue>
    {
        public static Resources operator *(float value, Resources resources)
        {
            var newResources = new Resources();
            foreach (var resource in resources)
            {
                newResources.Add(value*resource);
            }
            return newResources;
        }
    }
}
