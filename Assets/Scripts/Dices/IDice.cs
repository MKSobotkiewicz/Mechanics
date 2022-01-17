using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dices
{
    public interface IDice
    {
        int Roll();
    }

    public enum EDice
    {
        D4,
        D6,
        D8,
        D10,
        D12
    }
}
