using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Globe
{
    public class HeightmapGenerator : MonoBehaviour
    {
        [Range(0.0f, 1000f)]
        public float Scale = 1;
        [Range(0.0f, 1f)]
        public float Strength = 1;
        [Range(-10f, 10f)]
        public float X = 1;
        public ComputeShader HeightmapComputeShader;
        public ComputeShader RemoveWaterComputeShader;
        public Utility.MeshSimplificator MeshSimplificator;

        private MeshFilter meshFilter;

        private static readonly System.Random random = new System.Random();

        public void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                Debug.LogError(name+ " missing MeshFilter.");
            }
            var mesh = meshFilter.mesh;
            var vertices = mesh.vertices;
            var triangles = mesh.triangles;
            var colors = new Color[vertices.Length];
            var verticesBuffer = new ComputeBuffer(vertices.Length, sizeof(float)*3);
            var colorsBuffer = new ComputeBuffer(colors.Length, sizeof(float) * 4);
            var trianglesBuffer = new ComputeBuffer(triangles.Length, sizeof(int));
            trianglesBuffer.SetData(triangles);
            verticesBuffer.SetData(vertices);
            colorsBuffer.SetData(colors);

            var dataSize = vertices.Length/64+1;

            HeightmapComputeShader.SetBuffer(0, "Triangles", trianglesBuffer);
            HeightmapComputeShader.SetBuffer(0,"Vertices", verticesBuffer);
            HeightmapComputeShader.SetBuffer(0, "Colors", colorsBuffer);
            HeightmapComputeShader.SetFloat("Seed", (float)random.NextDouble());
            HeightmapComputeShader.Dispatch(0, dataSize, 1, 1);

            RemoveWaterComputeShader.SetBuffer(0, "Triangles", trianglesBuffer);
            RemoveWaterComputeShader.SetBuffer(0, "Vertices", verticesBuffer);
            RemoveWaterComputeShader.SetBuffer(0, "Colors", colorsBuffer);
            RemoveWaterComputeShader.Dispatch(0, dataSize, 1, 1);

            trianglesBuffer.GetData(triangles);
            verticesBuffer.GetData(vertices);
            colorsBuffer.GetData(colors);
            trianglesBuffer.Release();
            verticesBuffer.Release();
            colorsBuffer.Release();
            
            var trianglesT = new int[triangles.Length];
            var trianglesTLength = 0;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                if (vertices[triangles[i]] != new Vector3(0,0,0) && vertices[triangles[i + 1]] != new Vector3(0, 0, 0) && vertices[triangles[i + 2]] != new Vector3(0, 0, 0))
                {
                    trianglesT[trianglesTLength] = triangles[i];
                    trianglesT[trianglesTLength + 1] = triangles[i + 1];
                    trianglesT[trianglesTLength + 2] = triangles[i + 2];
                    trianglesTLength += 3;
                }
            }

            Array.Resize(ref trianglesT, trianglesTLength);

            Debug.Log("triangles: " + trianglesTLength + " from " + triangles.Length + " : " + (float)trianglesTLength * 100 / (float)triangles.Length + "%");

            mesh.SetTriangles(trianglesT, 0);
            mesh.SetVertices(vertices);
            mesh.SetColors(colors);
            mesh.Optimize();

            //MeshSimplificator.Simplify(meshFilter.mesh); doesn't work lol
            //mesh.Optimize();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            meshFilter.mesh = mesh;
            var collider = GetComponent<MeshCollider>();
            if (collider != null)
            {
                collider.sharedMesh = meshFilter.mesh;
            }
            else
            {
                Debug.LogWarning(name + " missing MeshCollider.");
            }
        }

        public void Update()
        {
        }
    }
}
