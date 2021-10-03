using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Project.Units
{
    public class Unit : MonoBehaviour, IMovable, Utility.IClicable
    {
        public bool Selected { get; private set; } = false;
        public UnitPath UnitPath;
        public float speed=1;
        public List<GameObject> MovingEffects;

        private UI.Unit uiElement;
        private Player.Player player;
        private Map.Area location;
        private List<Map.Area> path;
        private float remainingTravelToNextArea;
        private Animator animator;
        private LayerMask layerMask;

        public static HashSet<Unit> AllUnits { get; private set; } = new HashSet<Unit>();

        public void Init(Map.Area _location, Player.Player _player,Time.Time time)
        {
            animator = GetComponent<Animator>();
            time.AddHourly(this);
            player = _player;
            location = _location;
            UpdatePosition();
            AllUnits.Add(this);
            transform.LookAt(location.Neighbours[0].Position, transform.position.normalized);
            animator.SetBool("Moving", false);
            layerMask = ~LayerMask.GetMask("Unit","Outlined");
            foreach (var movingEffect in MovingEffects)
            {
                movingEffect.SetActive(false);
            }
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
            if (value == Selected)
            {
                return;
            }
            Selected = value;
            if (Selected)
            {
                UnitPath.Show();
                player.SelectedUnits.Add(this);
                Debug.Log("Selected");
                foreach (var tr in transform.GetComponentsInChildren<Transform>())
                {
                    tr.gameObject.layer = LayerMask.NameToLayer("Outlined");
                }
            }
            else
            {
                UnitPath.Hide();
                player.SelectedUnits.Remove(this);
                foreach (var tr in transform.GetComponentsInChildren<Transform>())
                {
                    tr.gameObject.layer = LayerMask.NameToLayer("Unit");
                }
            }
        }

        public float Speed()
        {
            return speed;
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
                animator.SetBool("Moving",true);
                foreach (var movingEffect in MovingEffects)
                {
                    movingEffect.SetActive(true);
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
                    animator.SetBool("Moving", false);
                    foreach (var movingEffect in MovingEffects)
                    {
                        movingEffect.SetActive(false);
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
    }
}
