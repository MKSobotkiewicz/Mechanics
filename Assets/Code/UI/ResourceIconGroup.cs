using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class ResourceIconGroup : MonoBehaviour
    {
        public int MaxWidth = 3;
        public float IconSize = 10;
        public UnityEngine.Material Material;

        private List<ResourceIcon> ResourceIcons=new List<ResourceIcon>();

        public void AddResourceIcon(Resources.Resource resource)
        {
            var rigo = new GameObject(resource.name, typeof(ResourceIcon),typeof(SpriteRenderer));
            rigo.transform.parent = transform;
            rigo.transform.localScale = new Vector3(IconSize, IconSize, IconSize);
            rigo.transform.localPosition = new Vector3(ResourceIcons.Count% MaxWidth, 10, ResourceIcons.Count/ MaxWidth);
            rigo.transform.LookAt(new Vector3());
            var risr= rigo.GetComponent<SpriteRenderer>();
            risr.material = Material;
            var ri = rigo.GetComponent<ResourceIcon>();
            ResourceIcons.Add(ri);
            ri.Set(resource);
        }
    }
}
