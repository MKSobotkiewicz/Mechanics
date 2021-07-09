using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Resources
{
    public class ResourceGeneratorType : MonoBehaviour
    {
        public List<ResourceValue> CostPerDay = new ResourceValueList();
        public List<ResourceValue> ProductionPerDay = new ResourceValueList();
        public int Priority = 1;

        public void Start()
        {
        }
    }
}
