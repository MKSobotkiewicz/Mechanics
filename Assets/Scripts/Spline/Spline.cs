using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Spline
{
    //[ExecuteInEditMode]
    public class Spline : MonoBehaviour
    {
        public float Width;
        [Range(0, 30)]
        public int Divisions;
        [Range(0, 10)]
        public int SoftenSteps;
        public bool Update = false;

        private EMarker splineMarker;

        private List<Vector3> mainPoints;
        private List<Vector3> softenedPoints;
        private List<Vector3> dividedPoints;

        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;

        /*public void OnValidate()
        {
            if (!Update)
            {
                return;
            }
            Update = false;
            Generate();
        }*/

        public void Generate(float width = 0, int divisions = 0,int softenSteps=0, EMarker marker= EMarker.None)
        {
            Width = width;
            Divisions = divisions;
            SoftenSteps = softenSteps;
            splineMarker = marker;
            GetMainPoints();
            SoftenPoints();
            DividePoints();
            GenerateMesh();

        }

        public void SetMaterial(UnityEngine.Material material)
        {
            meshRenderer.material = material;
        }

        private void GetMainPoints()
        {
            var nodes = GetComponentsInChildren<SplineNode>();
            mainPoints = new List<Vector3>();
            foreach (var node in nodes)
            {
                mainPoints.Add(node.transform.position);
            }
        }

        private void SoftenPoints()
        {
            softenedPoints = new List<Vector3>(mainPoints);
            for (int i=0;i< SoftenSteps;i++)
            {
                softenedPoints = SoftenStep(softenedPoints);
            }
        }

        private List<Vector3> SoftenStep(List<Vector3> points)
        {
            if (points.Count == 0)
            {
                Debug.LogWarning("Trying to soften zero length list of points.");
                return points;
            }
            var newPoints = new List<Vector3>();
            newPoints.Add(points[0]);
            for (int i=0;i< points.Count-1;i++)
            {
                newPoints.Add(Vector3.Lerp(points[i], points[i+1],0.5f));
            }
            newPoints.Add(points.Last());
            return newPoints;
        }

        private void DividePoints()
        {
            var dividedPointsSet = new HashSet<Vector3>();
            if (splineMarker!=EMarker.StartNarrow)
            {
                dividedPointsSet.Add(softenedPoints[0]);
            }
            for (int i = 0; i < softenedPoints.Count - 2; i++)
            {
                dividedPointsSet.UnionWith(DivideStep(softenedPoints[i], softenedPoints[i+1], softenedPoints[i+2]));
            }
            dividedPointsSet.Add(softenedPoints.Last());
            dividedPoints = dividedPointsSet.ToList();
        }

        private List<Vector3> DivideStep(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            var newPoints = new List<Vector3>();
            var startPoint = Vector3.Lerp(point1, point2, 0.5f);
            var endPoint = Vector3.Lerp(point2, point3, 0.5f);
            for (int i = 0; i <= Divisions; i++)
            {
                var pos = (float)i / (float)Divisions;
                var point12 = Vector3.Lerp(startPoint, point2, pos);
                var point23 = Vector3.Lerp(point2, endPoint, pos);
                newPoints.Add(Vector3.Lerp(point23, point12, 1- pos));
            }
            return newPoints;
        }

        private Vector3 GetNormal(Vector3 point1, Vector3 point2)
        {
            return (point2 - point1).normalized;
        }

        private List<Vector3> MovePoints(List<Vector3> points, float value)
        {
            var movedPoints = new List<Vector3>();
            Vector3 normal;
            for (int i = 0; i < points.Count-1; i++)
            {
                normal = GetNormal(points[i], points[i+1]);
                movedPoints.Add(points[i] + Vector3.Cross(normal, points[i].normalized).normalized * value);
            }
            normal= GetNormal(points[points.Count - 2], points[points.Count - 1]);
            movedPoints.Add(points[points.Count - 1] + Vector3.Cross(normal, points[points.Count - 1].normalized).normalized * value);
            return movedPoints;
        }

        private void GenerateMesh()
        {
            if ((meshRenderer = GetComponent<MeshRenderer>()) == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
            if ((meshFilter = GetComponent<MeshFilter>()) == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }

            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();

            var leftPoints = MovePoints(dividedPoints, -Width);
            var rightPoints = MovePoints(dividedPoints, Width);

            switch (splineMarker) {
                case EMarker.StartNarrow:
                for (int i = 0; i < Divisions; i++)
                {
                    var pos = (float)i / (float)Divisions;
                    leftPoints[i] = Vector3.Slerp(dividedPoints[i], leftPoints[i], pos);
                    rightPoints[i] = Vector3.Slerp(dividedPoints[i], rightPoints[i], pos);
                }
                    break;
                case EMarker.EndWithArrow:
                    leftPoints[leftPoints.Count - 1] = dividedPoints[leftPoints.Count - 1];
                    rightPoints[rightPoints.Count - 1] = dividedPoints[rightPoints.Count - 1];
                    var normal = GetNormal(leftPoints[leftPoints.Count - 2], leftPoints[leftPoints.Count - 1]);
                    leftPoints[leftPoints.Count - 2]=leftPoints[leftPoints.Count - 3]+ Vector3.Cross(normal, leftPoints[leftPoints.Count - 3].normalized).normalized * (-Width*2);
                    normal = GetNormal(rightPoints[rightPoints.Count - 2], rightPoints[rightPoints.Count - 1]);
                    rightPoints[rightPoints.Count - 2] = rightPoints[rightPoints.Count - 3] + Vector3.Cross(normal, rightPoints[rightPoints.Count - 3].normalized).normalized * (Width * 2);
                    break;
                default:
                    break;
            }

            vertices.Add(leftPoints[0]);
            vertices.Add(rightPoints[0]);
            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(0, 1));
            var length = 0f;
            for (int i=0;i< leftPoints.Count;i++)
            {
                vertices.Add(leftPoints[i]);
                vertices.Add(rightPoints[i]);
                triangles.AddRange(new List<int> {i * 2,
                                                  i * 2 + 1,
                                                  i * 2 + 2,
                                                  i * 2 + 2,
                                                  i * 2 + 1,
                                                  i * 2 + 3 });
                var distance = 0f;
                if (i == 0)
                {
                    distance = Vector3.Distance(leftPoints[i], leftPoints[i + 1]);
                }
                else
                {
                    distance = Vector3.Distance(leftPoints[i - 1], leftPoints[i]);
                }
                length += (distance/(Width*2));
                uvs.Add(new Vector2(length, 0));
                uvs.Add(new Vector2(length, 1));
            }

            var mesh = new Mesh();

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles,0);
            mesh.SetUVs(0,uvs);
            for(int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = vertices[i].normalized;
            }
            mesh.SetNormals(vertices);

            mesh.Optimize();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();

            meshFilter.mesh = mesh;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(this.transform.position, 0.1f);
            for (int i = 0; i < mainPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(mainPoints[i], mainPoints[i + 1]);
            }
            Gizmos.color = Color.green;
            var leftPoints = MovePoints(dividedPoints, -Width);
            var rightPoints = MovePoints(dividedPoints, Width);
            for (int i = 0; i < dividedPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(leftPoints[i], leftPoints[i + 1]);
                Gizmos.DrawLine(dividedPoints[i], dividedPoints[i + 1]);
                Gizmos.DrawLine(rightPoints[i], rightPoints[i + 1]);
            }
        }

        public static Spline CreateSpline(List<Vector3> positions, Transform parent, UnityEngine.Material material, string name, float width = 30, int divisions = 10, int softenSteps = 1, EMarker marker = EMarker.None, int startIndex = 0)
        {
            int nodeCount = 0;
            var spline = (new GameObject(name)).AddComponent<Spline>();
            spline.transform.parent = parent;
            for (int i = startIndex; i < positions.Count; i++)
            {
                var node = (new GameObject("Node " + nodeCount++)).AddComponent<SplineNode>();
                node.transform.parent = spline.transform;
                node.transform.position = positions[i];
            }
            spline.Generate(width, divisions, softenSteps, marker);
            spline.SetMaterial(material);
            return spline;
        }

        public static Spline CreateSpline(List<Map.Area> areas,Transform parent, UnityEngine.Material material,string name,float width = 30, int divisions = 10, int softenSteps = 1, EMarker marker=EMarker.None,int startIndex=0)
        {
            int nodeCount = 0;
            var spline = (new GameObject(name)).AddComponent<Spline>();
            spline.transform.parent = parent;
            for (int i = startIndex; i < areas.Count; i++)
            {
                var node = (new GameObject("Node " + nodeCount++)).AddComponent<SplineNode>();
                node.transform.parent = spline.transform;
                node.transform.position = areas[i].Position;
            }
            spline.Generate(width, divisions, softenSteps, marker);
            spline.SetMaterial(material);
            return spline;
        }

        public enum EMarker
        {
            None,
            StartNarrow,
            EndWithArrow
        }
    }
}
