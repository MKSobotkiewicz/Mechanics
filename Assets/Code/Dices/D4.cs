using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dices
{
    public class D4:Dice
    {
        private static readonly Random random = new Random();

        public override int MaxValue()
        {
            return 4;
        }
        public override int Roll()
        {
            return random.Next(1,5);
        }

        public override EDice ToEnum()
        {
            return EDice.D4;
        }
    }
}
