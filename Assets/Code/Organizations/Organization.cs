using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Organizations
{
    public class Organization : MonoBehaviour
    {
        public List<Organization> Enemies;
        public List<Resources.ResourceGenerator> OwnedResourceGenerators;
        public List<Map.City> OwnedCities=new List<Map.City>();
        public Color Color;
        public Texture2D Flag;
        public FlagsGenerator FlagsGenerator;

        public static List<Organization> AllOrganizations { get; private set; } = new List<Organization>();

        public void Start()
        {
            if (FlagsGenerator!= null)
            {
                Flag = FlagsGenerator.Generate();
            }
        }

        public void Initialize(string name, Texture2D flag)
        {
            gameObject.name = name;
            Flag = flag;
            AllOrganizations.Add(this);
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
