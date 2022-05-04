using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class UnitBar:Follower
    {
        public Image Icon;
        public RectTransform ManpowerBar;
        public RectTransform CohesionBar;

        private Units.Unit unit;

        private void UpdateIcon(Units.Unit _unit)
        {
            unit = _unit;
            Icon.sprite = unit.Icon;
            Icon.color = unit.GetOrganization().Color;
        }

        public static UnitBar Create(Canvas canvas, Units.Unit _unit)
        {
            var go = Instantiate(UnityEngine.Resources.Load("UI/Prefabs/Units/UnitBarCanvas"),canvas.transform)as GameObject;
            var ub = go.GetComponent<UnitBar>();
            ub.UpdateFollowed(_unit);
            ub.UpdateIcon(_unit);
            return ub;
        }
    }
}
