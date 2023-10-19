﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Units
{
    public class Weapon:MonoBehaviour
    {
        public Animator Animator;

        public void PlayAttackAnimation()
        {
            Animator.SetTrigger("Attack");
        }
    }
}
