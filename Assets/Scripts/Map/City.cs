using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Map
{
    public class City:MonoBehaviour
    {
        public UI.CityName CityNamePrefab;
        public Area Area { get; private set; }
        public Vector3 TextPosition { get; private set; }

        private UI.CityName cityName;

        public void Start()
        {
            TextPosition = Vector3.RotateTowards(transform.position,Vector3.down,0.01f,0);
        }

        public void Initialize(string name,Area area)
        {
            Area = area;
            transform.position = Area.Position;
            transform.LookAt(new Vector3());
            transform.parent = Area.transform;
            gameObject.name = name;
            var canvas = UnityEngine.Camera.main.GetComponentInChildren<Canvas>();
            cityName = Instantiate(CityNamePrefab, canvas.transform);
            cityName.SetCity(this);
        }

        /*public static City Create(string name, Transform parent)
        {
            var go = new GameObject(name);
            go.transform.parent = parent;
            go.transform.localPosition = new Vector3();
            go.transform.localEulerAngles = new Vector3();
            var city=go.AddComponent<City>();
            return city;
        }*/
    }
}
