using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Project.Units
{
    public class UnitGenerator : MonoBehaviour
    {
        public Player.Player Player;
        public Organizations.Organization Organization;
        public Time.Time Time;
        public List<Unit> Units;

        public Unit Generate(Unit unitType, Map.Area area)
        {
            foreach (var unit in Units)
            {
                if (unit == unitType)
                {
                    var newUnitGO=Instantiate(unit.gameObject);
                    var newUnit = newUnitGO.GetComponent<Unit>();
                    newUnit.Init(area, Player, Organization, Time);
                    return newUnit;
                }
            }
            return null;
        }
    }
}