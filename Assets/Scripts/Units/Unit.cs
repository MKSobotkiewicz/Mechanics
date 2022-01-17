﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Project.Units
{
    public class Unit : MonoBehaviour, IMovable, Utility.IClicable, UI.IFollowed
    {
        public bool Selected { get; private set; } = false;
        public UnitPath UnitPath;
        public float speed=1;
        public Sprite Icon;
        public uint MaxManpower=100;
        public uint MaxCohesion=100;
        
        private UI.Unit uiElement;
        private UI.UnitBar unitBar;
        private Player.Player player;
        private Organizations.Organization organization;
        private Map.Area location;
        private List<Map.Area> path;
        private float remainingTravelToNextArea;
        private List<Animator> animators=new List<Animator>();
        private LayerMask layerMask;
        private GameObject soldiers;
        private uint manpower;
        private uint cohesion;

        public static HashSet<Unit> AllUnits { get; private set; } = new HashSet<Unit>();

        public void Init(Map.Area _location, Player.Player _player, Organizations.Organization _organization,Time.Time time)
        {
            animators = GetComponentsInChildren<Animator>().ToList();
            time.AddHourly(this);
            player = _player;
            location = _location;
            organization = _organization;
            manpower = MaxManpower;
            cohesion = MaxCohesion;
            UpdatePosition();
            AllUnits.Add(this);
            transform.LookAt(location.Neighbours[0].Position, transform.position.normalized);
            CreateUIBar();
            foreach (var animator in animators)
            {
                animator.SetBool("Moving", false);
            }
            layerMask = ~LayerMask.GetMask("Unit","Outlined");
            var soldiersLM = LayerMask.NameToLayer("Soldiers");
            foreach (var tr in transform.GetComponentsInChildren<Transform>())
            {
                if (tr.gameObject.layer == soldiersLM)
                {
                    soldiers = tr.gameObject;
                    break;
                }
            }
        }

        public string Name()
        {
            return name;
        }

        public Vector3 FollowedPosition()
        {
            return Vector3.RotateTowards(transform.position, Vector3.down, 0.01f, 0);
        }

        public Map.Area Location()
        {
            return location;
        }

        public List<Map.Area> Path()
        {
            return path;
        }

        public void Click()
        {
        }

        public void Order()
        {
            //Attack or something
        }

        public void Unclick()
        {
            if (player.SelectedUnits.Count == 0)
            {
                player.SelectUnit(this);
            }
        }

        public void SetSelected(bool value)
        {
            /*if (value == Selected)
            {
                return;
            }*/
            var outlinedLM = LayerMask.NameToLayer("Outlined");
            var unitLM = LayerMask.NameToLayer("Unit");
            var tranparentLM = LayerMask.NameToLayer("Transparent");
            Selected = value;
            if (Selected)
            {
                UnitPath.Show();
                player.SelectedUnits.Add(this);
                Debug.Log("Selected");
                foreach (var tr in transform.GetComponentsInChildren<Transform>())
                {
                    if (tr.gameObject.layer != tranparentLM)
                    {
                        tr.gameObject.layer = outlinedLM;
                    }
                }
            }
            else
            {
                UnitPath.Hide();
                player.SelectedUnits.Remove(this);
                foreach (var tr in transform.GetComponentsInChildren<Transform>())
                {
                    if (tr.gameObject.layer != tranparentLM)
                    {
                        tr.gameObject.layer = unitLM;
                    }
                }
            }
        }

        public float Speed()
        {
            return speed;
        }

        public Organizations.Organization GetOrganization()
        {
            return organization;
        }

        public float RemainingTravelToNextArea()
        {
            return remainingTravelToNextArea;
        }

        public bool Move(Map.Area target)
        {
            path = Utility.Pathfinder.FindPath(location,target);
            if (path!=null)
            {
                //uiElement.PathSuccess(path);
                UpdatePosition();
                foreach (var animator in animators)
                {
                    animator.SetBool("Moving", true);
                }
                if (soldiers != null)
                {
                    soldiers.SetActive(false);
                }
                return true;
            }
            //uiElement.PathFail();
            return false;
        }

        public void HourlyUpdate()
        {
            if (path == null)
            {
                return;
            }
            if (path.Count == 0)
            {
                return;
            }
            remainingTravelToNextArea -= Speed();
            while (remainingTravelToNextArea <= 0)
            {
                if (path== null)
                { 
                    break;
                }
                Debug.Log("MOVE");
                path.RemoveAt(0);
                location = path[0];
                if (path.Count > 1)
                {
                    remainingTravelToNextArea += path[0].Weight();
                }
                else if (path.Count == 1)
                {
                    UnitPath.Destroy();
                    foreach (var animator in animators)
                    {
                        animator.SetBool("Moving", false);
                    }

                    if (soldiers != null)
                    {
                        soldiers.SetActive(true);
                        SetSelected(Selected);
                    }
                    path = null;
                }
                UpdatePosition();
            }
        }

        public void UpdatePosition()
        {
            var hit = new RaycastHit();
            if (path != null)
            {
                if (path.Count > 1)
                {
                    if (Physics.Raycast(Vector3.Lerp(location.Position,path[1].Position,0.25f) * 1.1f, -location.Position.normalized, out hit, Mathf.Infinity, layerMask))
                    {
                        transform.position = hit.point;
                        transform.LookAt(path[1].Position, transform.position.normalized);
                        UnitPath.Destroy();
                        UnitPath.Create(this,path);
                    }
                    return;
                }
            }
            if (Physics.Raycast(location.Position * 1.1f, -location.Position.normalized, out hit, Mathf.Infinity))
            {
                transform.position = hit.point;
            }
        }

        public void CreateUIBar()
        {
            unitBar = UI.UnitBar.Create(UnityEngine.Camera.main.GetComponentInChildren<Canvas>(),this);
        }

        public void UpdateUIBarValues()
        {

        }
    }
}
