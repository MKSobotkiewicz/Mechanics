﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dices
{
    public class D12 : Dice
    {
        private static readonly Random random = new Random();

        public override int MaxValue()
        {
            return 12;
        }
        public override int Roll()
        {
            return random.Next(1, 13);
        }

        public override EDice ToEnum()
        {
            return EDice.D12;
        }
    }
}