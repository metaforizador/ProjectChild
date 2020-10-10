using ProjectChild.Data;
using UnityEngine;

namespace ProjectChild.Weapons
{
    public interface IWeapon
    {
        WeaponType GetWeaponType();
    }

    public class Weapon<T> : MonoBehaviour, IWeapon where T : WeaponData
    {
        [SerializeField] private T data = null;

        public WeaponType GetWeaponType()
        {
            return data.type;
        }
    }
}
