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
        private static float spentTime = 0;

        public static void LogSpentTime()
        {
            Debug.Log("Pathfinder spent time: "+ spentTime);
        }

        public static List<Area> FindPath(
            Area start, 
            Area goal, 
            List<Area> areas,
            int maxCheckDistance=2, 
            Dictionary<Area, float> distance = null,
            Dictionary<Area, Area> previous=null, 
            List<Tuple<float, Area>> unvisited =null)
        {
            float startTime = UnityEngine.Time.realtimeSinceStartup;
            float stopTime;
            var maxDistance = Vector3.Distance(start.Position, goal.Position) * maxCheckDistance;
            if (distance == null|| previous == null||unvisited==null)
            {
                distance = new Dictionary<Area, float>();
                previous = new Dictionary<Area, Area>();
                unvisited = new List<Tuple<float, Area>>();
                distance[start] = 0;
                previous[start] = null;
                foreach (Area x in areas)
                {
                    if (x != start)
                    {
                        distance[x] = Mathf.Infinity;
                        previous[x] = null;
                    }
                    var distanceWeigth = Vector3.Distance(x.Position, start.Position) + Vector3.Distance(x.Position, goal.Position);
                    if (distanceWeigth < maxDistance)
                    {
                        unvisited.Add(new Tuple<float, Area>(distanceWeigth, x));
                    }
                }
                unvisited.OrderBy(u => u.Item1);
            }
            var Path = new List<Area>();

            while (unvisited.Count > 0)
            {
                Tuple<float, Area> u = null;
                foreach (var possibleU in unvisited)
                {
                    if (u == null)
                    {
                        u = possibleU;
                    }
                    if (distance[possibleU.Item2] < distance[u.Item2])
                    {
                        u = possibleU;
                    }
                }
                if (u.Item2 == goal)
                {
                    break;
                }
                unvisited.Remove(u);
                foreach (var x in u.Item2.GetNeighboursWithDistance())
                {
                    var alt = distance[u.Item2] + x.Item2;
                    if (alt < distance[x.Item1])
                    {
                        distance[x.Item1] = alt;
                        previous[x.Item1] = u.Item2;
                    }
                }
            }
            if (previous[goal] == null)
            {
                //Debug.Log("Cant Reach Goal.");
                if (maxCheckDistance >= 100)
                {
                    stopTime = UnityEngine.Time.realtimeSinceStartup;
                    spentTime += stopTime - startTime;
                    return null;
                }
                else
                {
                    maxCheckDistance *= 2;
                    Path = FindPath(start, goal, areas, maxCheckDistance,distance,previous,unvisited);
                    stopTime = UnityEngine.Time.realtimeSinceStartup;
                    spentTime += stopTime - startTime;
                    return Path;
                }
            }
            //Debug.Log("Reached Goal.");
            Area current = goal;
            while (current != null)
            {
                Path.Add(current);
                current = previous[current];
            }
            Path.Reverse();
            stopTime = UnityEngine.Time.realtimeSinceStartup;
            spentTime += stopTime - startTime;
            return Path;
        }
    }
}
