using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Utility
{
    public static class IntParser
    {
        public static string Parse(long value)
        {
            var prefix="";
            if (value >= 10000000000000000 || value <= -10000000000000000)
            {
                prefix = "P";
                value /= 1000000000000000;
            }
            else if (value >= 10000000000000 || value <= -10000000000000)
            {
                prefix = "T";
                value /= 1000000000000;
            }
            else if (value >= 10000000000 || value <= -10000000000)
            {
                prefix = "G";
                value /= 1000000000;
            }
            else if (value >= 10000000 || value <= -10000000)
            {
                prefix = "M";
                value /= 1000000;
            }
            else if (value >= 10000 || value <= -10000)
            {
                prefix = "k";
                value /= 1000;
            }
            if (value == 0)
            {
                return "00000";
            }
            if (value > 0)
            {
                if (prefix.Length > 0)
                {
                    return value.ToString("0000") + prefix;
                }
                return value.ToString("00000") + prefix;
            }
            if (value < 0)
            {
                if (prefix.Length > 0)
                {
                    return value.ToString("000") + prefix;
                }
                return value.ToString("0000") + prefix;
            }
            return "ERROR";
        } 
    }
}
