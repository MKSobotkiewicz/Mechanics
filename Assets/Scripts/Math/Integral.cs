using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Math
{
    public class Integral
    {
        public float Value { get; private set; } = 0;

        public Integral()
        {
        }
        
        public Integral(float startingValue)
        {
            Value = startingValue;
        }

        public float Update(float newSample,float deltaTime)
        {
            return Value = Value * (1f-0.1f* deltaTime) + (newSample * deltaTime);
        }
    }
}
