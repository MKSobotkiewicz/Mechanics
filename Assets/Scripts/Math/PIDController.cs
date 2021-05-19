using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Math
{
    public class PIDController : IController
    {
        private ControllerTypeE type;
        private PIDGains pidGains;
        private Integral integral = new Integral();
        private Derivative derivative = new Derivative();
        private bool debug = false;

        public PIDController(PIDGains _pidGains, ControllerTypeE _type = ControllerTypeE.Linear)
        {
            pidGains = _pidGains;
            type = _type;
        }

        public float Update(float desiredValue,float measuredValue,float deltaTime)
        {
            var error=0f;
            switch (type)
            {
                case ControllerTypeE.Linear:
                    error = desiredValue - measuredValue;
                    break;
                case ControllerTypeE.Radial:
                    var error1 = (desiredValue - measuredValue) % 360;
                    var error2 = (180- desiredValue + measuredValue)%360;
                    if (debug)
                    {
                        UnityEngine.Debug.Log("E1:" + error1+ "    E2:"+ error2);
                    }
                    if (System.Math.Abs(error1) < System.Math.Abs(error2))
                    {
                        error = -180-error1;
                    }
                    else
                    {
                        error = error2;
                    }
                    break;
            }
            if (debug)
            {
                UnityEngine.Debug.Log("E:" + error);
            }

            var proportionalControl = pidGains.ProportionalGain * error;
            var integralControl = pidGains.IntegralGain * integral.Update(error, deltaTime);
            var derivativeControl = pidGains.DerivativeGain * derivative.Update(error, deltaTime);

            var control = proportionalControl + integralControl + derivativeControl;
            if (debug)
            {
                UnityEngine.Debug.Log("P:"+ proportionalControl+"   I:"+ integralControl + "    D:" + derivativeControl);
            }
            return control;
        }

        public void SetDebug(bool _debug)
        {
            debug = _debug;
        }

        public new ControllerTypeE GetType()
        {
            return type;
        }

        [System.Serializable]
        public class PIDGains
        {
            public float ProportionalGain=0;
            public float IntegralGain=0;
            public float DerivativeGain=0;
        }
    }
}
