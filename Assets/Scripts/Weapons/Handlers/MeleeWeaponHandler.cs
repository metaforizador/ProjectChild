using UnityEngine;
using System.Collections;

namespace ProjectChild.Weapons
{
    public class MeleeWeaponHandler : WeaponHandler
    {
        private MeleeWeapon meleeWeapon = null;

        private void Start()
        {
            var meleeWeapon = weapon as MeleeWeapon;
            if (meleeWeapon == null) Destroy(this);

            this.meleeWeapon = meleeWeapon;
        }

        public override void OnWeaponAnimationStart()
        {
            meleeWeapon.Reset();
        }

        public override void OnWeaponAnimationEnd()
        {
            
        }
    }
}