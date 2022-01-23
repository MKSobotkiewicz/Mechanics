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
        public Dices.IDice[] ManpowerAttackDices { get; set; }
        public Dices.IDice[] CohesionAttackDices { get; set; }

        public Tuple<int,int> Commit(int armor, int entrenchment, int morale)
        {
            var unpenetratedArmor = armor-Piercing;
            unpenetratedArmor = unpenetratedArmor < 0 ? 0 : unpenetratedArmor;
            var unbrokenEntrenchment = entrenchment - Breakthrough;
            unbrokenEntrenchment = unbrokenEntrenchment < 0 ? 0 : unbrokenEntrenchment;
            var manpowerAttackValue = ManpowerAttackBonus - unbrokenEntrenchment - unpenetratedArmor;
            foreach (var dice in ManpowerAttackDices)
            {
                manpowerAttackValue += dice.Roll();
            }
            manpowerAttackValue = manpowerAttackValue < 0 ? 0 : manpowerAttackValue;

            var steadfastness = morale - Terror;
            steadfastness = steadfastness < 0 ? 0 : steadfastness;
            var cohesionAttackValue = CohesionAttackBonus - steadfastness;
            foreach (var dice in CohesionAttackDices)
            {
                cohesionAttackValue += dice.Roll();
            }
            cohesionAttackValue = cohesionAttackValue < 0 ? 0 : cohesionAttackValue;

            return new Tuple<int,int>(manpowerAttackValue, cohesionAttackValue);
        }
    }
}
