using ProjectChild.Data;
using UnityEngine;

namespace ProjectChild.Weapons
{
    public interface IWeapon
    {
        WeaponType GetWeaponType();
    }

    public class Weapon : MonoBehaviour, IWeapon 
    {
        [SerializeField] private WeaponData data = null;

        public WeaponType GetWeaponType()
        {
            return data.type;
        }
    }
}
