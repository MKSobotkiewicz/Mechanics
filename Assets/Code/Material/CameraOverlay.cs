using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Material
{
    public class CameraOverlay : MonoBehaviour
    {
        public float DropTime = 1;
        public float BaseJitter = 1;

        private UnityEngine.Material material;
        private float dropTimer;

        public void Start()
        {
            var volume= GetComponent<UnityEngine.Rendering.HighDefinition.CustomPassVolume>();
            if (volume == null)
            {
                Debug.LogError(name+" missing Custom Pass Volume");
            }
            material = (volume.customPasses[0] as UnityEngine.Rendering.HighDefinition.FullScreenCustomPass).fullscreenPassMaterial;
            if (material == null)
            {
                Debug.LogError(name + " missing Custom Pass Volume Full Screen Custom Pass Material");
            }
        }

        public void Update()
        {
            if (dropTimer > 0)
            {
                dropTimer -= UnityEngine.Time.deltaTime;
                if (dropTimer <= 0)
                {
                    dropTimer = 0;
                }
                material.SetFloat("_StripesStrength", (dropTimer/DropTime)+ BaseJitter);
            }
        }

        public void SetJitter(float value,float time)
        {
            DropTime = time;
            value = Mathf.Clamp01(value);
            dropTimer = value * DropTime;
        }

        public void SetJitter(float value)
        {
            value = Mathf.Clamp01(value);
            dropTimer = value * DropTime;
        }
    }
}
