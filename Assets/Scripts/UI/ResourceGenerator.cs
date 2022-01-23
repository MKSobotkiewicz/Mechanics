using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class ResourceGenerator : MonoBehaviour
    {
        public Text Name;
        public RectTransform resourcesListRectTransform;
        public Resource ResourcePrefab;

        private Resources.ResourceGenerator resourceGenerator;
        private List<Resource> resourcesList = new List<Resource>();

        public void Init(Resources.ResourceGenerator _resourceGenerator)
        {
            for (int i=0;i< resourcesList.Count;i++)
            {
                Destroy(resourcesList[i].gameObject);
            }
            resourcesList.Clear();
            resourceGenerator = _resourceGenerator;
            Name.text = resourceGenerator.GetName().ToUpper();
            foreach (var shownResource in resourceGenerator.DailyProduction())
            {
                resourcesList.Add(Instantiate(ResourcePrefab, resourcesListRectTransform.transform).Init(shownResource.Resource, (long)shownResource.Value));
            }
            foreach (var shownResource in resourceGenerator.DailyCost())
            {
                resourcesList.Add(Instantiate(ResourcePrefab, resourcesListRectTransform.transform).Init(shownResource.Resource, -(long)shownResource.Value));
            }
        }
    }
}
