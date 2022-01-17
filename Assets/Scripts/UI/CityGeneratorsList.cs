using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class CityGeneratorsList : MonoBehaviour
    {
        public Canvas List;
        public ResourceGenerator ResourceGeneratorPrefab;

        private List<ResourceGenerator> resourceGenerators=new List<ResourceGenerator>();

        public void Init(Map.Area area)
        {
            foreach (var resourceGenerator in resourceGenerators)
            {
                Destroy(resourceGenerator.gameObject);
            }
            resourceGenerators.Clear();
            foreach (var areaResourceGenerator in area.ResourceGenerators)
            {
                var resourceGenerator = Instantiate(ResourceGeneratorPrefab,List.transform);
                resourceGenerator.Init(areaResourceGenerator);
                resourceGenerators.Add(resourceGenerator);
            }
        }
    }
}
