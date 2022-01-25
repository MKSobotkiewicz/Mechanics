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
            bool ignoreWeights = false,
            AreaGroup areas = null,
            int maxCheckDistance = 2, 
            Dictionary<Area, float> distance = null,
            Dictionary<Area, Area> previous = null, 
            List<Tuple<float, Area>> unvisited = null)
        {
            if (areas == null)
            {
                areas = start.AreaGroup;
            }
            float startTime = UnityEngine.Time.realtimeSinceStartup;
            float stopTime;
            var maxDistance = Vector3.Distance(start.Position, goal.Position) * maxCheckDistance;
            if (distance == null|| previous == null||unvisited==null)
            {
                distance = new Dictionary<Area, float>();
                previous = new Dictionary<Area, Area>();
                unvisited = new List<Tuple<float, Area>>();
                distance[start] = 0;
                foreach (Area x in areas)
                {
                    previous[x]=null;
                    if (x != start)
                    {
                        distance[x] = Mathf.Infinity;
                    }
                    var distanceWeigth = Vector3.Distance(x.Position, start.Position) + Vector3.Distance(x.Position, goal.Position);
                    if (distanceWeigth < maxDistance)
                    {
                        unvisited.Add(new Tuple<float, Area>(distanceWeigth, x));
                    }
                }
                unvisited.OrderBy(u => u.Item1);
            }

            while (unvisited.Count > 0)
            {
                Tuple<float, Area> area = null;
                foreach (var possibleU in unvisited)
                {
                    if (area == null)
                    {
                        area = possibleU;
                    }
                    else if (distance[possibleU.Item2] < distance[area.Item2])
                    {
                        area = possibleU;
                    }
                }
                if (area.Item2 == goal)
                {
                    break;
                }
                unvisited.Remove(area);
                foreach (var x in area.Item2.Neighbours)
                {
                    if (x.Type == Area.EType.Mountains || x.Type == Area.EType.Water)
                    {
                        continue;
                    }
                    var alt = ignoreWeights? distance[area.Item2] + 1f : distance[area.Item2] + x.Weight();
                    if (alt < distance[x])
                    {
                        distance[x] = alt;
                        previous[x] = area.Item2;
                    }
                }
            }
            var Path = new List<Area>();
            if (previous[goal] == null)
            {
                if (maxCheckDistance >= 10)
                {
                    Debug.Log("Cant Reach Goal.");
                    stopTime = UnityEngine.Time.realtimeSinceStartup;
                    spentTime += stopTime - startTime;
                    return null;
                }
                else
                {
                    maxCheckDistance *= 2;
                    Path = FindPath(start, goal, ignoreWeights, areas, maxCheckDistance,distance,previous,unvisited);
                    stopTime = UnityEngine.Time.realtimeSinceStartup;
                    spentTime += stopTime - startTime;
                    return Path;
                }
            }
            Debug.Log("Reached Goal, maxCheckDistance: "+ maxCheckDistance);
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
