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

        private static readonly System.Random random = new System.Random();

        public void Start()
        {
            var mr = GetComponent<MeshFilter>();
            mr.mesh= Meshes[random.Next(Meshes.Count - 1)];
        }
    }
}
