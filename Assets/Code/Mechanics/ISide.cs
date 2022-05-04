using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public interface ISide
    {
        SideE GetSide();
    }

    public enum SideE
    {
        Left,
        Right
    }
}
