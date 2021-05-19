using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Mechanics
{
    public class TankBody : MonoBehaviour
    {
        private TankController tankController;

        public void Start()
        {
            tankController = GetComponentInParent<TankController>();
            if (tankController == null)
            {
                Debug.LogError(name+" missing tankController");
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            tankController.Destroy();
        }
    }
}
