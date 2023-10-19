using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Map
{
    public class Forest : MonoBehaviour,Time.IDaily
    {
        public UnityEngine.Material SummerMaterial;
        public UnityEngine.Material WinterMaterial;
        
        private Vector3 position;
        private Globe.SnowMovement snowMovement;
        private bool summer = true;

        private static readonly float scale = 6371;

        public static Forest Create(Forest prefab, Area area,Transform parent, Globe.SnowMovement _snowMovement,Time.Time time)
        {
            var forest = Instantiate(prefab);
            forest.transform.position = area.Position;
            forest.transform.LookAt(new Vector3());
            forest.position = forest.transform.position / scale;
            forest.snowMovement = _snowMovement;
            forest.transform.SetParent(parent);
            forest.InitialCheck();
            time.AddDaily(forest);
            return forest;
        }

        public uint Priority()
        {
            return 21;
        }

        public void DailyUpdate()
        {
            var map = GetMap();
            if (summer)
            {
                if (map < 0.5f)
                {
                    summer = false;
                    SwitchToWinter();
                }
            }
            else
            {
                if (map >= 0.5f)
                {
                    summer = true;
                    SwitchToSummer();
                }
            }
        }

        private void InitialCheck()
        {
            var map = GetMap();
            if (map < 0.5f)
            {
                summer = false;
                SwitchToWinter();
            }
            else
            {
                summer = true;
                SwitchToSummer();
            }
        }

        private float GetMap()
        {
            var polarIceCaps =  Mathf.Clamp01(Mathf.Pow(Mathf.Cos(position.y) + 0.42f, 100));
            var winter = Mathf.Cos(position.y + snowMovement.Value) - 0.1f;
            var height =  (1 - position.magnitude) * 50f;
            var map = Mathf.Clamp01((polarIceCaps + winter + height - 1) * 10f);
            return map;
        } 

        private void SwitchToSummer()
        {
            //particleSystemRenderer.sharedMaterial = SummerMaterial;
        }

        private void SwitchToWinter()
        {
            //particleSystemRenderer.sharedMaterial = WinterMaterial;
        }
    }
}
