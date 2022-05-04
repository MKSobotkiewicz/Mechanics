using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class Rotator : MonoBehaviour
    {
        public Vector3 Speed = new Vector3(0,0,0);
        private Rigidbody rigidbody;

        void Start()
        {
            transform.rotation=Quaternion.AngleAxis(30,Vector3.left);
            rigidbody = GetComponent<Rigidbody>();
            var part= GetComponentInChildren<Mechanics.Part>();
            //var mf = GetComponentInChildren < MeshFilter>();
            //mf.transform.localPosition -= mf.mesh.bounds.center;
            //transform.localScale *= 2.5f*Mathf.Pow(0.73575888234f, mf.mesh.bounds.max.magnitude);
        }

        void Update()
        {
            transform.Rotate(Speed* UnityEngine.Time.deltaTime, Space.Self);
            return;
        }
    }

}