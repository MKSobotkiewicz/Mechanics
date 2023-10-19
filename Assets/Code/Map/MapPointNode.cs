using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Map
{
    public class MapPointNode:Pathfinding.PointNode
    {
        public new Area Area;
        public MapPointNode(AstarPath astar, Area area) : base(astar) => Area = area;
    }
}
