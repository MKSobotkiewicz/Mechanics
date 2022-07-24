using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Project.Dices
{
    public class D10 : Dice
    {
        private static readonly Random random = new Random();

        public override int MaxValue()
        {
            return 10;
        }

        public override int Roll()
        {
            return random.Next(1, 11);
        }

        public override EDice ToEnum()
        {
            return EDice.D10;
        }
    }
}