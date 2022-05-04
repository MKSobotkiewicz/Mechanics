using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class TurretController : MonoBehaviour, IController
    {
        public bool RotationEnabled = true;
        public TurretControllerType TurretControllerType;
        public TurretControllerType CannonControllerType;
        public ControlE Control = ControlE.Locked;
        public Transform HatchPoint;
        public GameObject HatchFirePrefab;
        private Rotator turretRotator;
        private Rotator cannonRotator;
        private Transform rotationReferenceTransform;
        private Math.IController turretRotatorController;
        private Math.IController cannonRotatorController;
        private Queue<float> oldTurretRotationValues = new Queue<float>();
        private Queue<float> oldCannonRotationValues = new Queue<float>();
        private Transform cannonTransform;
        private bool clicked = false;
        private Vector3 desiredValue;
        private bool onFire = false;

        public void Start()
        {
            var allChildren = GetComponentsInChildren<Transform>();
            foreach (var tr in allChildren)
            {
                if (tr.tag == "Cannon")
                {
                    cannonTransform = tr;
                    break;
                }
            }
            turretRotator = GetComponentInParent<Rotator>();
            cannonRotator = GetComponentInChildren<Rotator>();
            if (TurretControllerType == null)
            {
                return;
            }
            if (CannonControllerType == null)
            {
                return;
            }
            turretRotatorController = new Math.PIDController(TurretControllerType.PIDGains, Math.ControllerTypeE.Radial);
            //turretRotatorController.SetDebug(true);
            cannonRotatorController = new Math.PIDController(CannonControllerType.PIDGains, Math.ControllerTypeE.Radial);
            //cannonRotatorController.SetDebug(true);
            if (turretRotator == null)
            {
                Debug.Log(gameObject.name + " missing turretRotator");
            }
        }

        public void SetRotationReferenceTransform(Transform transform)
        {
            rotationReferenceTransform = transform;
        }

        public void FireHatch()
        {
            if (!onFire)
            {
                onFire = true;
                Instantiate(HatchFirePrefab, HatchPoint);
            }
        }

        public void FixedUpdate()
        {
            if (rotationReferenceTransform == null)
            {
                return;
            }
            SetDesiredValue();
            RotationUpdate();
            ElevationUpdate();
        }

        public void Update()
        {
            ControlCheck();
        }

        public void Disable()
        {
            enabled = false;
        }

        private void SetDesiredValue()
        {
            RaycastHit hit;
            if (Physics.Raycast(rotationReferenceTransform.position + rotationReferenceTransform.forward * 2, rotationReferenceTransform.forward, out hit, 2000, ~(1 << 10)))
            {
                desiredValue = Quaternion.LookRotation(cannonTransform.position -hit.point, cannonTransform.up).eulerAngles;
            }
            else
            {
                desiredValue = Quaternion.LookRotation(-rotationReferenceTransform.forward * 5000, cannonTransform.up).eulerAngles;
            }
        }

        private void RotationUpdate()
        {
            if (turretRotator == null)
            {
                return;
            }
            switch (Control)
            {
                case ControlE.Auto:
                    turretRotator.Locked = false;
                    var desiredValueY = Math.General.RotationClamp(desiredValue.y);
                    var currentValue = Quaternion.LookRotation(transform.forward, Vector3.up).eulerAngles.y;
                    currentValue = Math.General.RotationClamp(currentValue);
                    oldTurretRotationValues.Enqueue(currentValue);
                    if (oldTurretRotationValues.Count < (int)(TurretControllerType.RotationDelay / UnityEngine.Time.fixedDeltaTime))
                    {
                        return;
                    }
                    turretRotator.SetSignal(turretRotatorController.Update(desiredValueY, oldTurretRotationValues.Dequeue(), UnityEngine.Time.fixedDeltaTime));
                    return;
                case ControlE.Manual:
                    turretRotator.Locked = false;
                    turretRotator.SetSignal(-Input.GetAxis("Mouse X"));
                    return;
                case ControlE.Locked:
                    turretRotator.SetSignal(0);
                    turretRotator.Locked = true;
                    return;
            }
        }

        private void ControlCheck()
        {
            if (Input.GetAxis("Control Type") > 0)
            {
                if (clicked == false)
                {
                    Control++;
                    if (Control > ControlE.Locked)
                    {
                        Control = ControlE.Auto;
                    }
                    Debug.Log(Control);
                    clicked = true;
                    return;
                }
                return;
            }
            clicked = false;
        }

        private void ElevationUpdate()
        {
            if (cannonRotator == null)
            {
                return;
            }
            switch (Control)
            {
                case ControlE.Auto:
                    cannonRotator.Locked = false;
                    var desiredValueX = Math.General.RotationClamp(desiredValue.x+180);
                    var currentValue = -Quaternion.LookRotation(cannonTransform.forward, Vector3.up).eulerAngles.x;
                    currentValue = Math.General.RotationClamp(currentValue);
                    oldCannonRotationValues.Enqueue(currentValue);
                    if (oldCannonRotationValues.Count < (int)(TurretControllerType.RotationDelay / UnityEngine.Time.fixedDeltaTime))
                    {
                        return;
                    }
                    cannonRotator.SetSignal(cannonRotatorController.Update(oldCannonRotationValues.Dequeue(), desiredValueX, UnityEngine.Time.fixedDeltaTime));
                    return;
                case ControlE.Manual:
                    cannonRotator.Locked = false;
                    cannonRotator.SetSignal(Input.GetAxis("Mouse Y"));
                    return;
                case ControlE.Locked:
                    cannonRotator.SetSignal(0);
                    cannonRotator.Locked = true;
                    return;
            }
        }

        public enum ControlE
        {
            Auto,
            Manual,
            Locked
        }
    }
}
