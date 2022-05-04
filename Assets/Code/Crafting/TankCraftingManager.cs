using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Crafting
{
    public class TankCraftingManager : MonoBehaviour
    {
        public Mechanics.Part WieldedPart { get; private set; } = null;
        private Mechanics.Part basePart=null;
        private Mechanics.TankController tankController = null;
        private List<Mechanics.Pivot> pivots=new List<Mechanics.Pivot>();
        private static readonly float distanceForBase = 2;
        private static readonly float platformHeight = 1.15f;

        public void Start()
        {
            
        }

        public void SetPart(Mechanics.Part newPart)
        {
            DestroyPart(WieldedPart);
            WieldedPart = Instantiate(newPart);
            if (newPart.Type == Mechanics.Part.TypeE.Hull && basePart != null)
            {
                pivots = new List<Mechanics.Pivot>();
                DestroyPart(basePart);
            }
            var mf=WieldedPart.gameObject.AddComponent<MouseFollower>();
            mf.SetPivots(pivots);
            if (WieldedPart.Type == Mechanics.Part.TypeE.Hull)
            {
                WieldedPart.GetComponent<Rigidbody>().isKinematic = true;
                //mf.Distance = DistanceForBase;
            }
            foreach (var collider in WieldedPart.gameObject.GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }

        public void Update()
        {
            if (WieldedPart == null)
            {
                return;
            }
            if (Input.GetAxis("Fire2") > 0)
            {
                DestroyPart(WieldedPart);
            }
            if (Input.GetAxis("Fire1") > 0)
            {
                var mf=WieldedPart.gameObject.GetComponent<MouseFollower>();
                if (mf.OnPivot()|| WieldedPart.Type == Mechanics.Part.TypeE.Hull)
                {
                    DestroyImmediate(mf);
                    if (tankController != null)
                    {
                        tankController.Start();
                    }
                    foreach (var collider in WieldedPart.gameObject.GetComponentsInChildren<Collider>())
                    {
                        collider.enabled = true;
                    }
                    foreach (var pivot in WieldedPart.GetComponentsInChildren<Mechanics.Pivot>())
                    {
                        pivots.Add(pivot);
                    }
                    if (WieldedPart.Type == Mechanics.Part.TypeE.Hull)
                    {
                        basePart = WieldedPart;
                        tankController = basePart.GetComponent<Mechanics.TankController>();
                        basePart.transform.SetPositionAndRotation(transform.position+new Vector3(0, platformHeight, 0),transform.rotation);
                    }
                    foreach (var pivot in pivots)
                    {
                        pivot.CreateGraphicNode();
                    }
                    WieldedPart = null;
                }
            }
        }

        private void DestroyPart(Mechanics.Part part)
        {
            if (part != null && pivots != null)
            {
                foreach (var pivot in part.GetComponentsInChildren<Mechanics.Pivot>())
                {
                    pivots.Remove(pivot);
                }
                Destroy(part.gameObject);
            }
        }
    }
}