using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Math
{
    public class ElectricEngineModel:IModel
    {
        public float Power { get; private set; }
        public float Dampening { get; private set; }
        private float lastValue;

        public ElectricEngineModel(float power, float dampening)
        {
            Power = power;
            Dampening = dampening;
        }

        public float Update(float control,float _lastValue, float deltaTime)
        {
            control = General.Clamp(control, -1, 1);
            lastValue = lastValue-(control * deltaTime * Power) / Dampening;
            return lastValue = General.RotationClamp(lastValue);
        }

        public float GetLastValue()
        {
            return lastValue;
        }

    }
}
