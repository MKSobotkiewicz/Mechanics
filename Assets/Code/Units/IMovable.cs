using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Project.Units
{
    public interface IMovable:Time.IHourly
    {
        Map.Area Location();
        Pathfinding.Path Path();
        bool Move(Map.Area target);
        float Speed();
        float RemainingTravelToNextArea();
    }
}
