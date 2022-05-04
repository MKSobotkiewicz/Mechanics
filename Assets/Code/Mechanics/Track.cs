using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Mechanics
{
    public class Track : MonoBehaviour,ISide
    {

        public UnityEngine.Material Material { get;private set; }
        public float Speed { get; private set; } = 0;
        public SideE Side = SideE.Right;

        private float offset=0;

        public void Start()
        {
            Material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        }

        public void Update()
        {
            offset = (offset+Speed)%1;
            Material.SetTextureOffset("_BaseColorMap", new Vector2(0, offset));
        }

        public SideE GetSide()
        {
            return Side;
        }

        public void SetSpeed(float speed)
        {
            switch (Side)
            {
                case SideE.Left:
                    Speed = -speed;
                    return;
                case SideE.Right:
                    Speed = speed;
                    return;
            }
        }
        
    }
}
