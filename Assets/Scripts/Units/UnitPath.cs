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

            public void Create(Unit unit,List<Map.Area> path)
        {
            Destroy();
            List<Vector3> positions = new List<Vector3>();
            positions.Add(unit.transform.position);
            for (int i = 1; i < path.Count; i++)
            {
                positions.Add(path[i].Position);
            }
            spline = Spline.Spline.CreateSpline(positions, transform, SelectedMaterial, "Unit Path", 5, 10, 1, Spline.Spline.EMarker.EndWithArrow, 0);
            splineMeshRenderer = spline.GetComponent<MeshRenderer>();
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
            //spline.gameObject.SetActive(false);
            splineMeshRenderer.material = UnselectedMaterial;
        }

        public void Show()
        {
            if (spline == null)
            {
                return;
            }
            //spline.gameObject.SetActive(true);
            splineMeshRenderer.material = SelectedMaterial;
        }
    }
}
