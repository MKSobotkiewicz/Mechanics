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

        private Map.Area area;

        private Units.UnitGenerator unitGenerator;

        public void Start()
        {
            var size = transform.localScale;
            transform.localScale = new Vector3(0,0,0);
            LeanTween.scale(gameObject,size, 0.2f).setEaseInOutSine();
            var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                unitGenerator = rootObject.GetComponentInChildren<Units.UnitGenerator>();
                if (unitGenerator != null)
                {
                    break;
                }
            }
        }
        
        public void CreateUnit()
        {
            unitGenerator.Generate(unitGenerator.Units[0],area);
        }

        public void SetArea(Map.Area _area)
        {
            area = _area;
            Name.text = area.name;
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
