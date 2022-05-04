using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Crafting
{
    public class MouseFollower : MonoBehaviour
    {
        public new UnityEngine.Camera camera;
        public float Distance = 1f;

        private List<Mechanics.Pivot> pivots;
        private Mechanics.Pivot onThatPivot = null;
        private static readonly float snapDistance = 1f;
        private Joint joint;
        //private new Rigidbody rigidbody;

        public void Start()
        {
            camera = UnityEngine.Camera.main;
            joint = GetComponentInChildren<Joint>();
            if (joint != null)
            {
                if (joint.GetComponent<Mechanics.Pivot>() != null)
                {
                    joint = null;
                   //return;
                }
                //rigidbody = joint.GetComponent<Rigidbody>();
            }
        }

        public void Update()
        {
            var hit = new RaycastHit();
            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (pivots != null)
                {
                    foreach (var pivot in pivots)
                    {
                        if (pivot.Occupied)
                        {
                            continue;
                        }
                        if (Vector3.Distance(pivot.transform.position, hit.point) < snapDistance)
                        {
                            transform.SetPositionAndRotation(pivot.transform.position, pivot.transform.rotation);
                            transform.localScale = pivot.transform.lossyScale;
                            if (joint != null)
                            {
                                joint.connectedBody = pivot.GetComponent<Rigidbody>();
                            }
                            onThatPivot = pivot;
                            pivot.DestroyGraphicNode();
                            return;
                        }
                    }
                }
                if (onThatPivot != null)
                {
                    onThatPivot.CreateGraphicNode();
                }
                onThatPivot = null;
                if (joint != null)
                {
                    joint.connectedBody = null;
                    //rigidbody.MovePosition(hit.point + hit.normal * Distance);
                    //rigidbody.MoveRotation(Quaternion.LookRotation(Vector3.forward, hit.normal));
                }
                transform.SetPositionAndRotation(hit.point+ hit.normal* Distance, Quaternion.LookRotation(Vector3.forward, hit.normal));
            }
        }

        public void SetPivots(List<Mechanics.Pivot> _pivots)
        {
            var part = GetComponentInChildren<Mechanics.Part>();
            if (part == null)
            {
                Debug.LogError("No part component in "+transform.name);
            }
            pivots = new List<Mechanics.Pivot>();
            foreach (var pivot in _pivots)
            {
                if (pivot == null)
                {
                    continue;
                }
                if (pivot.AcceptedTypes.Contains(part.Type))
                {
                    pivot.CreateGraphicNode();
                    pivots.Add(pivot);
                }
                else
                {
                    pivot.DestroyGraphicNode();
                }
            }
        }

        public bool OnPivot()
        {
            return (onThatPivot != null);
        }

        public void OnDestroy()
        {
            if (onThatPivot != null)
            {
                onThatPivot.Occupy(GetComponent<Mechanics.Part>());
            }
        }
    }
}