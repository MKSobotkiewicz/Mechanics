using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class SelectPart : MonoBehaviour
    {
        public Canvas PartsListCanvas;
        public Part PartPrefab;
        public Crafting.TankCraftingManager TankCraftingManager;

        private List<Part> parts=new List<Part>();

        void Start()
        {
            var allParts = UnityEngine.Resources.LoadAll<Mechanics.Part>("Parts");
            foreach (var part in allParts)
            {
                if (!part.Hidden)
                {
                    parts.Add(Instantiate(PartPrefab, PartsListCanvas.transform));
                    parts.Last().SetPart(part, TankCraftingManager);
                }
            }
        }
        
        void Update()
        {

        }
    }
}