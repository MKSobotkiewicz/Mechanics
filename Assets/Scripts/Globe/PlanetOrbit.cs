using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Globe
{
    public class PlanetOrbit : MonoBehaviour
    {
        public Time.Time Time;

        public void Start()
        {
            if (Time == null)
            {
                Debug.LogError(name+" missing Time.");
            }
        }

        public void FixedUpdate()
        {
            var angle = ((float)(Time.Hour * 60 + Time.Minute)) * 0.25f;
            transform.eulerAngles = new Vector3(0, angle, 0);
        }
    }
}
