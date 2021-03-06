using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Project.Units
{
    public class Unit : MonoBehaviour, IMovable
    {
        public float _Speed;

        private UI.Unit uiElement;
        private Map.Area location;
        private List<Map.Area> path;
        private float remainingTravelToNextArea;

        public void Init(Map.Area _location)
        {
            location = _location;
            UpdatePosition();
        }

        public Map.Area Location()
        {
            return location;
        }

        public List<Map.Area> Path()
        {
            return path;
        }

        public float Speed()
        {
            return _Speed;
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
                uiElement.PathSuccess(path);
                return true;
            }
            uiElement.PathFail();
            return false;
        }

        public void HourlyUpdate()
        {
            remainingTravelToNextArea -= Speed();
            while (remainingTravelToNextArea <= 0)
            {
                location = path[0];
                path.RemoveAt(0);
                remainingTravelToNextArea += path[0].Weight();
                UpdatePosition();
            }
        }

        public void UpdatePosition()
        {
            var hit = new RaycastHit();
            if (Physics.Raycast(location.Position*1.1f, -location.Position.normalized, out hit, Mathf.Infinity))
            {
                transform.position = hit.point;
            }
            transform.LookAt(transform.position*2);
        }
    }
}
