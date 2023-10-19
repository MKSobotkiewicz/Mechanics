using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class CityResourcesList : MonoBehaviour
    {
        public Canvas ProductionList;
        public Canvas DeficitList;
        public Canvas StockpileList;

        public Resource ResourcePrefab;
        private List<Resource> productions = new List<Resource>();
        private List<Resource> deficits = new List<Resource>();
        private List<Resource> stockpiles = new List<Resource>();

        private Map.Area area;

        public void InitOrUpdate(Map.Area _area)
        {
            area = _area;
            foreach (var production in productions)
            {
                Destroy(production.gameObject);
            }
            productions.Clear();
            foreach (var deficit in deficits)
            {
                Destroy(deficit.gameObject);
            }
            deficits.Clear();
            foreach (var stockpile in stockpiles)
            {
                Destroy(stockpile.gameObject);
            }
            stockpiles.Clear();
            var netProductions = area.GetNetResourceProduction();
            foreach (var netProduction in netProductions)
            {
                if (!netProduction.Resource.Unextracted)
                {
                    productions.Add(Instantiate(ResourcePrefab, ProductionList.transform).Init(netProduction.Resource, (long)netProduction.Value));
                }
            }
            var netDeficits = area.GetNetResourceDeficit();
            foreach (var netDeficit in netDeficits)
            {
                if (!netDeficit.Resource.Unextracted)
                {
                    deficits.Add(Instantiate(ResourcePrefab, DeficitList.transform).Init(netDeficit.Resource, -(long)netDeficit.Value));
                }
            }
            foreach (var stockpile in area.ResourceDepot.resources)
                {
                if (!stockpile.Resource.Unextracted)
                {
                    stockpiles.Add(Instantiate(ResourcePrefab, StockpileList.transform).Init(stockpile.Resource, (long)stockpile.Value));
                }
            }
        }
    }
}
