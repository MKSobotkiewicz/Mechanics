using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Mechanics
{
    public class Cannon : MonoBehaviour
    {
        public Transform Exit;
        public GameObject MuzzleFlash;
        public List<Projectile> Projectiles;
        public float ReloadTime=5;
        public UnityEngine.Material GreenMaterial;
        public UnityEngine.Material RedMaterial;

        private new Rigidbody rigidbody;
        private Rigidbody cabinRigidbody;
        private bool readyTofire = true;
        private float reloadTimer=0;
        private uint projectileIndex=0;
        private Text ammoTypeText;
        private Text ammoLoadingText;
        private RectTransform loading;

        private static readonly string loadedText="LOADED";
        private static readonly string loadingText="LOADING";

        public void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            var tk = GetComponentInParent<TankController>();
            if (tk == null)
            {
                Debug.LogError(name+" can't find TankController");
            }
            cabinRigidbody=tk.GetComponent<Rigidbody>();
            var texts = cabinRigidbody.GetComponentsInChildren<Text>();
            foreach(var text in texts)
            {
                if(text.gameObject.tag == "AmmoText")
                {
                    ammoTypeText = text;
                    continue;
                }
                if (text.gameObject.tag == "AmmoLoadingText")
                {
                    ammoLoadingText = text;
                }
            }
            ammoTypeText.text = Projectiles[(int)projectileIndex].gameObject.name;
            var rts = cabinRigidbody.GetComponentsInChildren<RectTransform>();
            foreach (var rt in rts)
            {
                if (rt.gameObject.tag == "LoadingPanel")
                {
                    loading = rt;
                    break;
                }
            }
            foreach (var projectile in Projectiles)
            {
                //Load(projectile,null);
            }
            SetLoaded(1);
        }

        public void SetLoaded(float value)
        {
            if(value==1)
            {
                ammoLoadingText.text = loadedText;
                ammoLoadingText.material = RedMaterial;
                loading.gameObject.SetActive(false);
                return;
            }
            ammoLoadingText.text = loadingText;
            ammoLoadingText.material = GreenMaterial;
            readyTofire = false;
            reloadTimer = ReloadTime;
            loading.gameObject.SetActive(true);
        }

        public void Update()
        {
            if (!readyTofire)
            {
                reloadTimer -= UnityEngine.Time.deltaTime;
                loading.sizeDelta=new Vector2(((ReloadTime-reloadTimer)/ ReloadTime) *100, 0);
                if (reloadTimer<=0)
                {
                    readyTofire = true;
                    SetLoaded(1);
                }
            }
        }

        public void ChangeProjectile()
        {
            projectileIndex++;
            if(projectileIndex >= Projectiles.Count)
            {
                projectileIndex = 0;
            }
            ammoTypeText.text = Projectiles[(int)projectileIndex].gameObject.name;
            SetLoaded(0);
        }

        public bool ChangeProjectile(uint index)
        {
            if (index >= Projectiles.Count)
            {
                return false;
            }
            projectileIndex = index;
            ammoTypeText.text = Projectiles[(int)projectileIndex].gameObject.name;
            SetLoaded(0);
            return true;
        }

        public float GetDrop(float distance)
        {
            return Mathf.Pow(distance / Projectiles[(int)projectileIndex].Velocity, 2) * Physics.gravity.y * 0.5f;
        }

        public bool Fire()
        {
            if (readyTofire)
            {
                var projectile=Instantiate(Projectiles[(int)projectileIndex], Exit);
                rigidbody.AddRelativeForce(new Vector3(0,0, -100 ),ForceMode.VelocityChange);
                rigidbody.AddRelativeTorque(new Vector3(-5 , 0, 0), ForceMode.VelocityChange);
                cabinRigidbody.AddExplosionForce(1000000, transform.position,10,1);
                var muzzleFlash = Instantiate(MuzzleFlash, Exit);
                muzzleFlash.transform.parent = null;
                SetLoaded(0);
                return true;
            }
            return false;
        }
    }
}
