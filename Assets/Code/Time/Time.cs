using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Dictionary<uint, List<IDaily>> Dailies { get; private set; }
        public List<IHourly> Hourlies { get; private set; }

        public void Awake()
        {
            Dailies = new Dictionary<uint, List<IDaily>>();
            Hourlies = new List<IHourly>();
            for (uint i = 0; i < 24; i++)
            {
                Dailies.Add(i,new List<IDaily>());
            }
        }

        public void FixedUpdate()
        {
            Minute += Speed;
            while (Minute >= 60)
            {
                Minute -= 60;
                Hour += 1;
                UpdateDailies(Hour % 24);
                UpdateHourlies();
            }
            while (Hour >= 24)
            {
                Hour -= 24;
                Day += 1;
            }
            while (Day >= (Year%4==0 ? 366 : 365))
            {
                Day -= (Year % 4 == 0 ? (uint)366 : (uint)365);
                Year++;
            }
        }

        public void AddDaily(IDaily daily)
        {
            Dailies[daily.Priority()].Add(daily);
        }

        public void AddHourly(IHourly hourly)
        {
            Hourlies.Add(hourly);
        }

        public uint GetThisYearsDayCount()
        {
            return (uint)(Year % 4 == 0 ? 366 : 365);
        }

        private void UpdateDailies(uint hour)
        {
            for (int i = 0; i < Dailies[hour].Count; i++)
            {
                Dailies[hour][i].DailyUpdate();
            }
        }

        private void UpdateHourlies()
        {
            for (int i = 0; i < Hourlies.Count; i++)
            {
                Hourlies[i].HourlyUpdate();
            }
        }
    }
}
