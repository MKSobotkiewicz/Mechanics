using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Map
{
    public class Area : MonoBehaviour,Utility.IClicable
    {
        public List<Area> Neighbours { get; private set; } = new List<Area>();
        public EType Type { get; private set; }
        public Color WaterTilesColor;
        public Color MountainTilesColor;
        public Color PlainsTilesColor;
        public Vector3 Position;
        public River River;
        public float Humidity;
        public Resources.ResourceDepot ResourceDepot { get; private set; }
        public List<Resources.ResourceGenerator> ResourceGenerators { get; private set; } = new List<Resources.ResourceGenerator>();
        public bool Road = false;
        public Forest Forest;
        public City City;
        public AreaGroup AreaGroup { get; private set; }
        public List<Units.Unit> Units = new List<Units.Unit>();
        public MapPointNode MapPointNode;

        private UnityEngine.Material material;
        private UI.CityView cityView;
        private UI.Area areaUi;
        private int[] globeVertices;
        private Mesh globeMesh;
        private Mesh waterMesh;
        private Mesh landformMesh;
        private Time.Time time;
        private UI.ResourceIconGroup ResourceIconGroup;
        private Player.Player player;
        private AstarPath path;
        private readonly Canvas canvas;

        private static Area clicked;
        private static Area unclicked;

        private static readonly List<Area> allAreas = new List<Area>();
        private static readonly string selected = "_Selected";
        private static readonly string accesibility = "_IsAccesible";
        private static readonly string color = "_Color";


        private static readonly System.Random random = new System.Random();

        public void Awake()
        {
            material = GetComponentInChildren<MeshRenderer>().material;
            ResourceIconGroup = GetComponentInChildren<UI.ResourceIconGroup>();
        }

        public void SetTime(Time.Time _time)
        {
            time = _time;
            ResourceDepot = Resources.ResourceDepot.Create(time);
            ResourceDepot.transform.parent = transform;
            ResourceDepot.transform.localPosition = new Vector3();
        }

        public void Initialize(List<Area> areas, Player.Player _player, Canvas canvas, AstarPath _path)
        {
            path = _path;
            //MapPointNode = new MapPointNode(path, this);
            AstarPath.active.AddWorkItem(new Pathfinding.AstarWorkItem(ctx => {
                var graph = AstarPath.active.data.pointGraph;
                MapPointNode = graph.AddNode<MapPointNode>(new MapPointNode(AstarPath.active, this),(Pathfinding.Int3)transform.position);
                MapPointNode.Penalty = Weight();
            }));
            player = _player;
            allAreas.Add(this);
            if (Neighbours.Count == 0)
            {
                Debug.LogWarning(name+" is missing neighbours");
                GetNeighbours(areas);
            }
            cityView = canvas.GetComponentInChildren<UI.CityView>();
            if (cityView == null)
            {
                Debug.LogError("Missing CityView");
            }
            areaUi = canvas.GetComponentInChildren<UI.Area>();
            if (areaUi == null)
            {
                Debug.LogError("Missing Area UI");
            }
            SetMesh();
        }

        public void AddConnectionsToNeighbours()
        {
            foreach (var neighbour in Neighbours)
            {
                AstarPath.active.AddWorkItem(new Pathfinding.AstarWorkItem(ctx => {
                    MapPointNode.AddConnection(neighbour.MapPointNode, neighbour.Weight());
                }));
            }
        }

        public Resources.ResourceValueList GetTotalResourceProduction()
        {
            var resources = new Resources.ResourceValueList();
            foreach (var generator in ResourceGenerators)
            {
                resources+=generator.DailyProduction();
            }
            return resources;
        }

        public Resources.ResourceValueList GetTotalResourceCosts()
        {
            var resources = new Resources.ResourceValueList();
            foreach (var generator in ResourceGenerators)
            {
                resources += generator.DailyCost();
            }
            return resources;
        }

        public Resources.ResourceValueList GetNetResourceProduction()
        {
            return GetTotalResourceProduction()- GetTotalResourceCosts();
        }

        public Resources.ResourceValueList GetNetResourceDeficit()
        {
            return GetTotalResourceCosts() - GetTotalResourceProduction();
        }

        public void Click()
        {
            clicked = this;
        }

        public void Unclick()
        {
            if (player.CurrentlySelectedArea != null)
            {
                if (this != player.CurrentlySelectedArea)
                {
                    player.UnselectCurrentlySelectedArea();
                }
            }
            if (player.SelectedUnits.Count==0)
            {
                unclicked = this;
                CheckForClicked();
            }
        }

        public void Order()
        {
            if (Type == EType.Mountains || Type == EType.Water)
            {
                return;
            }
            var units = player.SelectedUnits;

            if (units.Count == 0)
            {
                return;
            }

            foreach (var unit in units)
            {
                unit.Move(this);
            }
        }

        public void Select()
        {
            material.SetFloat(selected, 1f);
            if (City != null)
            {
                cityView.SetVisible(true);
                cityView.SetArea(this);
            }
            else
            {
                areaUi.SetVisible(true);
                areaUi.SetArea(this);
            }
        }

        public void Unselect()
        {
            material.SetFloat(selected, 0f);
            if (City != null)
            {
                cityView.SetVisible(false);
            }
            else
            {
                areaUi.SetVisible(false);
            }
        }

        public void AddToAreaGroup(AreaGroup areaGroup)
        {
            if (AreaGroup != null)
            {
                AreaGroup.Remove(this);
            }
            AreaGroup = areaGroup;
            AreaGroup.Add(this);
        }

        public Vector3 GetAreaOrNeighboursLowestPosition()
        {
            var areaAndNeighboursPositions = new List<Vector3>();
            areaAndNeighboursPositions.Add(Position);
            foreach (var neighbour in Neighbours)
            {
                areaAndNeighboursPositions.Add(neighbour.Position);
            }
            var distance = 99999f;
            var lowest = new Vector3();
            foreach (var pos in areaAndNeighboursPositions)
            {
                var currentDistance = pos.magnitude;
                if (currentDistance <transform.position.magnitude)
                {
                    return transform.position;
                }
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    lowest = pos;
                }
            }
            return lowest;
        }

        public void SetGlobeVertices(int[] vertices)
        {
            globeVertices = vertices;
        }

        public int[] GetGlobeVertices()
        {
            return globeVertices;
        }

        public void SetGlobeMesh(Mesh mesh)
        {
            globeMesh = mesh;
        }

        public void SetWaterMesh(Mesh mesh)
        {
            waterMesh = mesh;
        }

        public void SetLandformMesh(Mesh mesh)
        {
            landformMesh = mesh;
        }

        public void SetLandformVerticesColor(Color color)
        {
            if (landformMesh != null)
            {
                var count = landformMesh.vertices.Length;
                var landformMeshColor = new Color[count];
                for (int i=0;i< count; i++)
                {
                    landformMeshColor[i]=color;
                }
                landformMesh.SetColors(landformMeshColor);
            }
        }

        public void SetNeighbours(List<Area> neighbours)
        {
            if (neighbours.Count != 6)
            {
                Debug.LogError("Area " +name+ " given wrong number of neighbours.");
            }
            Neighbours = neighbours;
        }

        public uint Weight()
        {
            if (Road)
            {
                return 1;
            }
            if (Type == EType.Plains)
            {
                return 2;
            }
            if (Type == EType.Hills)
            {
                return 4;
            }
            return uint.MaxValue;
        }

        public float Distance(Area area)
        {
            return Vector3.Distance(transform.localPosition, area.transform.localPosition);
        }

        public List<Area> GetNeighboursOfType(EType type)
        {
            var neighboursOfType = new List<Area>();
            foreach(var neighbour in Neighbours)
            {
                if (neighbour.Type== type)
                {
                    neighboursOfType.Add(neighbour);
                }
            }
            return neighboursOfType;
        }

        private void GetNeighbours(List<Area> areas)
        {
            var closest = new Dictionary<Area, float>();
            foreach (var area in areas)
            {
                if (area == this)
                {
                    continue;
                }
                var distance = Distance(area);
                if (distance < 0.1)
                {
                    closest.Add(area, distance);
                }
            }
            var sortedClosest = from entry in closest orderby entry.Value ascending select entry;
            int i = 0;
            foreach(var area in sortedClosest)
            {
                i++;
                if (i > 6)
                {
                    break;
                }
                Neighbours.Add(area.Key);
            }
        }

        public void SetType(EType type, MapGenerator mapGenerator)
        {
            Type = type;
            SetColor();
            AddAreaToMapGeneratorAreaLists(mapGenerator);
        }

        public void SetType(Color vertexColor, MapGenerator mapGenerator)
        {
            if (vertexColor.b == 1)
            {
                Type = EType.Water;
            }
            else if (vertexColor.r==1)
            {
                Type = EType.Mountains;
                Humidity = 1-vertexColor.a;
            }
            else if(vertexColor.r == 0.5f)
            {
                Type = EType.Hills;
                Humidity = 1 - Mathf.Clamp(vertexColor.a*100,0,1);
            }
            else
            {
                Type = EType.Plains;
                Humidity = 1 - Mathf.Clamp(vertexColor.a * 100, 0, 1);
            }
            SetColor();
            AddAreaToMapGeneratorAreaLists(mapGenerator);
        }

        public void AddResourceGenerator(Resources.ResourceGeneratorType resourceGeneratorType,uint size,bool instant)
        {
            var rg = Resources.ResourceGenerator.Create(ResourceDepot, resourceGeneratorType, size, time, instant);
            rg.transform.parent = transform;
            rg.transform.localPosition = new Vector3();
            ResourceGenerators.Add(rg);
            foreach (var res in rg.ResourceGeneratorType.ProductionPerDay)
            {
                ResourceIconGroup.AddResourceIcon(res.Resource);
            }
        }

        private void AddAreaToMapGeneratorAreaLists(MapGenerator mapGenerator)
        {
            switch (Type)
            {
                case EType.Water:
                    mapGenerator.MapData.WaterAreas.Add(this);
                    break;
                case EType.Mountains:
                    mapGenerator.MapData.MountainAreas.Add(this);
                    break;
                case EType.Hills:
                    mapGenerator.MapData.HillsAreas.Add(this);
                    break;
                case EType.Plains:
                    mapGenerator.MapData.PlainsAreas.Add(this);
                    break;
            }
        }

        private void SetMesh()
        {
            var worldToLocal = transform.worldToLocalMatrix;
            var doneNeighbous = new List<Area>();
            var points = new List< Vector3>();
            var current = Neighbours[0];
            for (int j=0;j<6;j++)
            {
                for (int i = 0; i < current.Neighbours.Count; i++)
                {
                    if (Neighbours.Contains(current.Neighbours[i]) && (!doneNeighbous.Contains(current.Neighbours[i])||(current.Neighbours[i] == Neighbours[0]&&j>3)))
                    {
                        var point = (Position + current.Position + current.Neighbours[i].Position) / 3;
                        points.Add(worldToLocal.MultiplyPoint3x4(point));

                        doneNeighbous.Add(current);
                        current = current.Neighbours[i];
                        break;
                    }
                }
                if (points.Count > 5)
                {
                    break;
                }
            }
            var mesh = GetComponentInChildren<MeshFilter>().mesh;
            if (points.Count > 5)
            {
                if (new Math.Matrix3x3(points[0]- transform.position, points[2]- transform.position, points[4]- transform.position).Determinant() < 0)
                {
                    mesh.triangles = new int[] { 0, 1, 2, 5, 0, 2, 5, 2, 3, 3, 4, 5 };
                }
                else
                {
                    mesh.triangles = new int[] { 2, 1, 0, 2, 0, 5, 3, 2, 5, 5, 4, 3 };
                }
               /* mesh.triangles = new int[] { 0, 1, 2, 5, 0, 2, 5, 2, 3, 3, 4, 5,
                                             2, 1, 0, 2, 0, 5, 3, 2, 5, 5, 4, 3};*/
                mesh.SetVertices(points);
                mesh.SetUVs(0, new List<Vector2> {new Vector2(0.067f,0.25f),
                                                 new Vector2(0.067f, 0.75f),
                                                 new Vector2(0.5f, 1f),
                                                 new Vector2(0.933f,0.75f),
                                                 new Vector2(0.933f,0.25f),
                                                 new Vector2(0.5f, 0f)});
                mesh.RecalculateNormals();
                mesh.RecalculateTangents();
                mesh.RecalculateBounds();
                mesh.Optimize();
            }
            else if (points.Count > 4)
            {
                if (new Math.Matrix3x3(points[0] - transform.position, points[2] - transform.position, points[4] - transform.position).Determinant() < 0)
                {
                    mesh.triangles = new int[] { 0, 1, 2, 5, 0, 2, 5, 2, 3};
                }
                else
                {
                    mesh.triangles = new int[] { 2, 1, 0, 2, 0, 4, 3, 2, 4};
                }
                mesh.SetVertices(points);
                mesh.RecalculateNormals();
                mesh.RecalculateTangents();
                mesh.RecalculateBounds();
                mesh.Optimize();
            }
            else
            {
                Debug.LogWarning("Area " + name + " failed to create mesh.");
                var temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                temp.transform.position = transform.position;
                temp.transform.parent = transform;
                temp.transform.localScale *= 100;
            }
            var collider = GetComponentInChildren<MeshCollider>();
            collider.sharedMesh = mesh;
        }

        private void SetColor()
        {
            switch (Type)
            {
                case EType.Water:
                    material.SetColor(color, WaterTilesColor);
                    material.SetFloat(accesibility, 1);
                    return;
                case EType.Mountains:
                    material.SetColor(color, MountainTilesColor);
                    material.SetFloat(accesibility, 1);
                    return;
                case EType.Hills:
                case EType.Plains:
                    material.SetColor(color, PlainsTilesColor);
                    material.SetFloat(accesibility, 0);
                    return;
            }
        }

        public static bool CheckForClicked()
        {
            if (clicked != null&& unclicked != null&& clicked== unclicked)
            {
                clicked.player.SelectArea(clicked);
                clicked = null;
                unclicked = null;
                return true;
            }
            clicked = null;
            unclicked = null;
            return false;
        }

        public static void OptimizeMeshes(List<Area> areas,string name)
        {
            var masterMesh = new GameObject(name);
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(masterMesh, areas[0].gameObject.scene);
            masterMesh.transform.position = new Vector3();
            masterMesh.layer = LayerMask.NameToLayer("Area");
            var mf = masterMesh.AddComponent<MeshFilter>();
            var mr = masterMesh.AddComponent<MeshRenderer>();
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mr.material = areas[0].GetComponentInChildren<MeshRenderer>().material;
            mr.material.renderQueue += 100;
            var combine = new CombineInstance[areas.Count];
            for (int i = 0; i < areas.Count; i++)
            {
                combine[i].mesh = areas[i].GetComponentInChildren<MeshFilter>().mesh;
                combine[i].transform = areas[i].transform.localToWorldMatrix;
            }
            mf.mesh = new Mesh
            {
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };
            mf.mesh.CombineMeshes(combine);
            mf.mesh.RecalculateBounds();
            mf.mesh.RecalculateTangents();
            mf.mesh.RecalculateNormals();
            mr.allowOcclusionWhenDynamic = false;
            for (int i = 0; i < areas.Count; i++)
            {
                Destroy(areas[i].GetComponentInChildren<MeshFilter>());
                Destroy(areas[i].GetComponentInChildren<MeshRenderer>());
            }
        }

        public enum EType
        {
            Water=0,
            Plains=1,
            Hills = 2,
            Mountains =3
        }
    }
}
