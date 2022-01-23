using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Project.Globe
{
    public class Skybox : MonoBehaviour
    {
        public MeshRenderer Top;
        public MeshRenderer Bottom;
        public MeshRenderer Back;
        public MeshRenderer Front;
        public MeshRenderer Left;
        public MeshRenderer Right;
        public List<SkyboxMaterials> SkyboxMaterials;

        private static readonly System.Random random = new System.Random();

        public void Start()
        {
            var index = random.Next(SkyboxMaterials.Count-1);
            Top.material = SkyboxMaterials[index].Top;
            Bottom.material = SkyboxMaterials[index].Bottom;
            Back.material = SkyboxMaterials[index].Back;
            Front.material = SkyboxMaterials[index].Front;
            Left.material = SkyboxMaterials[index].Left;
            Right.material = SkyboxMaterials[index].Right;
        }
    }
}
