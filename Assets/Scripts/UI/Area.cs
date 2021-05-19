using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class Area : MonoBehaviour
    {
        public Text Name;

        public void Start()
        {
            var size = transform.localScale;
            transform.localScale = new Vector3(0,0,0);
            LeanTween.scale(gameObject,size, 0.2f).setEaseInOutSine();
        }

        public void SetName(string name)
        {
            Name.text = name;
        }


        public void Destroy()
        {
            LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.2f).setEaseInOutSine().setOnComplete(() => Remove());
        }

        private void Remove()
        {
            GameObject.Destroy(this);
        }
    }
}
