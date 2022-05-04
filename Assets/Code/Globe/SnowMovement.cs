using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Globe
{
    public class SnowMovement : MonoBehaviour, Time.IDaily
    {
        public Time.Time Time;
        public List<UnityEngine.Material> materials=new List<UnityEngine.Material>();
        public float Value { get; private set; }

        public void Start()
        {
            if (Time == null)
            {
                Debug.LogError(name + " missing Time.");
            }
            Time.AddDaily(this);
            materials.Add(GetComponent<MeshRenderer>().material);
        }

        public void AddMaterial(UnityEngine.Material material)
        {
            materials.Add(material);
        }

        public uint Priority()
        {
            return 23;
        }

        public void DailyUpdate()
        {
            float angle =Mathf.Sin((((float)Time.Day - 80) / (float)Time.GetThisYearsDayCount()) * 2 * Mathf.PI) * 10;
            //Debug.Log(angle);
            Value = -angle / 15;
            foreach (var material in materials)
            {
                material.SetFloat("_SnowMove", Value);
            }
        }
    }
}
