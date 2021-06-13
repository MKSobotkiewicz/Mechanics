using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Map
{
    public class CitiesGenerator : MonoBehaviour
    {
        public City CityPrefab;
        public TextAsset CityNamesFile;

        private List<string> names;

        private static readonly System.Random random = new System.Random();

        public void Start()
        {
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
                }
            }
            return cities;
        }

        private void LoadNames()
        {
            var parser = new Data.XmlParser(CityNamesFile.text);
            names = parser.Parse("name");
        }

        private City CreateCity(string name,Area area)
        {
            var city=Instantiate(CityPrefab);
            city.Initialize(name, area);
            return city;
        }
    }
}
