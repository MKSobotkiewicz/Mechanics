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
        List<Map.Area> Path();
        bool Move(Map.Area target);
        float Speed();
        float RemainingTravelToNextArea();
    }
}
