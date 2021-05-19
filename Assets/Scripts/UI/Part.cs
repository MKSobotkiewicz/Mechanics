using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class Part : MonoBehaviour
    {
        public UnityEngine.Camera RenderCamera;
        public RawImage PartImage;
        public Transform PartPivot;

        private Crafting.TankCraftingManager tankCraftingManager;
        private RenderTexture renderTexture;
        private Mechanics.Part part;

        void Start()
        {
            if (RenderCamera == null)
            {
                throw new ArgumentNullException("RenderCamera", "Missing RenderCamera object.");
            }
            if (PartImage == null)
            {
                throw new ArgumentNullException("PartImage", "Missing PartImage object.");
            }
            renderTexture = new RenderTexture(128, 128, 32, RenderTextureFormat.ARGB32);
            renderTexture.Create();
            RenderCamera.targetTexture = renderTexture;
            PartImage.texture = renderTexture;
        }

        public void SetPart(Mechanics.Part _part, Crafting.TankCraftingManager _tankCraftingManager)
        {
            tankCraftingManager = _tankCraftingManager;
            part = _part;
            var uiRotatingPart = Instantiate(part, PartPivot);
            var controllers = uiRotatingPart.GetComponentsInChildren<Mechanics.IController>();
            foreach (var controller in controllers)
            {
                controller.Disable();
            }
            var cameras = uiRotatingPart.GetComponentsInChildren<UnityEngine.Camera>();
            foreach (var camera in cameras)
            {
                Destroy(camera.gameObject);
            }
            foreach (var tr in uiRotatingPart.GetComponentsInChildren<Transform>())
            {
                tr.gameObject.layer = LayerMask.NameToLayer("UI");
            }
            var rb = uiRotatingPart.GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = false;
                var joint = rb.GetComponent<Joint>();
                if (joint == null)
                {
                    joint = rb.gameObject.AddComponent<FixedJoint>();
                }
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedBody = PartPivot.GetComponent<Rigidbody>();
                joint.connectedAnchor = new Vector3(0,0,0);
                joint.anchor = new Vector3(0, 0, 0);
            }
        }

        public void Click()
        {
            tankCraftingManager.SetPart(part);
        }

        void Update()
        {

        }
    }
}