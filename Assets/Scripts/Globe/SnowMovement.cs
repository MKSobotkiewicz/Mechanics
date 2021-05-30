using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Globe
{
    public class SnowMovement : MonoBehaviour, Time.IDaily
    {
        public Time.Time Time;
        public List<UnityEngine.Material> materials=new List<UnityEngine.Material>();

        public void Start()
        {
            if (Time == null)
            {
                Debug.LogError(name + " missing Time.");
            }
            Time.Dailies.Add(this);
            materials.Add(GetComponent<MeshRenderer>().material);
        }

        public void AddMaterial(UnityEngine.Material material)
        {
            materials.Add(material);
        }

        public int Priority()
        {
            return 1;
        }

        public void DailyUpdate()
        {
            float angle =Mathf.Sin((((float)Time.Day - 80) / (float)Time.GetThisYearsDayCount()) * 2 * Mathf.PI) * 10;
            Debug.Log(angle);
            foreach (var material in materials)
            {
                material.SetFloat("Vector1_c1314484067849e9a0897c8e6b791b8b", -angle / 30);
            }
        }
    }
}
