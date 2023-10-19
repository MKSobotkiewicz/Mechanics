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
        public int BuildingTime = 0;

        private ResourceDepot resourceDepot;
        private bool initialized = false;

        public static List<ResourceGenerator> AllResurceGenerators { get; private set; } = new List<ResourceGenerator>();
        public static ResourceValueList TotalProductionPerDay { get; private set; } = new ResourceValueList();
        public static ResourceValueList TotalCostPerDay { get; private set; } = new ResourceValueList();
        public static ResourceValueList TotalBuildCostPerDay { get; private set; } = new ResourceValueList();
        private static uint count = 0;

        public void Start()
        {
        }

        public static ResourceGenerator Create(ResourceDepot _resourceDepot, ResourceGeneratorType _resourceGeneratorType, uint size, Time.Time time, bool instant)
        {
            return new GameObject(_resourceGeneratorType.name + " " + count++.ToString()).AddComponent<ResourceGenerator>().Initialize(_resourceDepot, _resourceGeneratorType, size, time, instant);
        }

        public string GetName()
        {
            if (Size <= 1)
            {
                return ResourceGeneratorType.name;
            }
            return Size.ToString() + " " + ResourceGeneratorType.name + "s";
        }

        public ResourceValueList DailyProduction()
        {
            return Size * (ResourceGeneratorType.ProductionPerDay as ResourceValueList);
        }

        public ResourceValueList DailyCost()
        {
            return Size * (ResourceGeneratorType.CostPerDay as ResourceValueList);
        }

        public ResourceValueList DailyBuildCost()
        {
            return Size * (ResourceGeneratorType.BuildCostPerDay as ResourceValueList);
        }

        public string GetRemainingBuildTimeString()
        {
            return BuildingTime+"/" + ResourceGeneratorType.BuildingTime;
        }

        private ResourceGenerator Initialize(ResourceDepot _resourceDepot,ResourceGeneratorType resourceGeneratorType, uint size, Time.Time time,bool instant)
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
            if (time == null)
            {
                Debug.LogWarning(name + " Time is null");
                return null;
            }
            Size = size;
            initialized = true;
            resourceDepot = _resourceDepot;
            ResourceGeneratorType = resourceGeneratorType;
            transform.parent = resourceDepot.transform;
            time.AddDaily(this);
            AllResurceGenerators.Add(this);
            if (instant)
            {
                BuildingTime = 0;
                TotalProductionPerDay += ResourceGeneratorType.ProductionPerDay as ResourceValueList;
                TotalCostPerDay += ResourceGeneratorType.CostPerDay as ResourceValueList;
            }
            else
            {
                BuildingTime = resourceGeneratorType.BuildingTime;
                TotalCostPerDay += ResourceGeneratorType.BuildCostPerDay as ResourceValueList;
            }
            return this;
        }
        
        private bool Produce()
        {
            if (BuildingTime > 0)
            {
                if (!resourceDepot.CanSubstract(Size * (ResourceGeneratorType.BuildCostPerDay as ResourceValueList)))
                {
                    Debug.Log(name+" CAN'T SUBSTRACT");
                    return false;
                }
                resourceDepot.Substract(Size * (ResourceGeneratorType.BuildCostPerDay as ResourceValueList));
                BuildingTime--;
                return true;
            }
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

        public uint Priority()
        {
            return ResourceGeneratorType.Priority;
        }
    }
}
