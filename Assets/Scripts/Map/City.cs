using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Map
{
    public class City:MonoBehaviour,UI.IFollowed
    {
        public UI.CityName CityNamePrefab;
        public Area Area { get; private set; }

        private Vector3 textPosition;
        private UI.CityName cityName;
        private MeshFilter meshFilter;
        private MapData mapData;

        public void Start()
        {
            textPosition = Vector3.RotateTowards(transform.position,Vector3.down,0.01f,0);
        }

        public void Initialize(string name,Area area, MeshFilter _meshFilter, MapData _mapData, Canvas canvas)
        {
            meshFilter = _meshFilter;
            mapData = _mapData;
            Area = area;
            transform.position = Area.Position;
            transform.LookAt(new Vector3());
            transform.parent = Area.transform;
            gameObject.name = name;
            cityName = Instantiate(CityNamePrefab, canvas.transform);
            cityName.UpdateFollowed(this);
            cityName.UpdateName();
            SetVertexColors(1);
        }

        public Vector3 FollowedPosition()
        {
            return textPosition;
        }

        public string Name()
        {
            return name;
        }

        private void SetVertexColors(int size)
        {
            var vertices = new List<int>();
            var color = new Color(1, 1, 1, 0);
            var possibleAreas = mapData.PossibleAreas();
            var colors = meshFilter.mesh.colors;
            vertices.AddRange(Area.GetGlobeVertices());
            Area.SetLandformVerticesColor(color);
            for (int i = 0;i<size;i++)
            {
                if (colors[vertices[i]].b != 1)
                {
                    colors[vertices[i]] = color;
                }
            }
            meshFilter.mesh.SetColors(colors);
        }
    }
}
