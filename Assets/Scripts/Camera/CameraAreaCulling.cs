using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Camera
{
    public class CameraAreaCulling : MonoBehaviour
    {
        private new UnityEngine.Camera camera;
        private bool culled = false;

        void Start()
        {
            camera = GetComponent<UnityEngine.Camera>();
            float[] distances = new float[32];
            distances[LayerMask.NameToLayer("Area")] = 40000;
            distances[LayerMask.NameToLayer("Forests")] = 39000;
            camera.layerCullDistances = distances;
        }

        private void Update()
        {
            if (camera.fieldOfView > 4)
            {
                if (!culled)
                {
                    Debug.Log("culling");
                    culled = true;
                    float[] distances = new float[32];
                    distances[LayerMask.NameToLayer("Forests")] = 100;
                    camera.layerCullDistances = distances;
                }
            }
            else 
            {
                if (culled)
                {
                    Debug.Log("unculling");
                    culled = false;
                    float[] distances = new float[32];
                    distances[LayerMask.NameToLayer("Forests")] = 39000;
                    camera.layerCullDistances = distances;
                }
            }
        }
    }
}
