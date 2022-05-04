using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Camera
{
    public class ControlType : MonoBehaviour
    {
        private Text text;
        private Mechanics.TurretController turretController;

        public void Start()
        {
            text = GetComponent<Text>();
            turretController = GetComponentInParent<Mechanics.TurretController>();
        }

        public void Update()
        {
            switch (turretController.Control)
            {
                case Mechanics.TurretController.ControlE.Auto:
                    text.text = "AUTO";
                    break;
                case Mechanics.TurretController.ControlE.Locked:
                    text.text = "LOCKED";
                    break;
                case Mechanics.TurretController.ControlE.Manual:
                    text.text = "MANUAL";
                    break;
            }
        }
    }
}
