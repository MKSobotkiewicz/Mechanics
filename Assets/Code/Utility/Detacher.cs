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
    public class Detacher : MonoBehaviour
    {
        public void Start()
        {
            transform.parent = null;
        }
    }
}
