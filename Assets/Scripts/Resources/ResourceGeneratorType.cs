using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Resources
{
    public class ResourceGeneratorType : MonoBehaviour
    {
        public List<ResourceValue> CostPerDay = new Resources();
        public List<ResourceValue> ProductionPerDay = new Resources();
        public int Priority = 1;

        public void Start()
        {
        }
    }
}
