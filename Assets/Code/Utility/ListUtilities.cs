using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Utility
{
    public static class ListUtilities
    {
        private static readonly Random random = new Random();

        public static type GetRandomObject<type>(List<type> list)
        {
            return list[random.Next(list.Count-1)];
        }
    }
}
