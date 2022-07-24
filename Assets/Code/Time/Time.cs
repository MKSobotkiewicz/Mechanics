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

        public MeshRenderer CloudsMeshRenderer;

        private uint totalMinutes=0;

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
            totalMinutes += Speed;
            CloudsMeshRenderer.material.SetFloat("_CurrentTime", (float)totalMinutes);
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

        public Tuple<uint,uint> Date()
        {
            if (Day < 31)
            {
                return new Tuple<uint, uint>(Day,1);
            }
            if (Year % 4 == 0 ? true : false)
            {
                if (Day < 60)
                { 
                    return new Tuple<uint, uint>(Day - 31, 2);
                }
                if (Day < 91)
                {
                    return new Tuple<uint, uint>(Day - 60, 3);
                }
                if (Day < 121)
                {
                    return new Tuple<uint, uint>(Day - 91, 4);
                }
                if (Day < 152)
                {
                    return new Tuple<uint, uint>(Day - 121, 5);
                }
                if (Day < 182)
                {
                    return new Tuple<uint, uint>(Day - 152, 6);
                }
                if (Day < 213)
                {
                    return new Tuple<uint, uint>(Day - 182, 7);
                }
                if (Day < 244)
                {
                    return new Tuple<uint, uint>(Day - 213, 8);
                }
                if (Day < 274)
                {
                    return new Tuple<uint, uint>(Day - 244, 9);
                }
                if (Day < 305)
                {
                    return new Tuple<uint, uint>(Day - 274, 10);
                }
                if (Day < 335)
                {
                    return new Tuple<uint, uint>(Day - 305, 11);
                }
                if (Day < 366)
                {
                    return new Tuple<uint, uint>(Day - 335, 12);
                }
            }
            else
            {
                if (Day < 59)
                {
                    return new Tuple<uint, uint>(Day - 31, 2);
                }
                if (Day < 90)
                {
                    return new Tuple<uint, uint>(Day - 59, 3);
                }
                if (Day < 120)
                {
                    return new Tuple<uint, uint>(Day - 90, 4);
                }
                if (Day < 151)
                {
                    return new Tuple<uint, uint>(Day - 120, 5);
                }
                if (Day < 181)
                {
                    return new Tuple<uint, uint>(Day - 151, 6);
                }
                if (Day < 212)
                {
                    return new Tuple<uint, uint>(Day - 181, 7);
                }
                if (Day < 243)
                {
                    return new Tuple<uint, uint>(Day - 212, 8);
                }
                if (Day < 273)
                {
                    return new Tuple<uint, uint>(Day - 243, 9);
                }
                if (Day < 304)
                {
                    return new Tuple<uint, uint>(Day - 273, 10);
                }
                if (Day < 334)
                {
                    return new Tuple<uint, uint>(Day - 304, 11);
                }
                if (Day < 365)
                {
                    return new Tuple<uint, uint>(Day - 334, 12);
                }
            }
            return new Tuple<uint, uint>(0, 0);
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
