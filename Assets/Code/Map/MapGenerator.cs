using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Map
{
    public class MapGenerator : MonoBehaviour
    {
        public GameObject AreaPrefab;
        public Player.Player Player;
        public Canvas MainCanvas;
        public UnityEngine.UI.RawImage ScreenOverlay;
        public Resources.ResourceGeneratorType RiverGeneratorTypePrefab;
        public Forest ForestPrefab;
        public Time.Time Time;
        public CitiesGenerator CitiesGenerator;
        public AlienCitiesGenerator AlienCitiesGenerator;
        public ResourcesSpreader ResourcesSpreader;
        public List<River> Rivers { get; private set; } = new List<River>();
        public MapData MapData { get; private set; } = new MapData();
        public MeshFilter GlobeMeshFilter;
        public MeshFilter WaterMeshFilter;
        public Globe.SnowMovement SnowMovement;
        public UnityEngine.Material RiverMaterial;
        public UnityEngine.Material RoadMaterial;
        public ComputeShader DistanceShader;
        public ComputeShader NeighbourShader;
        public List<GameObject> Mountains;
        public List<GameObject> Hills;
        public List<UnityEngine.Material> ForestMaterials;
        public AstarPath AstarPath;

        protected Globe.PlanetConditions PlanetConditions;
        protected Scene loadingScreen;
        protected UI.TextConsole textConsole = null;
        protected List<City> cities;

        protected static readonly System.Random random = new System.Random();

        private int index = 0;

        public void Start()
        {
            SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Additive);
            loadingScreen = SceneManager.GetSceneByName("LoadingScreen");
            ScreenOverlay.enabled = false;
        }

        public void Update()
        {
            if (!loadingScreen.isLoaded)
            {
                return;
            }
            SceneManager.SetActiveScene(loadingScreen);
            textConsole = null;
            var rgos = loadingScreen.GetRootGameObjects();
            foreach (var rgo in rgos)
            {
                var go = rgo.GetComponentInChildren<UI.TextConsole>();
                if (go != null)
                {
                    textConsole = go;
                    break;
                }
            }
            if (textConsole == null)
            {
                Debug.LogError("LoadingScreen missing LoadingText");
            }
            index++;
            switch (index)
            {
                case 1:
                    GenerationSteps.InitialStep(this);
                    break;
                case 2:
                    GenerationSteps.CreateClimateStep(this);
                    break;
                case 3:
                    GenerationSteps.CreateAreasStep(this);
                    break;
                case 4:
                    GenerationSteps.SettingAreasNeighboursStep(this);
                    break;
                case 5:
                    GenerationSteps.SettingAreasTypesStep(this);
                    break;
                case 6:
                    GenerationSteps.InitializingAreasStep(this,Player);
                    break;
                case 7:
                    GenerationSteps.OptimizingAreasMeshesStep(this);
                    break;
                case 8:
                    GenerationSteps.GeneratingAreasConnectionsStep(this);
                    break;
                case 9:
                    GenerationSteps.GeneratingRiversStep(this);
                    break;
                case 10:
                    GenerationSteps.GroupingAreasStep(this);
                    break;
                case 11:
                    GenerationSteps.AddingForestsStep(this);
                    break;
                case 12:
                    GenerationSteps.SpreadingResourcesStep(this);
                    break;
                case 13:
                    GenerationSteps.GeneratingCitiesStep(this);
                    break;
                case 14:
                    GenerationSteps.GeneratingRoadsStep(this);
                    break;
                case 15:
                    //GenerationSteps.OptimizingGlobeMeshStep(this);
                    break;
                case 16:
                    GenerationSteps.FinalizeStep(this);
                    break;
                default:
                    Debug.LogError(name+" wrong index");
                    break;
            }
        }

        protected void CreateAreas()
        {
            var mesh = GetComponent<MeshFilter>().mesh;

            int i = 0;
            var vertices = new HashSet<Vector3>(mesh.vertices);
            foreach (var vertice in vertices)
            {
                var go = Instantiate(AreaPrefab);
                go.name = "Area " + i.ToString();
                var area = go.GetComponentInChildren<Area>();
                area.SetTime(Time);
                area.transform.parent = transform;
                area.transform.position = vertice * transform.localScale.x;
                area.SetGlobeMesh(GlobeMeshFilter.mesh);
                area.SetWaterMesh(WaterMeshFilter.mesh);
                MapData.Areas.Add(area);
                i++;
            }
        }
        
        protected void SetAreasNeighbours()
        {
            Debug.Log("  setting positions, time: " + UnityEngine.Time.realtimeSinceStartup);
            var positions = new Vector3[MapData.Areas.Count];
            var ids = new float[MapData.Areas.Count * 6];
            var areas = MapData.Areas;
            for (int i = 0; i < areas.Count; i++)
            {
                positions[i] = areas[i].transform.position;
            }
            var areasBuffer = new ComputeBuffer(positions.Length, sizeof(float) * 3);
            var neighbourIdsBuffer = new ComputeBuffer(ids.Length, sizeof(float));
            areasBuffer.SetData(positions);
            var dataSize = positions.Length / 64 + 1;
            NeighbourShader.SetBuffer(0, "Positions", areasBuffer);
            NeighbourShader.SetBuffer(0, "NeighbourIds", neighbourIdsBuffer);
            Debug.Log("  dispatching shader, time: " + UnityEngine.Time.realtimeSinceStartup);
            NeighbourShader.Dispatch(0, dataSize, 1, 1);
            neighbourIdsBuffer.GetData(ids);
            areasBuffer.Release();
            neighbourIdsBuffer.Release();

            Debug.Log("  setting neighbours, time: " + UnityEngine.Time.realtimeSinceStartup);
            for (int i = 0; i < areas.Count; i++)
            {
                var neighbours = new List<Area>
                {
                    areas[(int)ids[i * 6]],
                    areas[(int)ids[i * 6 + 1]],
                    areas[(int)ids[i * 6 + 2]],
                    areas[(int)ids[i * 6 + 3]],
                    areas[(int)ids[i * 6 + 4]],
                    areas[(int)ids[i * 6 + 5]],
                };
                areas[i].SetNeighbours(neighbours);
            }
        }

        protected void SetAreasTypes()
        {
            int idsPerArea = 20;
            var areas = MapData.Areas;
            MapData.meshFilter = GlobeMeshFilter;
            Debug.Log("  setting positions, time: " + UnityEngine.Time.realtimeSinceStartup);
            var positions = new Vector3[areas.Count];
            for (int i = 0; i < areas.Count; i++)
            {
                positions[i] = areas[i].transform.position;
            }
            var ids = new int[positions.Length* idsPerArea];
            var vertices = GlobeMeshFilter.mesh.vertices;
            vertices = vertices.Concat(WaterMeshFilter.mesh.vertices).ToArray();

            Matrix4x4 localToWorld = GlobeMeshFilter.transform.localToWorldMatrix;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = localToWorld.MultiplyPoint3x4(vertices[i]) * 0.99f;
            }

            var verticesBuffer = new ComputeBuffer(vertices.Length, sizeof(float) * 3);
            var positionBuffer = new ComputeBuffer(positions.Length, sizeof(float) * 3);
            var idBuffer = new ComputeBuffer(ids.Length, sizeof(int));
            verticesBuffer.SetData(vertices);
            positionBuffer.SetData(positions);
            var dataSize = positions.Length / 64 + 1;
            DistanceShader.SetBuffer(0, "Vertices", verticesBuffer);
            DistanceShader.SetBuffer(0, "Positions", positionBuffer);
            DistanceShader.SetBuffer(0, "VerticeIds", idBuffer);
            Debug.Log("  dispatching shader, time: " + UnityEngine.Time.realtimeSinceStartup);
            DistanceShader.Dispatch(0, dataSize, 1, 1);
            idBuffer.GetData(ids);
            verticesBuffer.Release();
            positionBuffer.Release();
            idBuffer.Release();
            Debug.Log("  setting colors, time: " + UnityEngine.Time.realtimeSinceStartup);
            var colors = GlobeMeshFilter.mesh.colors;
            for (int i = 0; i < areas.Count; i++)
            {
                if (ids.Length < i * idsPerArea)
                {
                    areas[i].SetType(new Color(0, 0, 1, 1), this);
                }
                else if (colors.Length < ids[i * idsPerArea])
                {
                    areas[i].SetType(new Color(0, 0, 1, 1), this);
                }
                else
                {
                    try
                    {
                        areas[i].SetType(colors[ids[i * idsPerArea]], this);
                    }
                    catch
                    {
                        areas[i].SetType(new Color(0, 0, 1, 1), this);
                    }
                }
                var verticesIds=new int[idsPerArea];
                Array.Copy(ids, i * idsPerArea, verticesIds,0, idsPerArea);
                areas[i].SetGlobeVertices(verticesIds);
            }
            Debug.Log("  instantiating meshes, time: " + UnityEngine.Time.realtimeSinceStartup);
            vertices = GlobeMeshFilter.mesh.vertices;
            vertices = vertices.Concat(WaterMeshFilter.mesh.vertices).ToArray();
            for (int i = 0; i < areas.Count; i++)
            {
                var position = localToWorld.MultiplyPoint3x4(vertices[ids[i * idsPerArea]]);
                areas[i].Position = position;
            }
            for (int i = 0; i < areas.Count; i++)
            {
                switch (areas[i].Type)
                {
                    case Area.EType.Mountains:
                        {
                            var j = random.Next(Mountains.Count);
                            var mountain = Instantiate(Mountains[j]);
                            mountain.transform.position = (areas[i].Position * (areas[i].GetAreaOrNeighboursLowestPosition().magnitude / areas[i].Position.magnitude)) * 0.9985f;
                            mountain.transform.LookAt(new Vector3(0, 0, 0));
                            var mr = mountain.GetComponentInChildren<MeshRenderer>();
                            mr.transform.localEulerAngles += new Vector3(0, 0, (float)random.NextDouble() * 360);
                            mr.material.SetColor(Globe.PlanetConditions.GlobeMaterialPlantColorName, PlanetConditions.PlantColor);
                            mountain.transform.parent = GlobeMeshFilter.transform;
                            mountain.transform.localScale /= 3;
                            SnowMovement.AddMaterial(mr.material);
                            areas[i].SetLandformMesh(mountain.GetComponentInChildren<MeshFilter>().mesh);
                            areas[i].SetLandformVerticesColor(colors[ids[i * idsPerArea]]);
                            areas[i].Position *= 1.001f;
                            break;
                        }
                    case Area.EType.Hills:
                        {
                            var j = random.Next(Mountains.Count);
                            var hill = Instantiate(Hills[j]);
                            hill.transform.position = (areas[i].Position * (areas[i].GetAreaOrNeighboursLowestPosition().magnitude / areas[i].Position.magnitude)) * 0.9985f;
                            hill.transform.LookAt(new Vector3(0, 0, 0));
                            var mr = hill.GetComponentInChildren<MeshRenderer>();
                            mr.transform.localEulerAngles += new Vector3(0, 0, (float)random.NextDouble() * 360);
                            mr.material.SetColor(Globe.PlanetConditions.GlobeMaterialPlantColorName, PlanetConditions.PlantColor);
                            hill.transform.parent = GlobeMeshFilter.transform;
                            hill.transform.localScale /= 2.5f;
                            SnowMovement.AddMaterial(mr.material);
                            areas[i].SetLandformMesh(hill.GetComponentInChildren<MeshFilter>().mesh);
                            areas[i].SetLandformVerticesColor(colors[ids[i * idsPerArea]]);
                            areas[i].Position *= 1.001f;
                            break;
                        }
                }
            }
        }

        protected void OptimizeGlobeMeshFilter()
        {
            var mesh = GlobeMeshFilter.mesh;
            var triangles = mesh.triangles;
            var colors = mesh.colors;
            var trianglesT = new int[triangles.Length];
            var trianglesTLength = 0;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                if (colors[triangles[i]].b == 0 || colors[triangles[i + 1]].b == 0 || colors[triangles[i + 2]].b == 0)
                {
                    trianglesT[trianglesTLength] = triangles[i];
                    trianglesT[trianglesTLength + 1] = triangles[i + 1];
                    trianglesT[trianglesTLength + 2] = triangles[i + 2];
                    trianglesTLength += 3;
                }
            }

            Array.Resize(ref trianglesT, trianglesTLength);

            Debug.Log("triangles: " + trianglesTLength + " from " + triangles.Length + " : " + (float)trianglesTLength * 100 / (float)triangles.Length + "%");

            mesh.SetTriangles(trianglesT, 0);
            mesh.Optimize();
        }

        protected void AddForests()
        {
            var forests = new GameObject("Forests");
            forests.transform.parent = transform;
            foreach (var mr in ForestMaterials)
            {
                mr.SetColor("_Color", PlanetConditions.PlantColor);
                SnowMovement.AddMaterial(mr);
            }
            var possibleAreas = MapData.PossibleAreas();
            foreach (var area in possibleAreas)
            {
                if (area.Humidity > 0.8&& area.City==null)
                {
                    var forest = Forest.Create(ForestPrefab,area, forests.transform, SnowMovement,Time);
                    area.Forest = forest;
                }
            }
        }

        protected void GroupAreas()
        {
            var possibleAreas = MapData.PossibleAreas();
            while (possibleAreas.Count>0)
            {
                var group = new AreaGroup();
                MapData.AreaGroups.Add(group);
                var area = Utility.ListUtilities.GetRandomObject(possibleAreas);
                area.AddToAreaGroup(group);
                possibleAreas.Remove(area);
                AddNeighboursToAreaGroup(group, area, possibleAreas);
            }
            Debug.Log("Area groups count: "+ MapData.AreaGroups.Count);
        }

        protected void AddNeighboursToAreaGroup(AreaGroup group,Area area,List<Area> possibleAreas)
        {
            var neighbours = area.GetNeighboursOfType(Area.EType.Plains);
            neighbours.AddRange(area.GetNeighboursOfType(Area.EType.Hills));
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (group.Contains(neighbours[i]))
                {
                    neighbours.RemoveAt(i);
                    i--;
                }
            }
            if (neighbours.Count == 0)
            {
                return;
            }
            foreach (var neighbour in neighbours)
            {
                neighbour.AddToAreaGroup(group);
                possibleAreas.Remove(neighbour);
            }
            foreach (var neighbour in neighbours)
            {
                AddNeighboursToAreaGroup(group, neighbour, possibleAreas);
            }
        }

        protected void GenerateRivers()
        {
            var max = 400 + random.Next(100);
            var mountainAreas = MapData.MountainAreas;
            for (int i = 0; i < max; i++)
            {
                var start = mountainAreas[random.Next(mountainAreas.Count - 1)];
                if (start.River == null)
                {
                    try
                    {
                        Rivers.Add(new River(start, 500, GlobeMeshFilter.transform, RiverMaterial));
                    }
                    catch
                    {
                    }
                }
            }
            var vertices = new List<int>();
            var color = new Color(0,0.3f,0,0);
            var possibleAreas = MapData.PossibleAreas();
            foreach (var area in possibleAreas)
            {
                if (area.River != null)
                {
                    vertices.AddRange(area.GetGlobeVertices());
                    area.SetLandformVerticesColor(color);
                    area.Humidity = 1;
                    area.AddResourceGenerator(RiverGeneratorTypePrefab,1,true);
                }
            }
            var colors = GlobeMeshFilter.mesh.colors;
            for(int i=0;i< vertices.Count; i++)
            {
                if (colors.Length < vertices[i])
                {
                    continue;
                }
                if (colors[vertices[i]].b != 1)
                {
                    colors[vertices[i]] += color;
                }
            }
            GlobeMeshFilter.mesh.SetColors(colors);

            River.OptimizeAllRivers();
        }

        protected void GenerateRoads(List<City> cities)
        {
            var pairs = new HashSet<Tuple<Area, Area>>();
            var areas = new List<Area>();
            foreach (var city in cities)
            {
                areas.Add(city.Area);
            }
            foreach (var area1 in areas)
            {
                foreach (var area2 in areas)
                {
                    if (area1 == area2)
                    {
                        continue;
                    }
                    if (area1.AreaGroup != area2.AreaGroup)
                    {
                        continue;
                    }
                    if (Vector3.Distance(area1.Position, area2.Position)>2000)
                    {
                        continue;
                    }
                    if (pairs.Contains(new Tuple<Area, Area>(area2, area1)))
                    {
                        //Debug.Log("already contained");
                        continue;
                    }
                    pairs.Add(new Tuple<Area, Area>(area1, area2));
                }
            }

            Debug.Log("pairs "+ pairs.Count);
            foreach (var pair in pairs)
            {
                try
               {
                    var road = new Road(pair.Item1, pair.Item2, transform, RoadMaterial);
                }
                catch (IOException e)
                {
                    if (e.Source != null)
                    {
                        Debug.LogWarning("IOException source: " + e.Source);
                    }
                }
                catch
                {
                }
            }
            Road.OptimizeAllRoads();
            Utility.Pathfinder.LogSpentTime();
        }

        private static class GenerationSteps
        {
            public static void InitialStep(MapGenerator mapGenerator)
            {
                mapGenerator.textConsole.PushBack("creating climate...");
            }

            public static void CreateClimateStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " creating climate, time: " + UnityEngine.Time.realtimeSinceStartup);
                mapGenerator.PlanetConditions = Globe.PlanetConditions.GenerateRandom();
                mapGenerator.textConsole.PushBack("creating areas...");
            }

            public static void CreateAreasStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " creating areas, time: " + UnityEngine.Time.realtimeSinceStartup);
                mapGenerator.CreateAreas();
                mapGenerator.textConsole.PushBack("setting areas neighbours...");
            }

            public static void SettingAreasNeighboursStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " setting areas neighbours, time: " + UnityEngine.Time.realtimeSinceStartup);
                mapGenerator.SetAreasNeighbours();
                mapGenerator.textConsole.PushBack("setting areas types...");
            }

            public static void SettingAreasTypesStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " setting areas types, time: " + UnityEngine.Time.realtimeSinceStartup);
                mapGenerator.SetAreasTypes();
                mapGenerator.textConsole.PushBack("initializing areas...");
            }

            public static void InitializingAreasStep(MapGenerator mapGenerator,Player.Player player)
            {
                Debug.Log(mapGenerator.name + " initializing areas, time: " + UnityEngine.Time.realtimeSinceStartup);
                var areas = mapGenerator.MapData.Areas;
                foreach (var area in areas)
                {
                    area.Initialize(areas, player, mapGenerator.MainCanvas, mapGenerator.AstarPath);
                }
                mapGenerator.textConsole.PushBack("optimizing areas meshes...");
            }

            public static void OptimizingAreasMeshesStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " optimizing areas meshes, time: " + UnityEngine.Time.realtimeSinceStartup);
                Area.OptimizeMeshes(mapGenerator.MapData.WaterAreas, "Water Areas");
                Area.OptimizeMeshes(mapGenerator.MapData.MountainAreas, "Mountain Areas");
                mapGenerator.textConsole.PushBack("Generating areas connections...");
            }

            public static void GeneratingAreasConnectionsStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " generating areas connections, time: " + UnityEngine.Time.realtimeSinceStartup);
                var areas = mapGenerator.MapData.Areas;
                foreach (var area in areas)
                {
                    area.AddConnectionsToNeighbours();
                }
                (mapGenerator.AstarPath.graphs[0] as Pathfinding.PointGraph).RebuildNodeLookup();
                mapGenerator.textConsole.PushBack("generating rivers...");
            }

            public static void GeneratingRiversStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " generating rivers, time: " + UnityEngine.Time.realtimeSinceStartup);
                mapGenerator.GenerateRivers();
                mapGenerator.textConsole.PushBack("grouping areas...");
            }

            public static void GroupingAreasStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " grouping areas, time: " + UnityEngine.Time.realtimeSinceStartup);
                mapGenerator.GroupAreas();
                mapGenerator.textConsole.PushBack("adding forests...");
            }


            public static void AddingForestsStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " adding forests, time: " + UnityEngine.Time.realtimeSinceStartup);
                mapGenerator.AddForests();
                mapGenerator.textConsole.PushBack("spreading resources...");
            }

            public static void SpreadingResourcesStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " spreading resources, time: " + UnityEngine.Time.realtimeSinceStartup);
                mapGenerator.ResourcesSpreader.SpreadResources(mapGenerator.MapData.PossibleAreas());
                mapGenerator.textConsole.PushBack("generating cities...");
            }

            public static void GeneratingCitiesStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " generating cities, time: " + UnityEngine.Time.realtimeSinceStartup);
                mapGenerator.CitiesGenerator.Init(mapGenerator.GlobeMeshFilter, mapGenerator.MapData, mapGenerator.MainCanvas);
                mapGenerator.cities = mapGenerator.CitiesGenerator.Generate(mapGenerator.MapData.PlainsAreas);
                mapGenerator.textConsole.PushBack("generating roads...");
            }

            public static void GeneratingRoadsStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " generating roads, time: " + UnityEngine.Time.realtimeSinceStartup);
                mapGenerator.GenerateRoads(mapGenerator.cities);
                mapGenerator.textConsole.PushBack("optimizing globe mesh...");
            }

            public static void OptimizingGlobeMeshStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " optimizing globe mesh, time: " + UnityEngine.Time.realtimeSinceStartup);
                mapGenerator.OptimizeGlobeMeshFilter();
                mapGenerator.textConsole.PushBack("finalizing...");
            }

            public static void FinalizeStep(MapGenerator mapGenerator)
            {
                Debug.Log(mapGenerator.name + " done, time: " + UnityEngine.Time.realtimeSinceStartup);
                mapGenerator.GetComponent<MeshRenderer>().enabled = true;
                mapGenerator.ScreenOverlay.enabled = true;
                UnityEngine.Camera camera=null;
                foreach (var go in mapGenerator.loadingScreen.GetRootGameObjects())
                {
                    camera = go.GetComponentInChildren<UnityEngine.Camera>();
                    if (camera != null)
                    {
                        break;
                    }
                }
                mapGenerator.ScreenOverlay.texture = Utility.Camera.RenderToTexture(camera);
                SceneManager.UnloadSceneAsync(mapGenerator.loadingScreen);
                LeanTween.alphaCanvas(mapGenerator.ScreenOverlay.GetComponent<CanvasGroup>(),0,3).setDestroyOnComplete(true);
                Destroy(mapGenerator.GetComponent<MeshFilter>());
                Destroy(mapGenerator.GetComponent<MeshRenderer>());
                Destroy(mapGenerator);
            }
        }
    }
}