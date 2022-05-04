using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Project.Globe
{
    public class StarsMovement : MonoBehaviour
    {
        public Volume SkyVolume;
        public Time.Time Time;

        private PhysicallyBasedSky physicallyBasedSky;

        public void Start()
        {
            if (Time == null)
            {
                Debug.LogError(name + " missing Time.");
            }
            SkyVolume.sharedProfile.TryGet<PhysicallyBasedSky>(out physicallyBasedSky);
        }

        public void FixedUpdate()
        {
            physicallyBasedSky.spaceRotation.value = transform.eulerAngles;
        }
    }
}
