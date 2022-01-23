using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Units
{
    [Serializable]
    public class Defense
    {
        public int Armor { get; set; }
        public int MaxEntrenchment { get; set; }
        public int Morale { get; set; }
    }
}
