﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Map
{
    public class MapGenerator : MonoBehaviour
    {
        public GameObject AreaPrefab;
        public List<Area> Areas = new List<Area>();
        public List<Area> WaterAreas = new List<Area>();
        public List<Area> MountainAreas = new List<Area>();
        public List<Area> HillsAreas = new List<Area>();
        public List<Area> PlainsAreas = new List<Area>();
        public List<River> Rivers = new List<River>();
        public MeshFilter MeshFilter;
        public Globe.SnowMovement SnowMovement;
        public UnityEngine.Material RiverMaterial;
        public ComputeShader DistanceShader;
        public ComputeShader NeighbourShader;
        public List<GameObject> Mountains;
        public List<GameObject> Hills;

        private static readonly System.Random random = new System.Random();

        public void Start()
        {
            Debug.Log(name + " creating climate, time: " + UnityEngine.Time.realtimeSinceStartup);
            var PlanetConditions=Globe.PlanetConditions.GenerateRandom();
            Debug.Log(name + " creating areas, time: " + UnityEngine.Time.realtimeSinceStartup);
            CreateAreas();
            Debug.Log(name + " setting areas neighbour, time: " + UnityEngine.Time.realtimeSinceStartup);
            SetAreasNeighbours();
            Debug.Log(name + " setting areas types, time: " + UnityEngine.Time.realtimeSinceStartup);
            SetAreasTypes();
            Debug.Log(name + " generating rivers, time: " + UnityEngine.Time.realtimeSinceStartup);
            GenerateRivers();
            Debug.Log(name + " initializing areas, time: " + UnityEngine.Time.realtimeSinceStartup);
            foreach (var area in Areas)
            {
                area.Initialize(Areas);
            }
            Debug.Log(name + " optimizing areas meshes, time: " + UnityEngine.Time.realtimeSinceStartup);
            Area.OptimizeMeshes(WaterAreas, "Water Areas");
            Area.OptimizeMeshes(MountainAreas, "Mountain Areas");
            /*
            var usableAreas = new List<Area>(PlainsAreas);
            usableAreas.AddRange(HillsAreas);
            Area.OptimizeMeshes(usableAreas, "Usable Areas");*/
            Debug.Log(name + " done, time: " + UnityEngine.Time.realtimeSinceStartup);

            GetComponent<MeshRenderer>().enabled = false;
        }

        private void CreateAreas()
        {
            var mesh = GetComponent<MeshFilter>().mesh;

            int i = 0;
            var vertices = new HashSet<Vector3>(mesh.vertices);
            foreach (var vertice in vertices)
            {
                var go = Instantiate(AreaPrefab);
                go.name = "Area " + i.ToString();
                var area = go.GetComponent<Area>();
                area.transform.parent = transform;
                area.transform.position = vertice * transform.localScale.x;
                area.SetGlobeMesh(MeshFilter.mesh);

                //var material = area.GetComponentInChildren<MeshRenderer>().material;
                //material.renderQueue = 3000 + ((i + 1) % 100);

                Areas.Add(area);
                i++;
            }
        }

        private void SetAreasNeighbours()
        {
            Debug.Log("  setting positions, time: " + UnityEngine.Time.realtimeSinceStartup);
            var positions = new Vector3[Areas.Count];
            var ids = new float[Areas.Count * 6];
            for (int i = 0; i < Areas.Count; i++)
            {
                positions[i] = Areas[i].transform.position;
            }
            var areasBuffer = new ComputeBuffer(positions.Length, sizeof(float) * 3);
            var neighbourIdsBuffer = new ComputeBuffer(ids.Length, sizeof(float));
            areasBuffer.SetData(positions);
            var dataSize = positions.Length / 64 + 1;
            NeighbourShader.SetBuffer(0, "Positions", areasBuffer);
            NeighbourShader.SetBuffer(0, "NeighbourIds", neighbourIdsBuffer);
            Debug.Log("  dispatching shader, time: " + UnityEngine.Time.realtimeSinceStartup);
            NeighbourShader.Dispatch(0, dataSize, 1, 1);
            neighbourIdsBuffer.GetData(ids);
            areasBuffer.Release();
            neighbourIdsBuffer.Release();

            Debug.Log("  setting neighbours, time: " + UnityEngine.Time.realtimeSinceStartup);
            for (int i = 0; i < Areas.Count; i++)
            {
                var neighbours = new List<Area>
                {
                    Areas[(int)ids[i * 6]],
                    Areas[(int)ids[i * 6 + 1]],
                    Areas[(int)ids[i * 6 + 2]],
                    Areas[(int)ids[i * 6 + 3]],
                    Areas[(int)ids[i * 6 + 4]],
                    Areas[(int)ids[i * 6 + 5]],
                };
                Areas[i].SetNeighbours(neighbours);
            }
        }

        private void SetAreasTypes()
        {
            int idsPerArea = 20;
            Debug.Log("  setting positions, time: " + UnityEngine.Time.realtimeSinceStartup);
            var positions = new Vector3[Areas.Count];
            for (int i = 0; i < Areas.Count; i++)
            {
                positions[i] = Areas[i].transform.position;
            }
            var ids = new int[positions.Length* idsPerArea];
            var vertices = MeshFilter.mesh.vertices;

            Matrix4x4 localToWorld = MeshFilter.transform.localToWorldMatrix;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = localToWorld.MultiplyPoint3x4(vertices[i]) * 0.99f;
            }

            var verticesBuffer = new ComputeBuffer(vertices.Length, sizeof(float) * 3);
            var positionBuffer = new ComputeBuffer(positions.Length, sizeof(float) * 3);
            var idBuffer = new ComputeBuffer(ids.Length, sizeof(int));
            verticesBuffer.SetData(vertices);
            positionBuffer.SetData(positions);
            var dataSize = positions.Length / 64 + 1;
            DistanceShader.SetBuffer(0, "Vertices", verticesBuffer);
            DistanceShader.SetBuffer(0, "Positions", positionBuffer);
            DistanceShader.SetBuffer(0, "VerticeIds", idBuffer);
            Debug.Log("  dispatching shader, time: " + UnityEngine.Time.realtimeSinceStartup);
            DistanceShader.Dispatch(0, dataSize, 1, 1);
            idBuffer.GetData(ids);
            verticesBuffer.Release();
            positionBuffer.Release();
            idBuffer.Release();
            Debug.Log("  setting colors, time: " + UnityEngine.Time.realtimeSinceStartup);
            var colors = MeshFilter.mesh.colors;
            for (int i = 0; i < Areas.Count; i++)
            {
                Areas[i].SetType(colors[ids[i* idsPerArea]],this);
                var verticesIds=new int[idsPerArea];
                System.Array.Copy(ids, i * idsPerArea, verticesIds,0, idsPerArea);
                Areas[i].SetGlobeVertices(verticesIds);

            }
            Debug.Log("  instantiating meshes, time: " + UnityEngine.Time.realtimeSinceStartup);
            vertices = MeshFilter.mesh.vertices;
            for (int i = 0; i < Areas.Count; i++)
            {
                var position = localToWorld.MultiplyPoint3x4(vertices[ids[i * idsPerArea]]);
                Areas[i].Position = position;
            }
            for (int i = 0; i < Areas.Count; i++)
            {
                switch (Areas[i].Type)
                {
                    case Area.EType.Mountains:
                        {
                            var j = random.Next(Mountains.Count);
                            var mountain = Instantiate(Mountains[j]);
                            mountain.transform.position = (Areas[i].Position * (Areas[i].GetAreaOrNeighboursLowestPosition().magnitude/ Areas[i].Position.magnitude)) * 0.999f;
                            mountain.transform.LookAt(new Vector3(0, 0, 0));
                            mountain.GetComponentInChildren<MeshRenderer>().transform.localEulerAngles += new Vector3(0, 0, (float)random.NextDouble() * 360);
                            mountain.transform.parent = MeshFilter.transform;
                            mountain.transform.localScale /= 3;
                            SnowMovement.AddMaterial(mountain.GetComponentInChildren<MeshRenderer>().material);
                            Areas[i].SetLandformMesh(mountain.GetComponentInChildren<MeshFilter>().mesh);
                            break;
                        }
                    case Area.EType.Hills:
                        {
                            var j = random.Next(Mountains.Count);
                            var hill = Instantiate(Hills[j]);
                            hill.transform.position = (Areas[i].Position * (Areas[i].GetAreaOrNeighboursLowestPosition().magnitude / Areas[i].Position.magnitude)) * 0.999f;
                            hill.transform.LookAt(new Vector3(0, 0, 0));
                            hill.GetComponentInChildren<MeshRenderer>().transform.localEulerAngles += new Vector3(0, 0, (float)random.NextDouble() * 360);
                            hill.transform.parent = MeshFilter.transform;
                            hill.transform.localScale /= 3;
                            SnowMovement.AddMaterial(hill.GetComponentInChildren<MeshRenderer>().material);
                            Areas[i].SetLandformMesh(hill.GetComponentInChildren<MeshFilter>().mesh);
                            break;
                        }
                }
            }
        }

        private void GenerateRivers()
        {
            var max = 400 + random.Next(100);
            for (int i = 0; i < max; i++)
            {
                var start = MountainAreas[random.Next(MountainAreas.Count - 1)];
                if (start.River == null)
                {
                    try
                    {
                        Rivers.Add(new River(start, 500, MeshFilter.transform, RiverMaterial));
                    }
                    catch
                    {
                    }
                }
            }
            var vertices = new List<int>();
            var color = new Color(0,1,0,1);
            foreach (var area in PlainsAreas)
            {
                if (area.River != null)
                {
                    vertices.AddRange(area.GetGlobeVertices());
                    area.SetLandformVerticesColor(color);
                }
            }
            foreach (var area in HillsAreas)
            {
                if (area.River != null)
                {
                    vertices.AddRange(area.GetGlobeVertices());
                    area.SetLandformVerticesColor(color);
                }
            }
            var colors = MeshFilter.mesh.colors;
            foreach (var vertice in vertices)
            {
                if (colors[vertice].b != 1)
                {
                    colors[vertice] = color;
                }
            }
            MeshFilter.mesh.SetColors(colors);

            River.OptimizeAllRivers();
        }
    }
}