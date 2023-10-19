using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Units
{
    public class WeaponTrigger : MonoBehaviour
    {
        public Weapon Weapon;

        public void PlayAttackAnimation()
        {
            Weapon.PlayAttackAnimation();
        }
    }
}
