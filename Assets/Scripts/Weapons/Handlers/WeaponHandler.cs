using UnityEngine;
using System.Collections;

namespace ProjectChild.Weapons
{
    public interface IWeaponHandler
    {
        void OnWeaponAnimationStart();

        void OnWeaponAnimationEnd();
    }

    public class WeaponHandler : MonoBehaviour, IWeaponHandler
    {
        [SerializeField] protected Weapon weapon = null;

        public virtual void OnWeaponAnimationEnd() { }

        public virtual void OnWeaponAnimationStart() { }
    }
}