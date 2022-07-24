using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class CityName:Follower
    {
        public Text Name;
        public RawImage Flag;

        public void UpdateName()
        {
            Name.text = followed.Name().ToUpper();
        }

        public void UpdateFlag(Texture2D flag)
        {
            Flag.texture = flag;
        }
    }
}
