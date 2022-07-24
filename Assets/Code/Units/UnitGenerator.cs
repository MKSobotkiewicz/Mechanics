using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Project.Units
{
    public class UnitGenerator : MonoBehaviour
    {
        public List<Player.Player> Players;
        public Time.Time Time;
        public List<Unit> Units;
        public Transform UnitPaths;
        public int playerId;

        public Unit Generate(Unit unitType, Map.Area area)
        {
            foreach (var unit in Units)
            {
                if (unit == unitType)
                {
                    unit.Template.LoadFromXml();
                    var newUnitGO=Instantiate(unit.gameObject);
                    var newUnit = newUnitGO.GetComponent<Unit>();
                    newUnit.Init(area, Players[playerId], Players[playerId].GetComponent<Organizations.Organization>(), UnitPaths, Time);
                    return newUnit;
                }
            }
            return null;
        }
    }
}