using UnityEngine;

namespace ProjectChild.Data
{
    [CreateAssetMenu(fileName = "Weapon Data", menuName = "Project Child/Create New Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public WeaponType type;
        public float damageBase = 1f;
    }

    public enum WeaponType
    {
        Undefined,
        GenericGun,
        GenericMelee
    }
}
