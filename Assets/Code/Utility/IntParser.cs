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
            if (value == 0)
            {
                return "00000";
            }
            var prefix = Prefix.none;
            if (value > 0)
            {
                if (value >= 10000000000000000 || value <= -10000000000000000)
                {
                    prefix = Prefix.P;
                    value /= 1000000000000000;
                }
                else if (value >= 10000000000000 || value <= -10000000000000)
                {
                    prefix = Prefix.T;
                    value /= 1000000000000;
                }
                else if (value >= 10000000000 || value <= -10000000000)
                {
                    prefix = Prefix.G;
                    value /= 1000000000;
                }
                else if (value >= 10000000 || value <= -10000000)
                {
                    prefix = Prefix.M;
                    value /= 1000000;
                }
                else if (value >= 10000 || value <= -10000)
                {
                    prefix = Prefix.k;
                    value /= 1000;
                }
                if (prefix != Prefix.none)
                {
                    return value.ToString("0000") + prefix.ToString();
                }
                return value.ToString("00000");
            }
            if (value >= 1000000000000000 || value <= -1000000000000000)
            {
                prefix = Prefix.P;
                value /= 1000000000000000;
            }
            else if (value >= 1000000000000 || value <= -1000000000000)
            {
                prefix = Prefix.T;
                value /= 100000000000;
            }
            else if (value >= 1000000000 || value <= -1000000000)
            {
                prefix = Prefix.G;
                value /= 1000000000;
            }
            else if (value >= 1000000 || value <= -1000000)
            {
                prefix = Prefix.M;
                value /= 1000000;
            }
            else if (value >= 1000 || value <= -1000)
            {
                prefix = Prefix.k;
                value /= 1000;
            }
            if (value < 0)
            {
                if (prefix != Prefix.none)
                {
                    return value.ToString("000") + prefix.ToString();
                }
                return value.ToString("0000");
            }
            return "ERROR";
        }

        public enum Prefix
        {
            none=0,
            k=1,
            M=2,
            G=3,
            T=4,
            P=5
        }
    }
}
