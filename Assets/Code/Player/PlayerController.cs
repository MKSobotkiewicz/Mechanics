using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Player
{
    public class PlayerController:MonoBehaviour
    {
        public Mechanics.TankController TankController;

        private Mechanics.TankController.MoveE move;
        private Mechanics.TankController.TurnE turn;
        private Transform linkedTransform;
        private Vector3 velocity = Vector3.zero;
        private Vector3 angularVelocity = Vector3.zero;
        private Quaternion correction;
        private bool zoomClicked = false;
        private UnityEngine.Camera mainCamera;
        private UnityEngine.Camera turretCamera;
        private Camera.Vehicle3rdPersonCamera vehicle3rdPersonCamera;
        private float defaultVehicle3rdPersonCameraRotationSpeed = 1;
        private bool avPressed=false;
        private bool caPressed = false;

        public void Start()
        {
            SetLinkedTransform();
            TankController.enabled = true;
            transform.rotation = Quaternion.Euler(new Vector3(0, linkedTransform.eulerAngles.y, 0));
            mainCamera = UnityEngine.Camera.main;
            var turretCameraBehaviour = TankController.GetComponentInChildren<Camera.TurretCameraBehaviour>();
            if (turretCameraBehaviour == null)
            {
                Debug.LogError("Missing turretCameraBehaviour");
            }
            turretCamera = turretCameraBehaviour.GetComponent<UnityEngine.Camera>();
            turretCamera.enabled = false;
            vehicle3rdPersonCamera = mainCamera.GetComponentInParent<Camera.Vehicle3rdPersonCamera>();
            if (vehicle3rdPersonCamera == null)
            {
                Debug.LogError("Missing vehicle3rdPersonCamera");
            }
            defaultVehicle3rdPersonCameraRotationSpeed = vehicle3rdPersonCamera.RotationSpeed;
            
            var rotationReferenceTransform = vehicle3rdPersonCamera.GetComponentInChildren<UnityEngine.Camera>().transform;
            if (rotationReferenceTransform == null)
            {
                Debug.LogError(" missing vehicle3rdPersonCamera.transform");
            }
            var TurretController= TankController.GetComponentInChildren<Mechanics.TurretController>();
            TurretController.SetRotationReferenceTransform(rotationReferenceTransform);
            Mechanics.Armor.SwitchArmorMaterialsVisibility();
        }

        public void FixedUpdate()
        {
            Move();
            Fire();
            Zoom();
            ArmorVisibility();
            ChangeAmmo();
        }

        public void Update()
        {
            transform.position = Vector3.SmoothDamp(transform.position, linkedTransform.position, ref velocity, 0.1f);
            transform.rotation = Quaternion.Slerp(transform.rotation, linkedTransform.rotation, 0.1f);
        }

        private void SetLinkedTransform()
        {
            var children = TankController.GetComponentsInChildren<Transform>();
            linkedTransform = TankController.transform;
        }
        private void Move()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            if (horizontal > 0)
            {
                turn = Mechanics.TankController.TurnE.Right;
            }
            else if (horizontal < 0)
            {
                turn = Mechanics.TankController.TurnE.Left;
            }
            else
            {
                turn = Mechanics.TankController.TurnE.None;
            }

            if (vertical > 0)
            {
                move = Mechanics.TankController.MoveE.Forward;
            }
            else if (vertical < 0)
            {
                move = Mechanics.TankController.MoveE.Backwards;
            }
            else
            {
                move = Mechanics.TankController.MoveE.None;
            }

            TankController.Move(move,turn);
        }

        private void Fire()
        {
            if (Input.GetAxis("Fire1") > 0)
            {
                TankController.Fire();
            }
        }

        private void Zoom()
        {
            if (Input.GetAxis("Reticle") > 0)
            {
                if (zoomClicked == false)
                {
                    zoomClicked = true;
                    if (mainCamera.enabled == false)
                    {
                        mainCamera.enabled = true;
                        turretCamera.enabled = false;
                        vehicle3rdPersonCamera.RotationSpeed = defaultVehicle3rdPersonCameraRotationSpeed;
                    }
                    else
                    {
                        mainCamera.enabled = false;
                        turretCamera.enabled = true;
                        vehicle3rdPersonCamera.RotationSpeed = defaultVehicle3rdPersonCameraRotationSpeed/10;
                    }
                    return;
                }
                return;
            }
            zoomClicked = false;
        }

        private void ArmorVisibility()
        {
            if (Input.GetAxis("ArmorVisibility") > 0)
            {
                if (!avPressed)
                {
                    Mechanics.Armor.SwitchArmorMaterialsVisibility();
                    avPressed = true;
                }
                return;
            }
            avPressed = false;
        }

        private void ChangeAmmo()
        {

            if (Input.GetAxis("ChangeAmmo") > 0)
            {
                if (!caPressed)
                {
                    TankController.ChangeAmmo();
                    caPressed = true;
                }
                return;
            }
            caPressed = false;
        }
    }
}
