using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Utility
{
    public class LightDirection : MonoBehaviour
    {
        private Transform mainLight;
        private UnityEngine.Material material;

        private static readonly string propertyName = "LightDirection";

        void Start()
        {
            var mr = GetComponent<MeshRenderer>();
            if (mr != null)
            {
                material = mr.material;
            }
            else
            {
                var psr = GetComponentInChildren<ParticleSystemRenderer>();
                if (psr != null)
                {
                    material = psr.material;
                }
                else
                {
                    Debug.LogError(name+ " missing both MeshRenderer and ParticleSystemRenderer.");
                }
            }
            foreach (var go in GameObject.FindGameObjectsWithTag("MainLight"))
            {
                var light = go.GetComponent<Light>();
                if (light != null)
                {
                    mainLight = light.transform;
                    return;
                }
            }
            Debug.LogError("Missing Main Light.");
        }

        void Update()
        {
            material.SetVector(propertyName, mainLight.position);
        }
    }
}