using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Camera
{
    public class CameraAreaCulling : MonoBehaviour
    {
        private new UnityEngine.Camera camera;

        void Start()
        {
            camera = GetComponent<UnityEngine.Camera>();
            float[] distances = new float[32];
            distances[LayerMask.NameToLayer("Area")] = 40000;
            camera.layerCullDistances = distances;
        }

        private void Update()
        {
            //float[] distances = new float[32];
            //distances[LayerMask.NameToLayer("Area")] = (100-camera.fieldOfView)*500;
            //camera.layerCullDistances = distances;
        }
    }
}
