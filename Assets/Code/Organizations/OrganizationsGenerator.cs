using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Project.Resources;

namespace Project.Organizations
{
    public class OrganizationsGenerator:MonoBehaviour
    {
        public FlagsGenerator FlagsGenerator;

        private readonly Map.MapData mapData;

        public OrganizationsGenerator(Map.MapData _mapData)
        {
            mapData = _mapData; 
        }

        public void Generate()
        {
            foreach (var city in Map.City.AllCities)
            {
                var organization=Create(city.name);
                organization.OwnedCities.Add(city);
                city.UpdateFlag(organization.Flag);
            }
        }

        public Organization Create(string name)
        {
            var go = new GameObject(name);
            go.transform.parent = transform;
            var organization = go.AddComponent<Organization>();
            organization.Initialize(name, FlagsGenerator.Generate());
            /*var TotalNetCosts = ResourceGenerator.TotalCostPerDay - ResourceGenerator.TotalProductionPerDay;
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
                //Resource mostValuableResource;
                foreach (var resource in ResourceGenerator.TotalProductionPerDay)
                {

                }
            }
            foreach (var resourceGenerator in ResourceGenerator.AllResurceGenerators)
            {

            }*/
            return organization;
        }
    }
}
