using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Project.Map;

namespace Project.Utility
{
    public class RandomMesh:MonoBehaviour
    {
        public List<Mesh> Meshes;
        public List<UnityEngine.Material> BillboardMaterials;
        public MeshRenderer Billboard;

        private static readonly System.Random random = new System.Random();

        public void Awake()
        {
            var mr = GetComponent<MeshFilter>();
            var randomValue = random.Next(Meshes.Count - 1);
            mr.mesh= Meshes[randomValue];
            Billboard.material = BillboardMaterials[randomValue];
        }
    }
}
