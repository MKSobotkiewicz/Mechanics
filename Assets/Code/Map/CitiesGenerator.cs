using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Map
{
    public class CitiesGenerator : MonoBehaviour
    {
        public City CityPrefab;
        public TextAsset CityNamesFile;
        public Organizations.OrganizationsGenerator OrganizationsGenerator;

        public List<BasicResourceGeneratorType> BasicResourceGenerators;

        private List<string> names;
        private MeshFilter meshFilter;
        private MapData mapData;
        private Canvas canvas;
        private static readonly System.Random random = new System.Random();

        public void Start()
        {
        }

        public void Init(MeshFilter _meshFilter, MapData _mapData, Canvas _canvas)
        {
            meshFilter = _meshFilter;
            mapData = _mapData;
            canvas = _canvas;
        }

        public List<City> Generate(List<Area> possibleAreas)
        {
            LoadNames();
            var cities = new List<City>();
            foreach (var area in possibleAreas)
            {
                if (random.NextDouble() * area.Humidity  > 0.98)
                {
                    cities.Add(CreateCity(Utility.ListUtilities.GetRandomObject(names),area));
                    foreach (var basicResourceGenerator in BasicResourceGenerators)
                    {
                        area.AddResourceGenerator(basicResourceGenerator.ResourceGeneratorType, basicResourceGenerator.Count, true);
                    }
                }
            }
            OrganizationsGenerator.Generate();
            return cities;
        }

        private void LoadNames()
        {
            var parser = new Data.XmlParser(CityNamesFile.text);
            names = parser.Parse("name");
        }

        public City CreateCity(string name,Area area)
        {
            var city = Instantiate(CityPrefab);
            area.City = city;
            city.Initialize(name, area, meshFilter, mapData, canvas);
            if (area.Forest != null)
            {
                Destroy(area.Forest.gameObject);
            }
            foreach (var neighbour in area.Neighbours)
            {
                if (neighbour.Forest != null)
                {
                    Destroy(neighbour.Forest.gameObject);
                }
            }
            return city;
        }

        [Serializable]
        public class BasicResourceGeneratorType
        {
            public Resources.ResourceGeneratorType ResourceGeneratorType;
            public uint Count;

            public BasicResourceGeneratorType(Resources.ResourceGeneratorType resourceGeneratorType, uint count)
            {
                ResourceGeneratorType = resourceGeneratorType;
                Count = count;
            }
        }
    }
}
