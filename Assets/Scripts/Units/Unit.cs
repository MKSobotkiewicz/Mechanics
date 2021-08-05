using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Project.Units
{
    public class Unit : MonoBehaviour, IMovable
    {
        public UnitType UnitType { get; private set; }

        private UI.Unit uiElement;
        private Map.Area location;
        private List<Map.Area> path;
        private float remainingTravelToNextArea;

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
            return UnitType.Speed;
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

        private void UpdatePosition()
        {
            transform.position = location.transform.position;
            transform.rotation = location.transform.rotation;
        }
    }
}
