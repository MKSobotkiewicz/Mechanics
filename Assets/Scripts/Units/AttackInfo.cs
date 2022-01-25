using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Units
{
    [Serializable]
    public class AttackInfo
    {
        public int Piercing { get; set; }
        public int Breakthrough { get; set; }
        public int Terror { get; set; }
        public int UnpenetratedArmor { get; set; }
        public int UnbrokenEntrenchment { get; set; }
        public int Steadfastness { get; set; }
        public int Armor { get; set; }
        public int Morale { get; set; }
        public int ManpowerAttack { get; set; }
        public int CohesionAttack { get; set; }
    }
}
