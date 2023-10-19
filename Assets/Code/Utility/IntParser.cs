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
            float fValue = 0;
            if (value == 0)
            {
                return "0";
            }
            var prefix = Prefix.none;
            //if (value > 0)
            //{
            if (value >= 1000000000000000 || value <= -1000000000000000)
            {
                prefix = Prefix.P;
                fValue = value / 1000000000000000;
            }
            else if (value >= 1000000000000 || value <= -1000000000000)
            {
                prefix = Prefix.T;
                fValue = value / 1000000000000;
            }
            else if (value >= 1000000000 || value <= -1000000000)
            {
                prefix = Prefix.G;
                fValue = value / 1000000000;
            }
            else if (value >= 1000000 || value <= -1000000)
            {
                prefix = Prefix.M;
                fValue = value / 1000000;
            }
            else if (value >= 1000 || value <= -1000)
            {
                prefix = Prefix.k;
                fValue = value / 1000;
            }
            if (prefix != Prefix.none)
            {
                if (fValue < 10 && fValue > -10)
                {
                    return fValue.ToString("0.0") + prefix.ToString();
                }
                else
                {
                    return fValue.ToString("0") + prefix.ToString();
                }
            }
            return value.ToString("0");
            /*}
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
            }*/
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
