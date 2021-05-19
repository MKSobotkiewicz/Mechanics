using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;

namespace Project.Utility
{
    public class LightScaler : MonoBehaviour
    {
        public float Lifetime = 1;
        private float timer = 0;
        private HDAdditionalLightData hDAdditionalLightData;
        private float maxValue;

        public void Start()
        {
            hDAdditionalLightData = GetComponent<HDAdditionalLightData>();
            maxValue = hDAdditionalLightData.intensity;
            timer = Lifetime;
        }

        public void Update()
        {
            timer -= UnityEngine.Time.deltaTime;
            hDAdditionalLightData.intensity = maxValue * ((Lifetime - timer) / Lifetime);
            if (timer <= 0)
            {
                Destroy(this);
            }
        }
    }
}
