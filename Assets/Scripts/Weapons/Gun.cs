using UnityEngine;
using ProjectChild.Data;

namespace ProjectChild.Weapons
{
    public class Gun : Weapon
    {
        [SerializeField] private GameObject bulletPrefab = null;
        [SerializeField] private Transform firingPosition = null;
    }
}
