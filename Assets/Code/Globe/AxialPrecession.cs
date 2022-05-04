using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Globe
{
    public class AxialPrecession : MonoBehaviour,Time.IDaily
    {
        public Time.Time Time;

        public void Start()
        {
            if (Time == null)
            {
                Debug.LogError(name+" missing Time.");
            }
            Time.AddDaily(this);
        }

        public uint Priority()
        {
            return 22;
        }

        public void DailyUpdate()
        {
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Sin((((float)Time.Day - 80) / (float)Time.GetThisYearsDayCount())*2*Mathf.PI) *10);
        }
    }
}
