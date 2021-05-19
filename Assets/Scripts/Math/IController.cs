using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Math
{
    public interface IController:IDebug
    {
        float Update(float desiredValue, float measuredValue, float deltaTime);
        ControllerTypeE GetType();
    }

    public enum ControllerTypeE
    {
        Linear,
        Radial
    }
}
