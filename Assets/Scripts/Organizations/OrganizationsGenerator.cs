using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Project.Resources;

namespace Project.Organizations
{
    public class OrganizationsGenerator
    {
        private readonly Map.MapData mapData;

        public OrganizationsGenerator(Map.MapData _mapData)
        {
            mapData = _mapData; 
        }

        public Organization Create()
        {
            var TotalNetCosts = ResourceGenerator.TotalCostPerDay - ResourceGenerator.TotalProductionPerDay;
            Debug.Log("ResourceGenerator.TotalCostPerDay "+ ResourceGenerator.TotalCostPerDay);
            Debug.Log("ResourceGenerator.TotalProductionPerDay " + ResourceGenerator.TotalProductionPerDay);
            if (TotalNetCosts.Count > 0)
            {
                Debug.Log("");
            }
            foreach (var resourceGenerator in ResourceGenerator.AllResurceGenerators)
            {

            }
            return null;
        }
    }
}
