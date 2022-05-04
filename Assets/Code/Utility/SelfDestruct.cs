using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Utility
{
    public class SelfDestruct : MonoBehaviour
    {
        public float Lifetime = 1;
        private float timer = 0;

        public void Start()
        {
            timer = Lifetime;
        }

        public void Update()
        {
            timer -= UnityEngine.Time.deltaTime;
            if (timer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
