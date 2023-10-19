using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Map
{
    public class ResourcesSpreader:MonoBehaviour
    {
        public List<Resources.NaturalResource> NaturalResources=new List<Resources.NaturalResource>();

        public void SpreadResources(List<Area> areas)
        {
            for (int i = 0; i < areas.Count; i++)
            {
                if (areas[i].ResourceGenerators.Count > 0)
                {
                    areas.RemoveAt(i);
                    i--;
                }
            }
            foreach (var naturalResource in NaturalResources)
            {
                var count = naturalResource.Count;
                while (count > 0)
                {
                    var area = Utility.ListUtilities.GetRandomObject(areas);
                    area.AddResourceGenerator(naturalResource.ResourceGeneratorType,1,true);
                    areas.Remove(area);
                    count--;
                }
            }
        }
    }
}
