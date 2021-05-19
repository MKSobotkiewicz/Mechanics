using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Globe
{
    public class SnowMovement : MonoBehaviour, Time.IDaily
    {
        public Time.Time Time;

        private UnityEngine.Material material;

        public void Start()
        {
            if (Time == null)
            {
                Debug.LogError(name + " missing Time.");
            }
            Time.Dailies.Add(this);
            material = GetComponent<MeshRenderer>().material;
        }

        public int Priority()
        {
            return 1;
        }

        public void DailyUpdate()
        {
            float angle =Mathf.Sin((((float)Time.Day - 80) / (float)Time.GetThisYearsDayCount()) * 2 * Mathf.PI) * 10;
            Debug.Log(angle);
            material.SetFloat("Vector1_c1314484067849e9a0897c8e6b791b8b", -angle/30);
        }
    }
}
