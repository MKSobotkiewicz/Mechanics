using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Math
{
    public static class General
    {
        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        public static float RotationClamp(float rotation)
        {

            if (rotation > 180)
            {
                return rotation - 360;
            }
            else if (rotation < -180)
            {
                return rotation + 360;
            }
            return rotation;
        }
    }
}
