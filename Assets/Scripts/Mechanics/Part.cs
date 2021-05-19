using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class Part:MonoBehaviour
    {
        public TypeE Type = TypeE.Undefined;
        public bool Hidden = false;

        public void Start()
        {
        }

        public enum TypeE
        {
            Undefined,
            Hull,
            Engine,
            Turret,
            Wheel,
            WheelWithSuspension,
            Roller,
            Sprocket,
            TurretMotor,
            CannonMotor,
            CannonBase,
            Cannon,
            TurretArmour,
            HullArmour,
            CannonAddon,
            TurretBackAddon,
            Visor,
            TrackArmour
        }
    }
}
