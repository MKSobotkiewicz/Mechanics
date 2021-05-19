using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Resources
{
    public class ResourceDepot : MonoBehaviour, Time.IDaily
    {
        public List<ResourceValue> resources = new Resources();

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

        public int Priority()
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
            Time.Dailies.Add(rd);
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

        public void Add(Resources _resources)
        {
            foreach (var resource in _resources)
            {
                Add(resource.Copy());
            }
        }

        public bool Substract(Resources _resources)
        {
            foreach (var resource in _resources)
            {
                if (!CanSubstract(resource))
                {
                    return false;
                }
            }
            foreach (var resource in _resources)
            {
                Substract(resource);
            }
            return true;
        }

        public bool CanSubstract(Resources _resources)
        {
            foreach (var resource in _resources)
            {
                if (!CanSubstract(resource))
                {
                    return false;
                }
            }
            return true;
        }

        public float CanSubstractPercentage(Resources _resources)
        {
            float percentage = 1;
            foreach (var resource in _resources)
            {
                var newPercentage = CanSubstractPercentage(resource);
                if (newPercentage< percentage)
                {
                    percentage = newPercentage;
                }
            }
            return percentage;
        }

        public float SubstractAsMuchAsPossible(Resources _resources)
        {
            float percentage = 1;
            foreach (var resource in _resources)
            {
                var newPercentage = CanSubstractPercentage(resource);
                if (newPercentage < percentage)
                {
                    percentage = newPercentage;
                }
            }
            Resources maxResurces = new Resources();
            foreach(var resource in _resources)
            {
                maxResurces.Add(percentage * resource);
            }
            Substract(maxResurces);
            return percentage;
        }
    }
}
