using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Units
{
    [Serializable]
    public class Attack
    {
        public int Piercing { get; set; }
        public int Breakthrough { get; set; }
        public int Terror { get; set; }
        public int ManpowerAttackBonus { get; set; }
        public int CohesionAttackBonus { get; set; }
        public Dices.IDice[] ManpowerAttackDices { get; set; } = new Dices.IDice[0];
        public Dices.IDice[] CohesionAttackDices { get; set; } = new Dices.IDice[0];
    }
}
