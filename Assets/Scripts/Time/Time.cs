using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Time
{
    public class Time:MonoBehaviour
    {
        public uint Minute = 0;
        public uint Hour = 0;
        public uint Day = 0;
        public uint Year = 2050;
        
        [Range(0, 1000)]
        public uint Speed=0;

        public List<IDaily> Dailies=new List<IDaily>();

        public void FixedUpdate()
        {
            Minute += Speed;
            while (Minute >= 60)
            {
                Minute -= 60;
                Hour += 1;
            }
            while (Hour >= 24)
            {
                Hour -= 24;
                Day += 1;
                UpdateDailies();
            }
            while (Day >= (Year%4==0 ? 366 : 365))
            {
                Day -= (Year % 4 == 0 ? (uint)366 : (uint)365);
                Year++;
            }
        }

        public uint GetThisYearsDayCount()
        {
            return (uint)(Year % 4 == 0 ? 366 : 365);
        }

        private void UpdateDailies()
        {
            var dailies = new List<IDaily>(Dailies);
            int priority = 0;
            while (dailies.Count > 0)
            {
                for (int i=0;i< dailies.Count;i++)
                {
                    if (dailies[i] == null)
                    {
                        continue;
                    }
                    if(priority== dailies[i].Priority())
                    {
                        dailies[i].DailyUpdate();
                        dailies.RemoveAt(i);
                    }
                }
                priority++;
            }
        }
    }
}
