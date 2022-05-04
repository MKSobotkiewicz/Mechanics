using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Player
{
    public class Player:MonoBehaviour
    {
        public List<Units.Unit> SelectedUnits { get; private set; } = new List<Units.Unit>();
        public Map.Area CurrentlySelectedArea { get; private set; }

        public void SelectArea(Map.Area Area)
        {
            UnselectAllUnits();
            if (CurrentlySelectedArea==Area)
            {
                return;
            }
            if (CurrentlySelectedArea != null)
            {
                CurrentlySelectedArea.Unselect();
            }
            CurrentlySelectedArea = Area;
            CurrentlySelectedArea.Select();
        }

        public void UnselectCurrentlySelectedArea()
        {
            if (CurrentlySelectedArea != null)
            {
                CurrentlySelectedArea.Unselect();
            }
            CurrentlySelectedArea = null;
        }

        public void SelectUnit(Units.Unit unit)
        {
            UnselectCurrentlySelectedArea();
            unit.SetSelected(true);
        }

        public void UnselectAllUnits()
        {
            for (int i=0; i< SelectedUnits.Count;i++)
            {
                SelectedUnits[i].SetSelected(false);
                i--;
            }
        }
    }
}
