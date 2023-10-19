using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Resources
{
    public class ResourceGeneratorType : MonoBehaviour
    {
        public List<ResourceValue> CostPerDay = new ResourceValueList();
        public List<ResourceValue> ProductionPerDay = new ResourceValueList();
        public List<ResourceValue> BuildCostPerDay = new ResourceValueList();
        public int BuildingTime = 0;
        public uint Priority = 1;

        public void Start()
        {
        }
    }
}
