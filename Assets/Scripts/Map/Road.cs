using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Map
{
    public class Road
    {
        public List<Area> Areas = new List<Area>();
        
        private Spline.Spline spline;

        private static readonly System.Random random = new System.Random();
        private static int roadCount = 0;

        private static readonly List<Road> allRoads = new List<Road>();

        public Road(Area start, Area target, Transform parent, UnityEngine.Material material)
        {
            //Debug.Log("looking for path");
            Areas = Utility.Pathfinder.FindPath(start,target);
            if (Areas == null)
            {
                Debug.Log("no path");
                Fail();
            }
            CreateMesh(parent, material);
            allRoads.Add(this);
            foreach (var area in Areas)
            {
                area.Road = true;
            }
        }

        public static void OptimizeAllRoads()
        {
            if (allRoads.Count == 0)
            {
                Debug.LogWarning("No roads to optimize.");
                return;
            }
            var masterSpline = new GameObject("AllRoads");
            var mf = masterSpline.AddComponent<MeshFilter>();
            var mr = masterSpline.AddComponent<MeshRenderer>();
            mr.material = allRoads[0].spline.GetComponent<MeshRenderer>().material;
            var combine = new CombineInstance[allRoads.Count];
            for (int i = 0; i < allRoads.Count; i++)
            {
                combine[i].mesh = allRoads[i].spline.GetComponent<MeshFilter>().mesh;
                combine[i].transform = allRoads[i].spline.transform.localToWorldMatrix;
            }
            mf.mesh = new Mesh();
            mf.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mf.mesh.CombineMeshes(combine);
            foreach (var road in allRoads)
            {
                GameObject.Destroy(road.spline);
            }
        }

        private void Fail()
        {
            throw new Exception("Failed at creating road.");
        }

        private void CreateMesh(Transform parent, UnityEngine.Material material)
        {
            spline = Spline.Spline.CreateSpline(Areas, parent, material, "Road " + roadCount++, 2, 10, 0, false,0);
            /*int nodeCount = 0;
            spline = (new GameObject("Road " + roadCount++)).AddComponent<Spline.Spline>();
            spline.transform.parent = parent;
            for (int i = 0; i < Areas.Count; i++)
            {
                var node = (new GameObject("Node " + nodeCount++)).AddComponent<Spline.SplineNode>();
                node.transform.parent = spline.transform;
                node.transform.position = Areas[i].Position;
            }
            spline.Generate(2, 10, 0);
            spline.SetMaterial(material);*/
        }
    }
}