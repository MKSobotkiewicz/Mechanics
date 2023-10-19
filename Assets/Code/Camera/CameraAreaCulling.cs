using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Camera
{
    public class CameraAreaCulling : MonoBehaviour
    {
        public RotatingCamera RotatingCamera;

        private new UnityEngine.Camera camera;
        private bool culled = false;
        private float distanceClippingPlaneDiffrence;
        private float baseZ;

        private static readonly int AreasCulling = 40000;
        private static readonly int ForestsCulling = 39000;
        private static readonly int StarsCulling = 1000000;

        void Start()
        {
            camera = GetComponent<UnityEngine.Camera>();
            var defaultValue= -camera.transform.localPosition.z;
            var distances = Enumerable.Repeat(defaultValue, 32).ToArray();
            //distances[LayerMask.NameToLayer("Area")] = AreasCulling;
            //distances[LayerMask.NameToLayer("Forests")] = ForestsCulling;
            distances[LayerMask.NameToLayer("Stars")] = StarsCulling;
            camera.layerCullDistances = distances;
            //camera.farClipPlane = defaultValue;
        }

        private void Update()
        {
            var defaultValue = -camera.transform.localPosition.z;
            var distances = Enumerable.Repeat(defaultValue, 32).ToArray();
            //distances[LayerMask.NameToLayer("Area")] = AreasCulling;
            //distances[LayerMask.NameToLayer("Forests")] = ForestsCulling;
            distances[LayerMask.NameToLayer("Stars")] = StarsCulling;
            camera.layerCullDistances = distances;
            //camera.farClipPlane = defaultValue;
            /*if (camera.fieldOfView > 4)
            {
                if (!culled)
                {
                    Debug.Log("culling");
                    culled = true;
                    float[] distances = new float[32];
                    distances[LayerMask.NameToLayer("Area")] = AreasCulling;
                    distances[LayerMask.NameToLayer("Stars")] = StarsCulling;
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
                    distances[LayerMask.NameToLayer("Area")] = AreasCulling;
                    distances[LayerMask.NameToLayer("Forests")] = ForestsCulling;
                    distances[LayerMask.NameToLayer("Stars")] = StarsCulling;
                    camera.layerCullDistances = distances;
                }
            }*/
        }
    }
}
