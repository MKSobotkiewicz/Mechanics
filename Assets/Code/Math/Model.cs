using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Math
{
    public interface IModel
    {
        float Update(float control,float lastValue, float deltaTime);
        float GetLastValue();
    }
}
