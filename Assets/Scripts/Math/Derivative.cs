using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Math
{
    public class Derivative
    {
        public float Value { get; private set; } = 0;
        private float lastSample = 0;

        public Derivative()
        {
        }

        public Derivative(float startingSample)
        {
            lastSample = startingSample;
        }

        public float Update(float newSample, float deltaTime)
        {
            Value = (newSample - lastSample) * 1/ deltaTime;
            newSample = lastSample;
            return Value;
        }
    }
}
