using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Camera
{
    public class Init : MonoBehaviour
    {
        public void Awake()
        {
            LeanTween.init(1000);
        }
    }
}
