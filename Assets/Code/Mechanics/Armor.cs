using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class Armor : MonoBehaviour
    {
        private static List<MeshRenderer> allArmorMeshRenderers = new List<MeshRenderer>();
        private static bool visible = true;

        public void Start()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogError(name + " missing MeshRenderer");
            }
            allArmorMeshRenderers.Add(meshRenderer);
        }

        public static void SwitchArmorMaterialsVisibility()
        {
            if (visible)
            {
                foreach(var meshRenderer in allArmorMeshRenderers)
                {
                    meshRenderer.enabled = false;
                }
                visible = false;
            }
            else
            {
                foreach (var meshRenderer in allArmorMeshRenderers)
                {
                    meshRenderer.enabled = true;
                }
                visible = true;
            }

        }
    }
}
