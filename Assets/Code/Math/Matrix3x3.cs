using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Math
{
    public class Matrix3x3
    {
        public Vector3 Row1 { get; private set; }
        public Vector3 Row2 { get; private set; }
        public Vector3 Row3 { get; private set; }

        public Matrix3x3(Vector3 row1, Vector3 row2, Vector3 row3)
        {
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }

        public float Determinant()
        {
            return Row1.x * Row2.y * Row3.z + Row1.y * Row2.z * Row3.x + Row1.z * Row2.x * Row3.y - Row1.z * Row2.y * Row3.x - Row1.y * Row2.x * Row3.z - Row1.x * Row2.z * Row3.y;
        }
    }
}
