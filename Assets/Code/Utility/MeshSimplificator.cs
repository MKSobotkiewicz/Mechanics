using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Utility
{
    public class MeshSimplificator:MonoBehaviour
    {
        public ComputeShader GetPlaneDistanceShader;
        public ComputeShader RemoveVerticesShader;

        public void Simplify(Mesh mesh)
        {
            Debug.Log("  Starting simplification, time: " + UnityEngine.Time.realtimeSinceStartup);
            var vertices = mesh.vertices;
            var triangles = mesh.triangles;
            var neighbours = new int[vertices.Length * 6];
            var planeDistances = new float[vertices.Length];
            var ids = new int[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                ids[i] = i;
            }
            var verticesBuffer = new ComputeBuffer(vertices.Length, sizeof(float) * 3);
            var trianglesBuffer = new ComputeBuffer(triangles.Length, sizeof(int));
            var idBuffer = new ComputeBuffer(vertices.Length, sizeof(int));
            var neighboursBuffer = new ComputeBuffer(vertices.Length * 6, sizeof(int));
            var planeDistancesBuffer = new ComputeBuffer(vertices.Length, sizeof(float));
            verticesBuffer.SetData(vertices);
            trianglesBuffer.SetData(triangles);
            idBuffer.SetData(ids);
            var dataSize = ids.Length / 64 + 1;
            GetPlaneDistanceShader.SetBuffer(0, "Vertices", verticesBuffer);
            GetPlaneDistanceShader.SetBuffer(0, "Triangles", trianglesBuffer);
            GetPlaneDistanceShader.SetBuffer(0, "VerticesId", idBuffer);
            GetPlaneDistanceShader.SetBuffer(0, "Distances", planeDistancesBuffer);
            GetPlaneDistanceShader.SetBuffer(0, "Neighbours", neighboursBuffer);
            Debug.Log("  dispatching GetPlaneDistanceShader, time: " + UnityEngine.Time.realtimeSinceStartup);
            GetPlaneDistanceShader.Dispatch(0, dataSize, 1, 1);
            neighboursBuffer.GetData(neighbours);
            planeDistancesBuffer.GetData(planeDistances);
            idBuffer.Release();
            planeDistancesBuffer.Release();
            Debug.Log("  GetPlaneDistanceShader done, time: " + UnityEngine.Time.realtimeSinceStartup);

            var verticesDictionary = new Dictionary<int,float>();
            for (int i=0;i< planeDistances.Length;i++)
            {
                if (float.IsNaN(planeDistances[i]))
                {
                    Debug.Log("  plane distance is NAN!");
                    continue;
                }
                verticesDictionary.Add(i,planeDistances[i]);
            }
            var sortedVertList = verticesDictionary.OrderBy(x => x.Value).ToList();
            ids = new int[vertices.Length/2];
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = sortedVertList[i].Key;
            }
            idBuffer = new ComputeBuffer(ids.Length, sizeof(int));
            idBuffer.SetData(ids);
            dataSize = ids.Length / 64 + 1;
            RemoveVerticesShader.SetBuffer(0, "Vertices", verticesBuffer);
            RemoveVerticesShader.SetBuffer(0, "Triangles", trianglesBuffer);
            RemoveVerticesShader.SetBuffer(0, "VerticesId", idBuffer);
            RemoveVerticesShader.SetBuffer(0, "Neighbours", neighboursBuffer);
            Debug.Log("  dispatching RemoveVerticesShader, time: " + UnityEngine.Time.realtimeSinceStartup);
            RemoveVerticesShader.Dispatch(0, dataSize, 1, 1);
            verticesBuffer.GetData(vertices);
            trianglesBuffer.GetData(triangles);
            idBuffer.Release();
            verticesBuffer.Release();
            trianglesBuffer.Release();
            neighboursBuffer.Release();

            var verticesT = new Vector3[vertices.Length];
            var verticesRemoved = new bool[vertices.Length];
            var verticesTLength = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (!(vertices[i].x == 0 &&
                      vertices[i].y == 0 &&
                      vertices[i].z == 0))
                {
                    verticesT[verticesTLength] = vertices[i];
                    verticesTLength++;
                    verticesRemoved[verticesTLength] = false;
                }
                else
                {
                    verticesRemoved[verticesTLength] = true;
                }
            }

            var trianglesT = new int[triangles.Length];
            var trianglesTLength = 0;
            for (int i=0;i< triangles.Length; i+=3)
            {
                if (triangles[i] != triangles[i + 1] &&
                    triangles[i] != triangles[i + 2] &&
                    triangles[i + 1] != triangles[i + 2])
                {
                    if (verticesRemoved[triangles[i]] is false &&
                        verticesRemoved[triangles[i + 1]] is false &&
                        verticesRemoved[triangles[i + 2]] is false)
                    {
                        trianglesT[trianglesTLength] = triangles[i];
                        trianglesT[trianglesTLength + 1] = triangles[i + 1];
                        trianglesT[trianglesTLength + 2] = triangles[i + 2];
                        trianglesTLength += 3;
                    }
                }
            }
            Array.Resize(ref trianglesT, trianglesTLength);
            Array.Resize(ref verticesT, verticesTLength);

            Debug.Log("triangles: " + trianglesTLength + " from " + triangles.Length + " : " + (float)trianglesTLength*100 / (float)triangles.Length+"%");
            Debug.Log("vertices: " + verticesTLength + " from " + vertices.Length + " : " + (float)verticesTLength * 100 / (float)vertices.Length + "%");

            mesh.SetTriangles(trianglesT, 0);
           //mesh.SetVertices(verticesT);

            Debug.Log("  RemoveVerticesShader done, time: " + UnityEngine.Time.realtimeSinceStartup);
        }
    }
}
