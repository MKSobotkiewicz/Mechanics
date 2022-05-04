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
    public class GravityManager : MonoBehaviour
    {
        public Transform CenterOfMass;

        private void Start()
        {
            Mechanics.Gravity.CenterOfMass = CenterOfMass.position;
            foreach (var rigidbody in FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[])
            {
                if (rigidbody.useGravity)
                {
                    var gravity = rigidbody.gameObject.AddComponent<Mechanics.Gravity>();
                    rigidbody.useGravity = false;
                }
            }
        }
    }
}
