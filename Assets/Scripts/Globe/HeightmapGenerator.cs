using System.Collections;
using System.Collections.Generic;
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
        public ComputeShader ComputeShader;

        private MeshFilter meshFilter;

        private static readonly System.Random random = new System.Random();

        public void Start()
        {
            meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                Debug.LogError(name+ " missing MeshFilter.");
            }
            var vertices = meshFilter.mesh.vertices;
            var colors = new Color[vertices.Length];
            var verticesBuffer = new ComputeBuffer(vertices.Length, sizeof(float)*3);
            var colorsBuffer = new ComputeBuffer(colors.Length, sizeof(float) * 4);
            verticesBuffer.SetData(vertices);
            colorsBuffer.SetData(colors);

            var dataSize = vertices.Length/64+1;

            ComputeShader.SetBuffer(0,"Vertices", verticesBuffer);
            ComputeShader.SetBuffer(0, "Colors", colorsBuffer);
            ComputeShader.SetFloat("Seed", (float)random.NextDouble());
            ComputeShader.Dispatch(0, dataSize, 1, 1);
            verticesBuffer.GetData(vertices);
            colorsBuffer.GetData(colors);
            meshFilter.mesh.SetVertices(vertices);
            meshFilter.mesh.SetColors(colors);
            meshFilter.mesh.RecalculateBounds();
            meshFilter.mesh.RecalculateNormals();
            meshFilter.mesh.RecalculateTangents();
            verticesBuffer.Release();
            colorsBuffer.Release();
        }

        public void Update()
        {
        }
    }
}
