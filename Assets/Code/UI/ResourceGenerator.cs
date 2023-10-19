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
        public Text InfoText;
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
                if (!shownResource.Resource.Unextracted)
                {
                    resourcesList.Add(Instantiate(ResourcePrefab, resourcesListRectTransform.transform).Init(shownResource.Resource, (long)shownResource.Value));
                }
            }
            foreach (var shownResource in resourceGenerator.DailyCost())
            {
                if (!shownResource.Resource.Unextracted)
                {
                    resourcesList.Add(Instantiate(ResourcePrefab, resourcesListRectTransform.transform).Init(shownResource.Resource, -(long)shownResource.Value));
                }
            }
            if (resourceGenerator.BuildingTime > 0)
            {
                foreach (var resource in resourcesList)
                {
                    resource.OverlayOn();
                }
                foreach (var shownResource in resourceGenerator.DailyBuildCost())
                {
                    if (!shownResource.Resource.Unextracted)
                    {
                        resourcesList.Add(Instantiate(ResourcePrefab, resourcesListRectTransform.transform).Init(shownResource.Resource, -(long)shownResource.Value));
                    }
                }
                InfoText.color = new Color(1, 1, 0, 1);
                InfoText.text = resourceGenerator.GetRemainingBuildTimeString() + "DAYS";
                return;
            }
            InfoText.color = new Color(0, 1, 0, 1);
            InfoText.text = "WORKING";
        }

        public void PauseClick()
        {
            if (resourceGenerator.BuildingTime > 0)
            {
                if (resourceGenerator.enabled)
                {
                    InfoText.color = new Color(1, 0.5f, 0.5f, 1);
                    InfoText.text = resourceGenerator.GetRemainingBuildTimeString()+ "DAYS";
                    resourceGenerator.enabled = false;
                    return;
                }
                InfoText.color = new Color(1, 1, 0, 1);
                InfoText.text = "BUILDING";
                resourceGenerator.enabled = true;
            }
            if (resourceGenerator.enabled)
            {
                InfoText.color = new Color(1, 0.5f, 0.5f, 1);
                InfoText.text = "PAUSED";
                foreach (var resource in resourcesList)
                {
                    resource.OverlayOn();
                }
                resourceGenerator.enabled = false;
                return;
            }
            Init(resourceGenerator);
            InfoText.color = new Color(0, 1, 0, 1);
            InfoText.text = "WORKING";
            resourceGenerator.enabled = true;
        }
    }
}
