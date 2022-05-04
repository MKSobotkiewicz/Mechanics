using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Globe
{
    public class StarColorDistributionTester : MonoBehaviour
    {
        public float Opercentage;
        public float Bpercentage;
        public float Apercentage;
        public float Fpercentage;
        public float Gpercentage;
        public float Kpercentage;
        public float Mpercentage;

        public int Ocount = 0;
        public int Bcount = 0;
        public int Acount = 0;
        public int Fcount = 0;
        public int Gcount = 0;
        public int Kcount = 0;
        public int Mcount = 0;

        public void Start()
        {
            var sampleSize = 1000;
            for (int i = 0; i < sampleSize; i++)
            {
                var PlanetConditions = Globe.PlanetConditions.GenerateRandom();
                if (PlanetConditions.StarTemperature > 30000)
                {
                    Ocount++;
                }
                else if (PlanetConditions.StarTemperature > 10000)
                {
                    Bcount++;
                }
                else if (PlanetConditions.StarTemperature > 7500)
                {
                    Acount++;
                }
                else if (PlanetConditions.StarTemperature > 6000)
                {
                    Fcount++;
                }
                else if (PlanetConditions.StarTemperature > 5200)
                {
                    Gcount++;
                }
                else if (PlanetConditions.StarTemperature > 3700)
                {
                    Kcount++;
                }
                else if (PlanetConditions.StarTemperature > 2400)
                {
                    Mcount++;
                }
                Destroy(PlanetConditions.gameObject);
            }
            Opercentage = Ocount * 100 / sampleSize;
            Bpercentage = Bcount * 100 / sampleSize;
            Apercentage = Acount * 100 / sampleSize;
            Fpercentage = Fcount * 100 / sampleSize;
            Gpercentage = Gcount * 100 / sampleSize;
            Kpercentage = Kcount * 100 / sampleSize;
            Mpercentage = Mcount * 100 / sampleSize;
        }
    }
}
