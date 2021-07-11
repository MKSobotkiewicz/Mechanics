using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Resources
{
    public class ResourceGenerator : MonoBehaviour, Time.IDaily
    {
        public uint Size = 1;
        public float Capacity = 1;
        public ResourceGeneratorType ResourceGeneratorType { get; private set; }

        private ResourceDepot resourceDepot;
        private bool initialized = false;

        public static List<ResourceGenerator> AllResurceGenerators { get; private set; } = new List<ResourceGenerator>();
        public static ResourceValueList TotalProductionPerDay { get; private set; } = new ResourceValueList();
        public static ResourceValueList TotalCostPerDay { get; private set; } = new ResourceValueList();
        private static uint count=0;

        public void Start()
        {
        }

        public static ResourceGenerator Create(ResourceDepot _resourceDepot, ResourceGeneratorType _resourceGeneratorType, Time.Time Time)
        {
            return new GameObject(_resourceGeneratorType.name+" " + count++.ToString()).AddComponent<ResourceGenerator>().Initialize(_resourceDepot, _resourceGeneratorType,Time);
        }

        private ResourceGenerator Initialize(ResourceDepot _resourceDepot,ResourceGeneratorType resourceGeneratorType, Time.Time Time)
        {
            if (resourceGeneratorType==null)
            {
                Debug.LogWarning(name + " ResourceGeneratorType is null");
                return null;
            }
            if (_resourceDepot == null)
            {
                Debug.LogWarning(name + " ResourceDepot is null");
                return null;
            }
            if (Time == null)
            {
                Debug.LogWarning(name + " Time is null");
                return null;
            }
            initialized = true;
            resourceDepot = _resourceDepot;
            ResourceGeneratorType = resourceGeneratorType;
            transform.parent = resourceDepot.transform;
            Time.AddDaily(this);
            AllResurceGenerators.Add(this);
            TotalProductionPerDay += ResourceGeneratorType.ProductionPerDay as ResourceValueList;
            TotalCostPerDay += ResourceGeneratorType.CostPerDay as ResourceValueList;
            return this;
        }

        private bool Produce()
        {
            /*
            if (resourceDepot.Substract(resourceGeneratorType.CostPerDay))
            {
                resourceDepot.Add(resourceGeneratorType.ProductionPerDay);
                return true;
            }
            return false;*/
            var capacity=resourceDepot.SubstractAsMuchAsPossible(Size*(ResourceGeneratorType.CostPerDay as ResourceValueList));
            resourceDepot.Add(capacity* Size*(ResourceGeneratorType.ProductionPerDay as ResourceValueList));
            Capacity = capacity;
            return true;
        }

        public void DailyUpdate()
        {
            if (!initialized)
            {
                Debug.LogError(name+" not initialized");
                return;
            }
            if (!enabled)
            {
                return;
            }
            Produce();
        }

        public int Priority()
        {
            return ResourceGeneratorType.Priority;
        }
    }
}
