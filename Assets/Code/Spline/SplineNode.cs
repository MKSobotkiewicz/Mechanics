using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Spline
{
    //[ExecuteInEditMode]
    public class SplineNode : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(this.transform.position, 0.1f);
        }
    }
}
