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
        public Color Color;

        public void Start()
        {
        }

        public void Initialize(string name)
        {
        }

        public Resources.ResourceValueList GetDailyResourceCosts()
        {
            var resourceCosts = new Resources.ResourceValueList();
            foreach (var resourceGenerator in OwnedResourceGenerators)
            {
                foreach (var resourceValue in resourceGenerator.ResourceGeneratorType.CostPerDay)
                {
                    resourceCosts.Add(resourceValue.Copy());
                }
            }
            return resourceCosts;
        }

        public Resources.ResourceValueList GetDailyGrossResourceProduction()
        {
            var resourceCosts = new Resources.ResourceValueList();
            foreach (var resourceGenerator in OwnedResourceGenerators)
            {
                foreach (var resourceValue in resourceGenerator.ResourceGeneratorType.ProductionPerDay)
                {
                    resourceCosts.Add(resourceValue.Copy());
                }
            }
            return resourceCosts;
        }

        public Resources.ResourceValueList GetDailyNetResourceProduction()
        {
            return GetDailyGrossResourceProduction() - GetDailyResourceCosts();
        }
    }
}
