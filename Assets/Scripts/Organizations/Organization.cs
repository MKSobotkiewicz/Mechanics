using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Organizations
{
    public class Organization : MonoBehaviour
    {
        public List<Resources.ResourceGenerator> OwnedResourceGenerators;

        public void Start()
        {
        }

        public void Initialize(string name)
        {
        }

        public List<Resources.ResourceValue> GetDailyResourceCosts()
        {
            var resourceCosts = new List<Resources.ResourceValue>();
            foreach (var resourceGenerator in OwnedResourceGenerators)
            {
                foreach (var resourceValue in resourceGenerator.resourceGeneratorType.CostPerDay)
                {
                    resourceCosts.Add(resourceValue.Copy());
                }
            }
            return resourceCosts;
        }

        public List<Resources.ResourceValue> GetDailyResourceProduction()
        {
            var resourceCosts = new List<Resources.ResourceValue>();
            foreach (var resourceGenerator in OwnedResourceGenerators)
            {
                foreach (var resourceValue in resourceGenerator.resourceGeneratorType.ProductionPerDay)
                {
                    resourceCosts.Add(resourceValue.Copy());
                }
            }
            return resourceCosts;
        }
    }
}
