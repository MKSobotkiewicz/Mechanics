using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Project.Units
{
    public class UnitGenerator : MonoBehaviour
    {
        public List<Unit> Units;

        public Unit Generate(Unit unitType, Map.Area area)
        {
            foreach (var unit in Units)
            {
                if (unit == unitType)
                {
                    var newUnitGO=Instantiate(unit.gameObject,area.transform);
                    //newUnit.transform.position = area.transform.position;
                    //newUnit.transform.rotation = area.transform.rotation;
                    return newUnitGO.GetComponent<Unit>();
                }
            }
            return null;
        }
    }
}
