using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class Freeze : MonoBehaviour
    {
        public bool Xpos = false;
        public bool Ypos = false;
        public bool Zpos = false;
        public bool Xrot = false;
        public bool Yrot = false;
        public bool Zrot = false;

        private Vector3 position;
        private Vector3 rotation;

        public void Start()
        {
            position = transform.localPosition;
            rotation = transform.localEulerAngles;
        }

        public void FixedUpdate()
        {
            Vector3 newPos;
            if (Xpos)
            {
                newPos.x = position.x;
            }
            else
            {
                newPos.x = transform.localPosition.x;
            }
            if (Ypos)
            {
                newPos.y = position.y;
            }
            else
            {
                newPos.y = transform.localPosition.y;
            }
            if (Zpos)
            {
                newPos.z = position.z;
            }
            else
            {
                newPos.z = transform.localPosition.z;
            }
            transform.localPosition = newPos;

            Vector3 newRot;

            if (Xrot)
            {
                newRot.x = rotation.x;
            }
            else
            {
                newRot.x = transform.localEulerAngles.x;
            }
            if (Yrot)
            {
                newRot.y = rotation.y;
            }
            else
            {
                newRot.y = transform.localEulerAngles.y;
            }
            if (Zrot)
            {
                newRot.z = rotation.z;
            }
            else
            {
                newRot.z = transform.localEulerAngles.z;
            }
            transform.localEulerAngles = newRot;
        }
    }
}
