using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Map
{
    public class River
    {
        public List<Area> Areas = new List<Area>();

        private readonly River riverItConnectsTo = null;
        private Spline.Spline spline;

        private static readonly System.Random random = new System.Random();
        private static int riverCount = 0;

        private static readonly List<River> allRivers = new List<River>();

        public River(Area start,int maxLenght, Transform parent, UnityEngine.Material material)
        {
            var current = start;
            Areas.Add(current);
            while (maxLenght > 0)
            {
                var next = GetNext(current);
                if (next == null)
                {
                    break;
                }
                if (next.River != null&&next.Type!=Area.EType.Water)
                {
                    riverItConnectsTo = next.River;
                    var index = next.River.Areas.IndexOf(next);
                    var rest = next.River.Areas.GetRange(index, next.River.Areas.Count - index);
                    Areas.AddRange(rest);
                    CreateMesh(parent, material);
                    allRivers.Add(this);
                    return;
                }
                Areas.Add(next);
                current = next;
                current.River = this;
                maxLenght--;
                if (current.Type == Area.EType.Water)
                {
                    if (Areas.Count < 5)
                    {
                        Fail();
                        return;
                    }
                    CreateMesh(parent, material);
                    allRivers.Add(this);
                    return;
                }
            }
            if (Areas.Last().Type != Area.EType.Water|| (Areas.Count<3))
            {
                Fail();
            }
        }

        public static void OptimizeAllRivers()
        {
            var masterSpline = new GameObject("AllRivers");
            var mf=masterSpline.AddComponent<MeshFilter>();
            var mr=masterSpline.AddComponent<MeshRenderer>();
            mr.material = allRivers[0].spline.GetComponent<MeshRenderer>().material;
            var combine = new CombineInstance[allRivers.Count];
            for (int i=0;i<allRivers.Count;i++)
            {
                combine[i].mesh= allRivers[i].spline.GetComponent<MeshFilter>().mesh;
                combine[i].transform = allRivers[i].spline.transform.localToWorldMatrix;
            }
            mf.mesh = new Mesh();
            mf.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mf.mesh.CombineMeshes(combine);
            foreach (var river in allRivers)
            {
                GameObject.Destroy(river.spline);
            }
        }

        private void Fail()
        {
            foreach (var area in Areas)
            {
                if (area.River == this)
                {
                    area.River = null;
                }
            }
            throw new Exception("Failed at creating river.");
        }

        private void CreateMesh(Transform parent, UnityEngine.Material material)
        {
            spline = Spline.Spline.CreateSpline(Areas,parent, material, "River " + riverCount++);
            /*int nodeCount = 0;
            spline = (new GameObject("River "+ riverCount++)).AddComponent<Spline.Spline>();
            spline.transform.parent = parent;
            for (int i=1;i<Areas.Count;i++)
            {
                var node=(new GameObject("Node "+ nodeCount++)).AddComponent<Spline.SplineNode>();
                node.transform.parent = spline.transform;
                node.transform.position = Areas[i].Position;
            }
            spline.Generate(30,10,1,true);
            spline.SetMaterial(material);*/
        }

        private Area GetNext(Area currentArea)
        {
            var possibleAreas = new List<Area>();
            int i = 0;
            if (currentArea.Type == Area.EType.Mountains)
            {
                i = (int)currentArea.Type-1;
            }
            else
            {
                i = (int)currentArea.Type;
            }
            while (i >= 0)
            {
                possibleAreas.AddRange((currentArea.GetNeighboursOfType((Area.EType)i)).Except(Areas).ToList());
                i--;
            }
            if (possibleAreas.Count > 0)
            {
                return possibleAreas[random.Next(possibleAreas.Count - 1)];
            }
            return null;
        }
    }
}
