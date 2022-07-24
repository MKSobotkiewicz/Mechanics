using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Project.UI
{
    public class Unit:MonoBehaviour, IPointerClickHandler
    {
        public RectTransform RectTransform;
        public Image Outline;
        public RawImage Flag;
        public Image UnitIcon;
        public Text MaxAttack;

        public UiBar ManpowerBar;
        public UiBar CohesionBar;
        public UiBar SupplyBar;

        private Units.Unit unitGameobject;
        private Utility.MouseRaycasting mouseRaycasting;

        public void Start()
        {
            mouseRaycasting = UnityEngine.Camera.main.GetComponentInChildren<Utility.MouseRaycasting>();
        }

        public void Update()
        {
            var position= UnityEngine.Camera.main.WorldToScreenPoint(unitGameobject.transform.position);
            position.x = (int)position.x;
            position.y = (int)position.y;
            RectTransform.anchoredPosition = position;
        }

        public void Init(Units.Unit _unitGameobject)
        {
            unitGameobject = _unitGameobject;
            Flag.texture = unitGameobject.GetOrganization().Flag;
            UnitIcon.material = unitGameobject.Template.UnitMaterial;
            UpdateValues();
        }

        public void UpdateSupply()
        {
            SupplyBar.UpdateValue(unitGameobject.GetSupplyRatio());
        }

        public void UpdateWhenAttacked()
        {
            MaxAttack.text = unitGameobject.GetMaxManpowerAttack().ToString();
            ManpowerBar.UpdateValue(unitGameobject.GetManpowerRatio());
            CohesionBar.UpdateValue(unitGameobject.GetCohesionRatio());
        }

        public void UpdateValues()
        {
            MaxAttack.text = unitGameobject.GetMaxManpowerAttack().ToString();
            ManpowerBar.UpdateValue(unitGameobject.GetManpowerRatio());
            CohesionBar.UpdateValue(unitGameobject.GetCohesionRatio());
            SupplyBar.UpdateValue(unitGameobject.GetSupplyRatio());
        }

        public void PathFail()
        {
        }

        public void PathSuccess(List<Map.Area> path)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            foreach (var unit in Units.Unit.AllUnits)
            {
                unit.SetSelected(false);
            }
            mouseRaycasting.Break();
            unitGameobject.SetSelected(true);
        }

        public void Select()
        {
            Outline.enabled = true;
        }
        public void Deselect()
        {
            Outline.enabled = false;
        }
    }
}
