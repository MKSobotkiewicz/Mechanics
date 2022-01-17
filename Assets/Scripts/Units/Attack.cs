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
        public List<Dices.EDice> Rolls;
        public int Bonus=0;
        public int Piercing=0;

        private Dices.IDice[] rolls;

        public void Init()
        {
            rolls = new Dices.IDice[Rolls.Count];
            for (int i=0;i< Rolls.Count;i++)
            {
                switch (Rolls[i])
                {
                    case Dices.EDice.D4:
                        rolls[i] = new Dices.D4();
                        break;
                    case Dices.EDice.D6:
                        rolls[i] = new Dices.D6();
                        break;
                    case Dices.EDice.D8:
                        rolls[i] = new Dices.D8();
                        break;
                    case Dices.EDice.D10:
                        rolls[i] = new Dices.D10();
                        break;
                    case Dices.EDice.D12:
                        rolls[i] = new Dices.D12();
                        break;
                    default:
                        UnityEngine.Debug.LogError("Wrong dice.");
                        break;
                }
            }
        }

        public int Commit(int armor)
        {
            var attackValue = Piercing - armor;
            attackValue = attackValue > 0 ? 0 : attackValue;
            attackValue += Bonus;
            foreach (var roll in rolls)
            {
                attackValue += roll.Roll();
            }
            attackValue = attackValue < 0 ? 0 : attackValue;
            return attackValue;
        }
    }
}
