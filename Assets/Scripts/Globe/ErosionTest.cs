using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Globe
{
    public class ErosionTest : MonoBehaviour
    {
        public ComputeShader HeightmapGenerator;
        public ComputeShader ErosionGenerator;
        public RenderTexture Heightmap;

        public void Start()
        {
            Heightmap = new RenderTexture(4096, 2096, 24);
            Heightmap.enableRandomWrite = true;
            Heightmap.Create();

            HeightmapGenerator.SetTexture(0, "Result", Heightmap);
            HeightmapGenerator.SetFloat("Resolution", 4096);
            HeightmapGenerator.SetFloat("Scale", 0.1f);
            HeightmapGenerator.SetFloat("Strength", 1f);
            HeightmapGenerator.SetFloat("X", 0);
            HeightmapGenerator.Dispatch(0,Heightmap.width/8, Heightmap.height/8,1);

            ErosionGenerator.SetTexture(0, "Heightmap", Heightmap);
            ErosionGenerator.Dispatch(0, Heightmap.width/8, Heightmap.height/8, 1);
        }

        public void Update()
        {
        }
    }
}
