using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Project.Units
{
    public class UnitPath:MonoBehaviour
    {
        public UnityEngine.Material SelectedMaterial;
        public UnityEngine.Material UnselectedMaterial;
        public float Width;

        private Spline.Spline spline;
        private MeshRenderer splineMeshRenderer;
        private List<Vector3> positions;
        private bool shown=true;

        public void Create(Vector3 start, Pathfinding.Path path, Transform unitPaths)
        {
            Debug.Log("CREATING_PATH");
            Destroy();
            positions = new List<Vector3>();
            positions.Add(start);
            for (int i = 1; i < path.path.Count; i++)
            {
                positions.Add((path.path[i] as Map.MapPointNode).Area.Position);
            }
            spline = Spline.Spline.CreateSpline(positions, unitPaths, SelectedMaterial, "Unit Path", 5, 10, 1, Spline.Spline.EMarker.EndWithArrow, 0);
            splineMeshRenderer = spline.GetComponent<MeshRenderer>();
            if (shown)
            {
                splineMeshRenderer.material = SelectedMaterial;
            }
            else
            {
                splineMeshRenderer.material = UnselectedMaterial;
            }
        }

        public void Destroy()
        {
            if (spline == null)
            {
                return;
            }
            Destroy(spline.gameObject);
        }
        
        public void Hide()
        {
            if (spline == null)
            {
                return;
            }
            shown = false;
            splineMeshRenderer.material = UnselectedMaterial;
        }

        public void Show()
        {
            if (spline == null)
            {
                return;
            }
            shown = true;
            splineMeshRenderer.material = SelectedMaterial;
        }
    }
}
