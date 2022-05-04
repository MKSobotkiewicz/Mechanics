using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Utility
{
    public class Scaler : MonoBehaviour
    {
        public float StartScale = 1;
        public float EndScale = 1;
        public float Lifetime = 1;
        private float timer = 0;

        public void Start()
        {
            timer = Lifetime;
        }

        public void Update()
        {
            timer -= UnityEngine.Time.deltaTime;
            var value = StartScale + (EndScale - StartScale) * ((Lifetime - timer) / Lifetime);
            transform.localScale = new Vector3(value, value, value);
            if (timer <= 0)
            {
                Destroy(this);
            }
        }
    }
}
