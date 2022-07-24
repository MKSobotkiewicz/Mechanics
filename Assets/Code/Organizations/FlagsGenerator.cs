using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Organizations
{
    public class FlagsGenerator : MonoBehaviour
    {
        public List<Color> Golds;
        public List<Color> Reds;
        public List<Color> Blues;

        public List<Sprite> Bases;
        public List<Sprite> Overlays;

        private static readonly System.Random random = new System.Random();
        
        public Texture2D Generate()
        {
            var colors = new Color[3];
            var lottery = new List<int>{ 0, 1, 2, 3, 4};
            for (int i=0;i<3;i++)
            {
                var ticket = Utility.ListUtilities.GetRandomObject(lottery);
                Debug.Log(ticket);
                lottery.Remove(ticket);
                switch (ticket)
                {
                    case 0:
                        colors[i]= Utility.ListUtilities.GetRandomObject(Golds);
                        break;
                    case 1:
                        colors[i] = Utility.ListUtilities.GetRandomObject(Reds);
                        break;
                    case 2:
                        colors[i] = Utility.ListUtilities.GetRandomObject(Blues);
                        break;
                    case 3:
                        colors[i] = Color.black;
                        break;
                    case 4:
                        colors[i] = Color.white;
                        break;
                }
            }
            Texture2D flag = new Texture2D(64,64);
            var baseMap = Utility.ListUtilities.GetRandomObject(Bases);
            for (var i = 0; i < flag.width; i++)
            {
                for (var j = 0; j < flag.height; j++)
                {
                    var pixel = baseMap.texture.GetPixel((int)baseMap.rect.x + i, (int)baseMap.rect.y + j);
                    if (pixel.r > 0.5f)
                    {
                        flag.SetPixel(i, j, colors[0]);
                    }
                    else if (pixel.g > 0.5f)
                    {
                        flag.SetPixel(i, j, colors[1]);
                    }
                    else
                    {
                        flag.SetPixel(i, j, colors[2]);
                    }
                }
            }
            if (random.Next(10) > 2)
            {
                var overlayMap = Utility.ListUtilities.GetRandomObject(Overlays);
                for (var i = 0; i < flag.width; i++)
                {
                    for (var j = 0; j < flag.height; j++)
                    {
                        var pixel = overlayMap.texture.GetPixel((int)overlayMap.rect.x + i, (int)overlayMap.rect.y + j);
                        if (pixel.r > 0.5f)
                        {
                            flag.SetPixel(i, j, colors[2]);
                        }
                    }
                }
                if (random.Next(10) > 6)
                {
                    var colId = random.Next(2);
                    var overlayMap2 = Utility.ListUtilities.GetRandomObject(Overlays);
                    for (var i = 0; i < flag.width; i++)
                    {
                        for (var j = 0; j < flag.height; j++)
                        {
                            var pixel = overlayMap2.texture.GetPixel((int)overlayMap2.rect.x + i, (int)overlayMap2.rect.y + j);
                            if (pixel.r > 0.5f)
                            {
                                flag.SetPixel(i, j, colors[colId]);
                            }
                        }
                    }
                }
            }
            if (random.Next(10) > 7)
            {
                var extrude = new int[flag.width, flag.height];
                for (var i = 0; i < flag.width ; i++)
                {
                    extrude[i, 0] = 1;
                    extrude[i, flag.height-1] = 1;
                }
                for (var j = 0; j < flag.height; j++)
                {
                    extrude[0, j] = 1;
                    extrude[flag.width-1, j] = 1;
                }
                for (var i = 1; i < flag.width - 1; i++)
                {
                    for (var j = 1; j < flag.height-1; j++)
                    {
                        if (flag.GetPixel(i, j) != flag.GetPixel(i - 1, j) ||
                            flag.GetPixel(i, j) != flag.GetPixel(i + 1, j) ||
                            flag.GetPixel(i, j) != flag.GetPixel(i, j - 1) ||
                            flag.GetPixel(i, j) != flag.GetPixel(i, j + 1) ||
                            flag.GetPixel(i, j) != flag.GetPixel(i - 1, j - 1) ||
                            flag.GetPixel(i, j) != flag.GetPixel(i - 1, j + 1) ||
                            flag.GetPixel(i, j) != flag.GetPixel(i + 1, j - 1) ||
                            flag.GetPixel(i, j) != flag.GetPixel(i + 1, j + 1))
                        {
                            extrude[i, j] = 1;
                        }
                    }
                }
                var colId = random.Next(3);
                for (var i = 1; i < flag.width - 1; i++)
                {
                    for (var j = 1; j < flag.height - 1; j++)
                    {
                        if (extrude[i, j] == 1)
                        {
                            flag.SetPixel(i, j, colors[colId]);
                        }
                    }
                }
            }
            flag.Apply();
            return flag;
        }
    }
}
