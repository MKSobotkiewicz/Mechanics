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
            Debug.Log(".TotalCostPerDay:");
            ResourceGenerator.TotalCostPerDay.Log();
            Debug.Log("TotalProductionPerDay:");
            ResourceGenerator.TotalProductionPerDay.Log();
            Debug.Log("TotalNetCosts:");
            TotalNetCosts.Log();
            if (TotalNetCosts.Count > 0)
            {
                Debug.Log("Costs are not supplied.");
            }
            else
            {
                Debug.Log("Costs are supplied.");
                Resource mostValuableResource;
                foreach (var resource in ResourceGenerator.TotalProductionPerDay)
                {

                }
            }
            foreach (var resourceGenerator in ResourceGenerator.AllResurceGenerators)
            {

            }
            return null;
        }
    }
}
