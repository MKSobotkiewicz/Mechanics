using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Map
{
    public class MapGenerator : MonoBehaviour
    {
        public GameObject AreaPrefab;
        public List<Area> Areas = new List<Area>();
        public MeshFilter MeshFilter;
        public ComputeShader DistanceShader;
        public ComputeShader NeighbourShader;

        private readonly System.Random random = new System.Random();

        public void Start()
        {
            Debug.Log(name+ " creating areas");
            CreateAreas();
            Debug.Log(name + " setting areas neighbours");
            SetAreasNeighbours();
            Debug.Log(name + " setting areas types");
            SetAreasTypes();
            Debug.Log(name + " done");
            foreach (var area in Areas)
            {
                area.Initialize(Areas);
            }

            GetComponent<MeshRenderer>().enabled = false;
        }

        private void CreateAreas()
        {
            var mesh = GetComponent<MeshFilter>().mesh;

            int i = 0;
            foreach (var vertice in mesh.vertices)
            {
                bool done = false;
                foreach (var oldArea in Areas)
                {
                    if (Vector3.Distance(oldArea.transform.position, vertice * transform.localScale.x) < 0.1f)
                    {
                        done = true;
                        break;
                    }
                }
                if (done)
                {
                    continue;
                }
                var go = Instantiate(AreaPrefab);
                go.name = "Area " + i.ToString();
                var area = go.GetComponent<Area>();
                area.transform.parent = transform;
                area.transform.position = vertice * transform.localScale.x;

                var material = area.GetComponentInChildren<MeshRenderer>().material;
                material.renderQueue = 3000 + ((i + 1) % 100);

                Areas.Add(area);
                i++;
            }
        }

        private void SetAreasNeighbours()
        {
            var positions = new Vector3[Areas.Count];
            var ids=new float[Areas.Count*6];
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
            NeighbourShader.Dispatch(0, dataSize, 1, 1);
            neighbourIdsBuffer.GetData(ids);
            areasBuffer.Release();
            neighbourIdsBuffer.Release();

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
            var positions = new Vector3[Areas.Count];
            for (int i = 0; i < Areas.Count; i++)
            {
                positions[i] = Areas[i].transform.position;
            }
            var ids = new float[positions.Length];
            var vertices = MeshFilter.mesh.vertices;

            Matrix4x4 localToWorld = MeshFilter.transform.localToWorldMatrix;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = localToWorld.MultiplyPoint3x4(vertices[i]) * 0.99f;
            }

            var verticesBuffer = new ComputeBuffer(vertices.Length, sizeof(float) * 3);
            var positionBuffer = new ComputeBuffer(positions.Length, sizeof(float) * 3);
            var idBuffer = new ComputeBuffer(ids.Length, sizeof(float));
            verticesBuffer.SetData(vertices);
            positionBuffer.SetData(positions);
            var dataSize = positions.Length / 64 + 1;
            DistanceShader.SetBuffer(0, "Vertices", verticesBuffer);
            DistanceShader.SetBuffer(0, "Positions", positionBuffer);
            DistanceShader.SetBuffer(0, "VerticeIds", idBuffer);
            DistanceShader.Dispatch(0, dataSize, 1, 1);
            idBuffer.GetData(ids);
            verticesBuffer.Release();
            positionBuffer.Release();
            idBuffer.Release();
            for (int i = 0; i < Areas.Count; i++)
            {
                Areas[i].SetType(MeshFilter.mesh.colors[(int)ids[i]]);
            }
        }
    }
}