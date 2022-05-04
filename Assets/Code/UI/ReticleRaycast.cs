using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class ReticleRaycast : MonoBehaviour
    {
        private RectTransform mainReticleTransform;
        private RectTransform turretReticleTransform;
        private UnityEngine.Camera mainCamera;
        private UnityEngine.Camera turretCamera;
        private RectTransform mainRectTransform;
        private RectTransform turretRectTransform;
        private RectTransform turretDropRectTransform;
        private Mechanics.Cannon cannon;

        public void Start()
        {
            mainCamera = UnityEngine.Camera.main;
            mainRectTransform = mainCamera.GetComponentInChildren<RectTransform>();
            var rectTransforms = mainCamera.GetComponentsInChildren<RectTransform>();
            foreach (var tr in rectTransforms)
            {
                if (tr.gameObject.tag == "Reticle")
                {
                    mainReticleTransform = tr;
                    break;
                }
            }
            var turretController = GetComponentInParent<Mechanics.TurretController>();
            if (turretController==null)
            {
                Debug.LogError("missing turretController");
                return;
            }
            cannon = turretController.GetComponentInChildren<Mechanics.Cannon>();
            if (cannon == null)
            {
                Debug.LogError("missing cannon");
                return;
            }
            if ((turretCamera = turretController.GetComponentInChildren<UnityEngine.Camera>()) == null)
            {
                Debug.LogError("missing turretCamera");
                return;
            }
            turretRectTransform = turretCamera.GetComponentInChildren<RectTransform>();
            rectTransforms = turretCamera.GetComponentsInChildren<RectTransform>();
            foreach (var tr in rectTransforms)
            {
                if (tr.gameObject.tag == "Reticle")
                {
                    turretReticleTransform = tr;
                    continue;
                }
                if (tr.gameObject.tag == "DropReticle")
                {
                    turretDropRectTransform = tr;
                }
            }


        }

        public void Update()
        {
            if (mainReticleTransform == null || turretReticleTransform == null)
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.forward * 2, transform.forward, out hit, 2000, ~(1 << 10)))
            {
                Vector2 mainPosition;
                var screenPoint = RectTransformUtility.ScreenPointToLocalPointInRectangle(mainRectTransform, mainCamera.WorldToScreenPoint(hit.point), mainCamera, out mainPosition);
                mainReticleTransform.anchoredPosition = new Vector3(mainPosition.x, mainPosition.y, 0);
                Vector2 turretPosition;
                screenPoint = RectTransformUtility.ScreenPointToLocalPointInRectangle(turretRectTransform, turretCamera.WorldToScreenPoint(hit.point), turretCamera, out turretPosition);
                turretReticleTransform.anchoredPosition = new Vector3(turretPosition.x, turretPosition.y, 0);
                Vector2 turretDropPosition;
                Vector3 drop=new Vector3(0,cannon.GetDrop(hit.distance),0);
                screenPoint = RectTransformUtility.ScreenPointToLocalPointInRectangle(turretRectTransform, turretCamera.WorldToScreenPoint(hit.point+ drop), turretCamera, out turretDropPosition);
                turretDropRectTransform.anchoredPosition = new Vector3(turretDropPosition.x, turretDropPosition.y, 0);
            }
            else
            {
                Vector2 mainPosition;
                var screenPoint = RectTransformUtility.ScreenPointToLocalPointInRectangle(mainRectTransform, mainCamera.WorldToScreenPoint(transform.position + transform.forward * 2000), mainCamera, out mainPosition);
                mainReticleTransform.anchoredPosition = new Vector3(mainPosition.x, mainPosition.y, 0);
                Vector2 turretPosition;
                screenPoint = RectTransformUtility.ScreenPointToLocalPointInRectangle(turretRectTransform, turretCamera.WorldToScreenPoint(transform.position + transform.forward * 2000), turretCamera, out turretPosition);
                turretReticleTransform.anchoredPosition = new Vector3(turretPosition.x, turretPosition.y, 0);
                turretDropRectTransform.anchoredPosition = new Vector3(10000, 0, 0);
            }
        }
    }
}
