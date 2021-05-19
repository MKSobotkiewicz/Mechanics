using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Resources
{
    public class ResourceGeneratorTestCreator : MonoBehaviour
    {
        public ResourceGeneratorType[] ResourceGeneratorTypes;
        public Time.Time Time;

        public void Start()
        {
            var rd = ResourceDepot.Create(Time);
            foreach (var resourceGeneratorType in ResourceGeneratorTypes)
            {
                var rg = ResourceGenerator.Create(rd, resourceGeneratorType, Time);
            }
        }
    }
}
