using UnityEngine;

namespace ProjectChild.Data
{
    [CreateAssetMenu(fileName = "GunData", menuName = "Project Child/Create New Gun Data")]
    public class GunData : WeaponData
    {
        [Header("Bullet Properties")]
        public GameObject bulletPrefab;
        public float bullerDamage = 1f;
        public float bulletSpeed = 1f;
        public int bulletsPerClip = 6;
        public float fireRate = 1f;
        public float reloadTime = 1f;
        public DamageType damageType;

        [Header("Audio")]
        [SerializeField] private AudioClip shootingSound = null;
        [SerializeField] private AudioClip reloadingSound = null;
    }
}
