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
        public Vector3 Position;
        public River River;

        private UnityEngine.Material material;
        private UI.Area areaUI;
        private int[] globeVertices;
        private Mesh globeMesh;
        private Mesh landformMesh;

        private static Area currentlySelectedArea;

        private static readonly string selected = "Vector1_4b18e5eff33c4158af523be6ab56aacd";
        private static readonly string accesibility = "Vector1_a4b5e65b775c4fda98dfbe45254e6f0a";
        private static readonly string color = "Color_3a1ee222bd634d87aa92e46c379001ab";

        private static readonly System.Random random = new System.Random();

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

        public void SetGlobeVertices(int[] vertices)
        {
            globeVertices = vertices;
        }

        public int[] GetGlobeVertices()
        {
            return globeVertices;
        }

        public void SetGlobeMesh(Mesh mesh)
        {
            globeMesh = mesh;
        }

        public void SetLandformMesh(Mesh mesh)
        {
            landformMesh = mesh;
        }

        public void SetLandformVerticesColor(Color color)
        {
            //WTF IS WRONG WITH MY BRAIN!!!
            //I will leave this part for some time here so to remember me of my retardness
            /*foreach (var vertice in globeVertices)
            {
                if (globeMesh.colors[vertice].b !=1)
                {
                    var colors = globeMesh.colors;
                    for (int i = 0; i < globeVertices.Length; i++)
                    {
                        colors[globeVertices[i]] = color;
                    }
                    globeMesh.SetColors(colors);
                }
            }*/
            if (landformMesh != null)
            {
                var count = landformMesh.vertices.Length;
                var landformMeshColor = new List<Color>();
                for (int i=0;i< count; i++)
                {
                    landformMeshColor.Add(color);
                }
                landformMesh.SetColors(landformMeshColor);
            }
        }

        public void SetNeighbours(List<Area> neighbours)
        {
            if (neighbours.Count != 6)
            {
                Debug.LogError("Area " +name+ " given wrong number of neighbours.");
            }
            Neighbours = neighbours;
        }

        public float Distance(Area area)
        {
            return Vector3.Distance(transform.localPosition, area.transform.localPosition);
        }

        public List<Area> GetNeighboursOfType(EType type)
        {
            var neighboursOfType = new List<Area>();
            foreach(var neighbour in Neighbours)
            {
                if (neighbour.Type== type)
                {
                    neighboursOfType.Add(neighbour);
                }
            }
            return neighboursOfType;
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

        public void SetType(EType type, MapGenerator mapGenerator)
        {
            Type = type;
            SetColor();
            AddAreaToMapGeneratorAreaLists(mapGenerator);
        }

        public void SetType(Color vertexColor, MapGenerator mapGenerator)
        {
            if (vertexColor.b == 1)
            {
                Type = EType.Water;
            }
            else if (vertexColor==new Color(1,0,0,1))
            {
                Type = EType.Mountains;
            }
            else if(vertexColor == new Color(0.5f, 0, 0, 1))
            {
                Type = EType.Hills;
            }
            else
            {
                Type = EType.Plains;
            }
            SetColor();
            AddAreaToMapGeneratorAreaLists(mapGenerator);
        }

        private void AddAreaToMapGeneratorAreaLists(MapGenerator mapGenerator)
        {
            switch (Type)
            {
                case EType.Water:
                    mapGenerator.WaterAreas.Add(this);
                    break;
                case EType.Mountains:
                    mapGenerator.MountainAreas.Add(this);
                    break;
                case EType.Hills:
                    mapGenerator.HillsAreas.Add(this);
                    break;
                case EType.Plains:
                    mapGenerator.PlainsAreas.Add(this);
                    break;
            }
        }

        private void SetMesh()
        {
            var worldToLocal = transform.worldToLocalMatrix;
            var doneNeighbous = new List<Area>();
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
            var mesh = GetComponentInChildren<MeshFilter>().mesh;
            if (points.Count > 5)
            {
                mesh.triangles = new int[] { 0, 1, 2, 5, 0, 2, 5, 2, 3, 3, 4, 5,
                                             2, 1, 0, 2, 0, 5, 3, 2, 5, 5, 4, 3};
                mesh.SetVertices(points);
                mesh.SetUVs(0,new List<Vector2> {new Vector2(0.067f,0.25f),
                                                 new Vector2(0.067f, 0.75f),
                                                 new Vector2(0.5f, 1f),
                                                 new Vector2(0.933f,0.75f),
                                                 new Vector2(0.933f,0.25f),
                                                 new Vector2(0.5f, 0f)});
                mesh.Optimize();
                mesh.RecalculateNormals();
                mesh.RecalculateTangents();
                mesh.RecalculateBounds();
            }
            else if (points.Count > 4)
            {
                mesh.triangles = new int[] { 0, 1, 2, 4, 0, 2, 4, 2, 3,
                                             2, 1, 0, 2, 0, 4, 3, 2, 4};
                mesh.SetVertices(points);
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
            var collider = GetComponentInChildren<MeshCollider>();
            collider.sharedMesh = mesh;
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
                case EType.Hills:
                case EType.Plains:
                    material.SetColor(color, PlainsTilesColor);
                    material.SetFloat(accesibility, 0);
                    return;
            }
        }

        public enum EType
        {
            Water=0,
            Plains=1,
            Hills = 2,
            Mountains =3
        }
    }
}
