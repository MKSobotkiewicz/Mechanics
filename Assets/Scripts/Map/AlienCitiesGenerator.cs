using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Map
{
    public class AlienCitiesGenerator : MonoBehaviour
    {
        public GameObject AlienCityPrefab;

        private static readonly System.Random random = new System.Random();

        public void Start()
        {
        }

        public List<GameObject> Generate(List<Area> possibleAreas,int count)
        {
            if (possibleAreas.Count < count)
            {
                Debug.LogWarning(name+ " possibleAreas smaller than city count.");
                count = possibleAreas.Count;
            }
            var alienCities = new List<GameObject>();
            while (count>0)
            {
                var area = Utility.ListUtilities.GetRandomObject(possibleAreas);
                alienCities.Add(CreateCity(area));
                possibleAreas.Remove(area);
                count--;
            }
            return alienCities;
        }
        private GameObject CreateCity(Area area)
        {
            var alienCity = Instantiate(AlienCityPrefab,area.transform);
            alienCity.transform.position = area.Position;
            alienCity.transform.LookAt(new Vector3());
            return alienCity;
        }
    }
}