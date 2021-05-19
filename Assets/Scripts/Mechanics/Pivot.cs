using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class Pivot : MonoBehaviour
    {
        public bool Occupied = false;
        public List<Part.TypeE> AcceptedTypes;
        private GameObject partPivot;

        public void Start()
        {
            if (!Occupied)
            {
                Release();
            }
        }

        public void Occupy(Part part)
        {
            part.transform.SetParent(transform);
            var rb = part.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                var fj=part.gameObject.AddComponent<ConfigurableJoint>();
                fj.connectedBody = GetComponent<Rigidbody>();
                fj.autoConfigureConnectedAnchor = false;
                fj.anchor = new Vector3(0, 0, 0);
                fj.connectedAnchor = new Vector3(0,0,0);
                fj.xMotion = ConfigurableJointMotion.Locked;
                fj.yMotion = ConfigurableJointMotion.Locked;
                fj.zMotion = ConfigurableJointMotion.Locked;
                fj.angularXMotion = ConfigurableJointMotion.Locked;
                fj.angularYMotion = ConfigurableJointMotion.Locked;
                fj.angularZMotion = ConfigurableJointMotion.Locked;
                //fj.projectionfmode = JointProjectionfmode.PositionAndRotation;
                fj.projectionDistance = 0;
            }
            Occupied = true;
            DestroyGraphicNode();
        }

        public void Release()
        {
            Occupied = false;
            CreateGraphicNode();
        }

        public void CreateGraphicNode()
        {
            if (!Occupied&& partPivot==null)
            {
                partPivot = Instantiate(UnityEngine.Resources.Load("TankCrafting/PartPivot"), transform) as GameObject;
            }
        }

        public void DestroyGraphicNode()
        {
            if (partPivot == null)
            {
                return;
            }
            Destroy(partPivot);
        }
    }
}
