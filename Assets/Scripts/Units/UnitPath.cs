﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Project.Units
{
    public class UnitPath:MonoBehaviour
    {
        public UnityEngine.Material Material;
        public float Width;

        private Spline.Spline spline;

        public void Create(List<Map.Area> path)
        {
            spline = Spline.Spline.CreateSpline(path, transform, Material, "Unit Path");
        }

        public void Destroy()
        {
            if (spline == null)
            {
                return;
            }
            Destroy(spline);
        }

        public void Hide()
        {
            if (spline == null)
            {
                return;
            }
            spline.gameObject.SetActive(false);
        }

        public void Show()
        {
            if (spline == null)
            {
                return;
            }
            spline.gameObject.SetActive(true);
        }
    }
}