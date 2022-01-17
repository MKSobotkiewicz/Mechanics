using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dices
{
    public class D8 : IDice
    {
        private static readonly Random random = new Random();

        public int Roll()
        {
            return random.Next(1, 9);
        }
    }
}