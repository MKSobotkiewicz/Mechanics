using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class TankController : MonoBehaviour,IController
    {
        private TurnE turn = TurnE.None;
        private MoveE move = MoveE.None;
        private TankWheel[] tankWheels;
        private Track[] tracks;
        private Cannon cannon;
        private EngineController engineController;
        private TurretController turretController;
        private Material.CameraOverlay cameraOvelay;
        private new Rigidbody rigidbody;
        private static readonly float tracksMultiplier=0.003f;
        private float stopTimer = 0;
        private MeshRenderer[] meshRenderers;
        private bool destroyed = false;

        public void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody==null)
            {
                Debug.LogError(gameObject.name + " misses rigidbody");
            }
            engineController = GetComponentInChildren<EngineController>();
            if (engineController == null)
            {
                return;
            }
            engineController.SetTracks();
            tankWheels = GetComponentsInChildren<TankWheel>();
            tracks= GetComponentsInChildren<Track>();
            foreach (var wheel in tankWheels)
            {
                wheel.SetMaxSpeed(engineController.MaxSpeed);
            }
            cannon = GetComponentInChildren<Cannon>();
            turretController = GetComponentInChildren<TurretController>();
            cameraOvelay = GetComponentInChildren<Material.CameraOverlay>();
        }
        
        public void Update()
        {
        }

        public void FixedUpdate()
        {
            WheelsControl();
            TracksControl();
            MomentumControl();
        }

        public void Fire()
        {
            if (cannon != null)
            {
                if (cannon.Fire())
                {
                    cameraOvelay.SetJitter(0.2f);
                }
            }
        }

        public void Hit()
        {
            cameraOvelay.SetJitter(1f);
        }

        public void Destroy()
        {
            if (destroyed)
            {
                return;
            }
            destroyed = true;
            Disable();
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
            LeanTween.value(0, 1, 5).setOnUpdate(UpdateMaterial);
            turretController.FireHatch();
        }

        private void UpdateMaterial(float value)
        {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material.SetFloat("Vector1_f2f5fcf31fc14412ace87b608ffe83b0", value);
            }

        }

        public void ChangeAmmo()
        {
            cannon.ChangeProjectile();
        }

        private void TracksControl()
        {
            foreach (var track in tracks)
            {
                switch (track.GetSide())
                {
                    case SideE.Left:
                        track.SetSpeed(-engineController.LeftEngineWheel.AngularVelocity()* tracksMultiplier);
                        break;
                    case SideE.Right:
                        track.SetSpeed(engineController.RightEngineWheel.AngularVelocity()* tracksMultiplier);
                        break;
                }
            }
        }

        public void Disable()
        {
            enabled = false;
        }

        private void WheelsControl()
        {
            switch (move)
            {
                case MoveE.None:
                    switch (turn)
                    {
                        case TurnE.None:
                            foreach (var wheel in tankWheels)
                            {
                                {
                                    wheel.Stop();
                                }
                            }
                            break;
                        case TurnE.Right:
                            switch (engineController.NeutralSteering)
                            {
                                case true:
                                    foreach (var wheel in tankWheels)
                                    {
                                        switch (wheel.Side)
                                        {
                                            case SideE.Left:
                                                wheel.Accelerate(engineController.Acceleration);
                                                break;
                                            case SideE.Right:
                                                wheel.Decelerate(engineController.Acceleration);
                                                break;
                                        }
                                    }
                                    break;
                                case false:
                                    foreach (var wheel in tankWheels)
                                    {
                                        switch (wheel.Side)
                                        {
                                            case SideE.Left:
                                                wheel.Accelerate(2 * engineController.Acceleration);
                                                break;
                                            case SideE.Right:
                                                wheel.Stop();
                                                break;
                                        }
                                    }
                                    break;
                            }
                            break;
                        case TurnE.Left:
                            switch (engineController.NeutralSteering)
                            {
                                case true:
                                    foreach (var wheel in tankWheels)
                                    {
                                        switch (wheel.Side)
                                        {
                                            case SideE.Left:
                                                wheel.Decelerate(engineController.Acceleration);
                                                break;
                                            case SideE.Right:
                                                wheel.Accelerate(engineController.Acceleration);
                                                break;
                                        }
                                    }
                                    break;
                                case false:
                                    foreach (var wheel in tankWheels)
                                    {
                                        switch (wheel.Side)
                                        {
                                            case SideE.Left:
                                                wheel.Stop();
                                                break;
                                            case SideE.Right:
                                                wheel.Accelerate(2 * engineController.Acceleration);
                                                break;
                                        }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case MoveE.Forward:
                    switch (turn)
                    {
                        case TurnE.None:
                            if (rigidbody.velocity.magnitude < 1)
                            {
                                stopTimer += UnityEngine.Time.fixedDeltaTime * 10;
                            }
                            else
                            {
                                stopTimer = 0;
                            }
                            //rigidbody.AddRelativeForce(new Vector3(0, 0, engineController.Acceleration * Mathf.Clamp(stopTimer - 5, 0, 100) * 10000), ForceMode.Force);
                            rigidbody.AddRelativeForce(new Vector3(0, 0, engineController.Acceleration*10000), ForceMode.Force);
                            foreach (var wheel in tankWheels)
                            {
                                if (transform.InverseTransformDirection(rigidbody.velocity).z < -1)
                                {
                                    wheel.Stop();
                                }
                                else
                                {
                                    wheel.Accelerate(engineController.Acceleration);
                                }
                            }
                            break;
                        case TurnE.Right:
                            stopTimer = 0;
                            switch (engineController.NeutralSteering)
                            {
                                case true:
                                    foreach (var wheel in tankWheels)
                                    {
                                        switch (wheel.Side)
                                        {
                                            case SideE.Left:
                                                wheel.Accelerate(2 * engineController.Acceleration);
                                                //wheel.Accelerate(engineController.Acceleration);
                                                break;
                                            case SideE.Right:
                                                //wheel.Brake(engineController.Acceleration);
                                                wheel.Stop();
                                                break;
                                        }
                                    }
                                    break;
                                case false:
                                    foreach (var wheel in tankWheels)
                                    {
                                        switch (wheel.Side)
                                        {
                                            case SideE.Left:
                                                wheel.Accelerate(2 * engineController.Acceleration);
                                                break;
                                            case SideE.Right:
                                                wheel.Stop();
                                                break;
                                        }
                                    }
                                    break;
                            }
                            break;
                        case TurnE.Left:
                            stopTimer = 0;
                            switch (engineController.NeutralSteering)
                            {
                                case true:
                                    foreach (var wheel in tankWheels)
                                    {
                                        switch (wheel.Side)
                                        {
                                            case SideE.Left:
                                                wheel.Stop();
                                                //wheel.Brake(engineController.Acceleration);
                                                break;
                                            case SideE.Right:
                                                //wheel.Accelerate(engineController.Acceleration);
                                                wheel.Accelerate(2 * engineController.Acceleration);
                                                break;
                                        }
                                    }
                                    break;
                                case false:
                                    foreach (var wheel in tankWheels)
                                    {
                                        switch (wheel.Side)
                                        {
                                            case SideE.Left:
                                                wheel.Stop();
                                                break;
                                            case SideE.Right:
                                                wheel.Accelerate(2 * engineController.Acceleration);
                                                break;
                                        }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;

                case MoveE.Backwards:
                    switch (turn)
                    {
                        case TurnE.None:
                            if (rigidbody.velocity.magnitude < 1)
                            {
                                stopTimer += UnityEngine.Time.fixedDeltaTime * 10;
                            }
                            else
                            {
                                stopTimer = 0;
                            }
                            //rigidbody.AddRelativeForce(new Vector3(0, 0, -engineController.Acceleration * Mathf.Clamp(stopTimer - 5, 0, 100) * 10000), ForceMode.Force);
                            rigidbody.AddRelativeForce(new Vector3(0, 0, -engineController.Acceleration * 10000), ForceMode.Force);
                            foreach (var wheel in tankWheels)
                            {
                                if (transform.InverseTransformDirection(rigidbody.velocity).z > 1)
                                {
                                    wheel.Stop();
                                }
                                else
                                {
                                    wheel.Decelerate(engineController.Acceleration);
                                }
                            }
                            break;
                        case TurnE.Right:
                            stopTimer = 0;
                            switch (engineController.NeutralSteering)
                            {
                                case true:
                                    foreach (var wheel in tankWheels)
                                    {
                                        switch (wheel.Side)
                                        {
                                            case SideE.Left:
                                                wheel.Decelerate(2 * engineController.Acceleration);
                                                //wheel.Decelerate(engineController.Acceleration);
                                                break;
                                            case SideE.Right:
                                                wheel.Stop();
                                                //wheel.Brake(engineController.Acceleration);
                                                break;
                                        }
                                    }
                                    break;
                                case false:
                                    foreach (var wheel in tankWheels)
                                    {
                                        switch (wheel.Side)
                                        {
                                            case SideE.Left:
                                                wheel.Decelerate(2 * engineController.Acceleration);
                                                break;
                                            case SideE.Right:
                                                wheel.Stop();
                                                break;
                                        }
                                    }
                                    break;
                            }
                            break;
                        case TurnE.Left:
                            stopTimer = 0;
                            switch (engineController.NeutralSteering)
                            {
                                case true:
                                    foreach (var wheel in tankWheels)
                                    {
                                        switch (wheel.Side)
                                        {
                                            case SideE.Left:
                                                wheel.Stop();
                                                //wheel.Brake(engineController.Acceleration);
                                                break;
                                            case SideE.Right:
                                                //wheel.Decelerate(engineController.Acceleration);
                                                wheel.Decelerate(2 * engineController.Acceleration);
                                                break;
                                        }
                                    }
                                    break;
                                case false:
                                    foreach (var wheel in tankWheels)
                                    {
                                        switch (wheel.Side)
                                        {
                                            case SideE.Left:
                                                wheel.Stop();
                                                break;
                                            case SideE.Right:
                                                wheel.Decelerate(2 * engineController.Acceleration);
                                                break;
                                        }
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }

        private void MomentumControl()
        {
            switch (move)
            {
                case MoveE.None:
                    break;
                case MoveE.Forward:
                    rigidbody.AddRelativeTorque(new Vector3(Mathf.Clamp(-engineController.Acceleration * 10000,-100000, 1000000),0,0),ForceMode.Force);
                    break;
                case MoveE.Backwards:
                    rigidbody.AddRelativeTorque(new Vector3(Mathf.Clamp(engineController.Acceleration * 10000, -100000, 1000000), 0, 0), ForceMode.Force);
                    break;
            }
        }

        public void Move(MoveE _move, TurnE _turn)
        {
            move = _move;
            turn = _turn;
        }

        public enum TurnE
        {
            None,
            Left,
            Right
        }

        public enum MoveE
        {
            None,
            Forward,
            Backwards
        }
    }
}
