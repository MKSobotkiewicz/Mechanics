﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public interface IFollowed
    {
        string Name();
        Vector3 FollowedPosition();
    }
}
