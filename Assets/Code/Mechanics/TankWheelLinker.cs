using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Project.Mechanics
{
    public class TankWheelLinker : MonoBehaviour
    {
        private TankWheel linkedTankWheel;
        private Transform baseTransform;
        private Vector3 correction;
        private Vector3 startingRotation;
        private float wobbliness = 0;
        private float wobblinessCount = 0;
        private float wobblinessCountMax = 0;
        private static readonly float maxRot = 10;
        private static readonly System.Random random = new System.Random();

        public void Start()
        {
            startingRotation = transform.localEulerAngles;
            baseTransform = transform;
            SetBaseTransform();
            var allTransforms = FindObjectsOfType<Transform>();
            var closest = new Tuple<Transform, float>(null, float.MaxValue);
            var transformsUnderBaseTransform = new List <Transform>( baseTransform.GetComponentsInChildren<Transform>() );
            var trackTransforms = new List<Transform>(transform.parent.parent.GetComponentsInChildren<Transform>());
            foreach (var tr in allTransforms)
            {
                if (tr.GetComponent<TankWheel>() == null)
                {
                    continue;
                }
                if (!transformsUnderBaseTransform.Contains(tr))
                {
                    continue;
                }
                if (trackTransforms.Contains(tr))
                {
                    continue;
                }
                var distance = Vector3.Distance(transform.position, tr.position);
                if (distance < closest.Item2)
                {
                    closest = new Tuple<Transform, float>(tr, distance);
                }
            }
            linkedTankWheel = closest.Item1.GetComponent<TankWheel>();
            correction = (linkedTankWheel.transform.position - transform.position) / 100;
            correction = Quaternion.AngleAxis(baseTransform.eulerAngles.y, Vector3.down) * correction;
            //correction = Quaternion.AngleAxis(baseTransform.eulerAngles.z, Vector3.back) * correction;
        }

        public void Update()
        {
            wobblinessCount += UnityEngine.Time.deltaTime;
            if (wobblinessCount > wobblinessCountMax)
            {
                wobblinessCount = 0;
                wobblinessCountMax = ((float)random.NextDouble())*0.1f;
                wobbliness = (((float)random.NextDouble())-0.5f) * 10;
            }
            transform.position = linkedTankWheel.transform.position;
            transform.localPosition += correction;
            if (linkedTankWheel.Ground == false)
            {
                float addRot = linkedTankWheel.Torque;
                if (addRot > maxRot)
                {
                    addRot = maxRot;
                }
                else if (addRot < -maxRot)
                {
                    addRot = -maxRot;
                }
                if (addRot != 0)
                {
                    addRot += wobbliness;
                }
                Vector3 newEulerAngles = startingRotation + new Vector3(addRot, 0, 0);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(newEulerAngles), UnityEngine.Time.deltaTime * 10);
            }
        }

        private void SetBaseTransform() 
        {
            if (baseTransform.parent != null)
            {
                baseTransform = baseTransform.parent;
                SetBaseTransform();
            }
        }
    }
}