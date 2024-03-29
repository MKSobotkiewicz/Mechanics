﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Resources
{
    public class ResourceDepot : MonoBehaviour, Time.IDaily
    {
        public List<ResourceValue> resources = new ResourceValueList();

        private static uint count = 0;

        public void DailyUpdate()
        {
            foreach (var resource in resources)
            {
                if (resource.Resource.Volatile)
                {
                    resource.Value = 0;
                }
            }
        }

        public uint Priority()
        {
            return 0;
        }

        public void Add(ResourceValue resource)
        {
            if (resources.Exists(x => x.Resource == resource.Resource))
            {
                resources.Find(x => x.Resource == resource.Resource).Value += resource.Value;
                return;
            }
            resources.Add(resource.Copy());
        }

        public static ResourceDepot Create(Time.Time Time)
        {
            var rd = new GameObject("ResourceDepot " + count++.ToString()).AddComponent<ResourceDepot>() as ResourceDepot;
            Time.AddDaily(rd);
            return rd;
        }

        public bool Substract(ResourceValue resource)
        {
            if (resources.Exists(x => x.Resource == resource.Resource))
            {
                var container = resources.Find(x => x.Resource == resource.Resource);
                if (container.Value > resource.Value)
                {
                    container.Value -= resource.Value;
                    return true;
                }
                if (container.Value == resource.Value)
                {
                    resources.Remove(container);
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool CanSubstract(ResourceValue resource)
        {
            if (resources.Exists(x => x.Resource == resource.Resource))
            {
                var container = resources.Find(x => x.Resource == resource.Resource);
                if (container.Value >= resource.Value)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public float CanSubstractPercentage(ResourceValue resource)
        {
            if (resources.Exists(x => x.Resource == resource.Resource))
            {
                var container = resources.Find(x => x.Resource == resource.Resource);
                if (container.Value >= resource.Value)
                {
                    return 1;
                }
                return ((float)container.Value)/ ((float)resource.Value);
            }
            return 1;
        }

        public void Add(ResourceValueList resourceValueList)
        {
            foreach (var resource in resourceValueList)
            {
                Add(resource.Copy());
            }
        }

        public bool Substract(ResourceValueList resourceValueList)
        {
            foreach (var resource in resourceValueList)
            {
                if (!CanSubstract(resource))
                {
                    return false;
                }
            }
            foreach (var resource in resourceValueList)
            {
                Substract(resource);
            }
            return true;
        }

        public bool CanSubstract(ResourceValueList resourceValueList)
        {
            foreach (var resource in resourceValueList)
            {
                if (!CanSubstract(resource))
                {
                    return false;
                }
            }
            return true;
        }

        public float CanSubstractPercentage(ResourceValueList resourceValueList)
        {
            float percentage = 1;
            foreach (var resource in resourceValueList)
            {
                var newPercentage = CanSubstractPercentage(resource);
                if (newPercentage< percentage)
                {
                    percentage = newPercentage;
                }
            }
            return percentage;
        }

        public float SubstractAsMuchAsPossible(ResourceValueList resourceValueList)
        {
            float percentage = 1;
            foreach (var resource in resourceValueList)
            {
                var newPercentage = CanSubstractPercentage(resource);
                if (newPercentage < percentage)
                {
                    percentage = newPercentage;
                }
            }
            ResourceValueList maxResurces = new ResourceValueList();
            foreach(var resource in resourceValueList)
            {
                maxResurces.Add(percentage * resource);
            }
            Substract(maxResurces);
            return percentage;
        }
    }
}
