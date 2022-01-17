using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Map
{
    public class MapData
    {
        public List<Area> Areas { get; private set; } = new List<Area>();
        public List<Area> WaterAreas { get; private set; } = new List<Area>();
        public List<Area> MountainAreas { get; private set; } = new List<Area>();
        public List<Area> HillsAreas { get; private set; } = new List<Area>();
        public List<Area> PlainsAreas { get; private set; } = new List<Area>();
        public List<AreaGroup> AreaGroups { get; private set; } = new List<AreaGroup>();
        public MeshFilter meshFilter;

        public List<Area> PossibleAreas()
        {
            var possibleAreas = new List<Area>(PlainsAreas);
            possibleAreas.AddRange(HillsAreas);
            return possibleAreas;
        }
    }
}
