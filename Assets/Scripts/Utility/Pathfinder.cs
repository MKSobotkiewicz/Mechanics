using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Project.Map;

namespace Project.Utility
{
    public static class Pathfinder
    {

        public static List<Area> FindPath(Area start, Area goal, List<Area> areas)
        {
            Dictionary<Area, float> distance = new Dictionary<Area, float>();
            Dictionary<Area, Area> previous = new Dictionary<Area, Area>();

            List<Area> Unvisited = new List<Area>();

            List<Area> Path = null;

            distance[start] = 0;
            previous[start] = null;
            foreach (Area x in areas)
            {
                if (x != start)
                {
                    distance[x] = Mathf.Infinity;
                    previous[x] = null;
                }
                Unvisited.Add(x);
            }
            while (Unvisited.Count > 0)
            {
                Area u = null;
                foreach (var possibleU in Unvisited)
                {
                    if (u == null || distance[possibleU] < distance[u])
                    {
                        u = possibleU;
                    }
                }

                if (u == goal)
                {
                    break;
                }
                Unvisited.Remove(u);
                foreach (var x in u.GetNeighboursWithDistance())
                {
                    float alt = distance[u] + x.Item2;
                    if (alt < distance[x.Item1])
                    {
                        distance[x.Item1] = alt;
                        previous[x.Item1] = u;
                    }
                }
            }
            if (previous[goal] == null)
            {
                Debug.Log("Cant Reach Goal.");
                return null;
            }
            Debug.Log("Reached Goal.");
            Path = new List<Area>();
            Area current = goal;
            while (current != null)
            {
                Debug.Log(current);
                Path.Add(current);
                current = previous[current];
            }
            Path.Reverse();
            return Path;
        }
    }
}
