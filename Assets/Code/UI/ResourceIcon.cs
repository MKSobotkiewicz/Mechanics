using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class ResourceIcon : MonoBehaviour
    {
        public void Set(Resources.Resource resource)
        {
            var sr = GetComponentInChildren<SpriteRenderer>();
            if (sr == null)
            {
                Debug.LogError(name+" missing sprite renderer.");
            }
            sr.sprite = resource.Icon;
        }
    }
}
