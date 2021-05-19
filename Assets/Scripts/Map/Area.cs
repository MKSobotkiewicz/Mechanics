using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Map
{
    public class Area : MonoBehaviour,Utility.IClicable
    {
        public List<Area> Neighbours = new List<Area>();
        public EType Type;
        public Color WaterTilesColor;
        public Color MountainTilesColor;
        public Color PlainsTilesColor;
        public UI.Area AreaUIPrefab;

        private UnityEngine.Material material;
        private UI.Area areaUI;

        private static Area currentlySelectedArea;

        private static readonly string selected = "Vector1_4b18e5eff33c4158af523be6ab56aacd";
        private static readonly string accesibility = "Vector1_a4b5e65b775c4fda98dfbe45254e6f0a";
        private static readonly string color = "Color_3a1ee222bd634d87aa92e46c379001ab";

        public void Awake()
        {
            material = GetComponentInChildren<MeshRenderer>().material;
        }

        public void Initialize(List<Area> areas)
        {
            if (Neighbours.Count == 0)
            {
                Debug.LogWarning(name+" is missing neighbours");
                GetNeighbours(areas);
            }
            SetMesh();
        }

        public void Click()
        {
            Select();
        }

        public void Select()
        {
            if (currentlySelectedArea == this)
            {
                return;
            }
            Debug.Log(name + " selected");
            material.SetFloat(selected, 1f);
            var canvas = UnityEngine.Camera.main.GetComponentInChildren<Canvas>();
            areaUI=Instantiate(AreaUIPrefab,canvas.transform);
            areaUI.SetName(name);
            if (currentlySelectedArea != null)
            {
                currentlySelectedArea.Unselect();
            }
            currentlySelectedArea = this;
        }

        public void Unselect()
        {
            UnityEngine.Debug.Log(name+" unselected");
            material.SetFloat(selected, 0f);
            if (areaUI != null)
            {
                areaUI.Destroy();
            }
        }

        public void SetNeighbours(List<Area> neighbours)
        {
            if (neighbours.Count != 6)
            {
                Debug.LogError("Area " +name+ " given wrong number of neighbours.");
            }
            Neighbours = neighbours;
            /*for(int i=0; i< Neighbours.Count; i++)
            {
                var distance = Distance(Neighbours[i]);
                Debug.Log(distance);
                if (distance > 0.035f)
                {
                    Neighbours.RemoveAt(i);
                }
            }*/
        }

        public float Distance(Area area)
        {
            return Vector3.Distance(transform.localPosition, area.transform.localPosition);
        }

        private void GetNeighbours(List<Area> areas)
        {
            var closest = new Dictionary<Area, float>();
            foreach (var area in areas)
            {
                if (area == this)
                {
                    continue;
                }
                var distance = Distance(area);
                if (distance < 0.1)
                {
                    closest.Add(area, distance);
                }
            }
            var sortedClosest = from entry in closest orderby entry.Value ascending select entry;
            int i = 0;
            foreach(var area in sortedClosest)
            {
                i++;
                if (i > 6)
                {
                    break;
                }
                Neighbours.Add(area.Key);
            }
        }

        public void SetType(EType type)
        {
            Type = type;
            SetColor();
        }

        public void SetType(Color vertexColor)
        {
            if (vertexColor.b == 1)
            {
                Type = EType.Water;
                SetColor();
                return;
            }
            if (vertexColor==new Color(1,0,0,1))
            {
                Type = EType.Mountains;
                SetColor();
                return;
            }
            Type = EType.Plains;
            SetColor();
            return;
        }

        private void SetMesh()
        {
            /*transform.LookAt(Neighbours[0].transform, transform.position);
            List<float> distances = new List<float>();
            for (int i = 0; i < 6; i++)
            {
                distances.Add(Vector3.Distance(Neighbours[i].transform.position, transform.position) / 1350000);
            }
            var scale = distances.Max();
            transform.localScale = new Vector3(scale, scale, scale);*/

            var worldToLocal = transform.worldToLocalMatrix;
            var doneNeighbous = new List<Area>();
            //var points = new SortedList<float,Vector3>();
            var points = new List< Vector3>();
            var current = Neighbours[0];
            for (int j=0;j<6;j++)
            {
                for (int i = 0; i < current.Neighbours.Count; i++)
                {
                    if (Neighbours.Contains(current.Neighbours[i]) && (!doneNeighbous.Contains(current.Neighbours[i])||(current.Neighbours[i] == Neighbours[0]&&j>3)))
                    {
                        var point = (transform.position + current.transform.position + current.Neighbours[i].transform.position) / 3;
                        points.Add(worldToLocal.MultiplyPoint3x4(point));

                        doneNeighbous.Add(current);
                        current = current.Neighbours[i];
                        break;
                    }
                }
                if (points.Count > 5)
                {
                    break;
                }
            }
            /*foreach (var neighbour1 in Neighbours)
            {
                foreach (var neighbour2 in neighbour1.Neighbours)
                {
                    if (Neighbours.Contains(neighbour2)&& !doneNeighbous.Contains(neighbour2))
                    {
                        var point = (transform.position + neighbour1.transform.position + neighbour2.transform.position) / 3;
                        var pointToCenter = point + transform.position;
                        var theta = System.Math.Atan(System.Math.Sqrt(pointToCenter.x * pointToCenter.x + pointToCenter.z * pointToCenter.z) / pointToCenter.y);
                        var phi = System.Math.Atan(pointToCenter.z/ pointToCenter.x);
                        points.Add(/*(float)(phi* theta), worldToLocal.MultiplyPoint3x4(point));
                    }
                }
                doneNeighbous.Add(neighbour1);
            }*/
            var mesh = GetComponentInChildren<MeshFilter>().mesh;
            //var p = points.Values.ToList();
            //mesh.SetVertices(new List<Vector3>{p[0],p[1], p[2], p[3], p[4], p[5]});
            if (points.Count > 5)
            {
                //mesh.triangles=new int[]{0,1,2,2,3,4,4,5,0,0,2,4};
                mesh.triangles = new int[] { 0, 1, 2, 5, 0, 2, 5, 2, 3, 3, 4, 5,
                                             2, 1, 0, 2, 0, 5, 3, 2, 5, 5, 4, 3};
                mesh.SetVertices(points);
                mesh.SetUVs(0,new List<Vector2> {new Vector2(0.067f,0.25f),
                                                 new Vector2(0.067f, 0.75f),
                                                 new Vector2(0.5f, 1f),
                                                 new Vector2(0.933f,0.75f),
                                                 new Vector2(0.933f,0.25f),
                                                 new Vector2(0.5f, 0f)});
                /*mesh.SetUVs(0,new List<Vector2> {new Vector2(0.067f,0.25f),
                                                 new Vector2(0.067f, 0.75f),
                                                 new Vector2(0.5f, 1f),
                                                 new Vector2(0.933f,0.25f),
                                                 new Vector2(0.933f,0.75f),
                                                 new Vector2(0.5f, 0f)});*/
                //mesh.SetNormals(new List<Vector3> { points[0].normalized, points[1].normalized, points[2].normalized, points[3].normalized, points[4].normalized });
                mesh.Optimize();
                mesh.RecalculateNormals();
                mesh.RecalculateTangents();
                mesh.RecalculateBounds();
            }
            else if (points.Count > 4)
            {
                //mesh.triangles=new int[]{0,1,2,2,3,4,4,5,0,0,2,4};
                mesh.triangles = new int[] { 0, 1, 2, 4, 0, 2, 4, 2, 3,
                                             2, 1, 0, 2, 0, 4, 3, 2, 4};
                mesh.SetVertices(points);
                //mesh.SetNormals(new List<Vector3> { points[0].normalized, points[1].normalized, points[2].normalized, points[3].normalized, points[4].normalized, points[5].normalized });
                mesh.RecalculateNormals();
                mesh.RecalculateTangents();
                mesh.RecalculateBounds();
            }
            else
            {
                Debug.LogWarning("Area " + name + " failed to create mesh.");
                var temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                temp.transform.position = transform.position;
                temp.transform.parent = transform;
                temp.transform.localScale *= 100;
            }
            var collider = GetComponentInChildren<SphereCollider>();
            collider.radius = 140f;
            //transform.localScale /= 10000;
            //transform.rotation=new Quaternion(0,0,0,0);
            /*foreach (var point in points)
            {
                var temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                temp.transform.position = point;
                temp.transform.parent = transform;
                temp.transform.localScale *= 10;
            }*/
        }

        private void SetColor()
        {
            switch (Type)
            {
                case EType.Water:
                    material.SetColor(color, WaterTilesColor);
                    material.SetFloat(accesibility, 1);
                    return;
                case EType.Mountains:
                    material.SetColor(color, MountainTilesColor);
                    material.SetFloat(accesibility, 1);
                    return;
                case EType.Plains:
                    material.SetColor(color, PlainsTilesColor);
                    material.SetFloat(accesibility, 0);
                    return;
            }
        }

        public enum EType
        {
            Water,
            Plains,
            Mountains
        }
    }
}
