using UnityEngine;

namespace ProjectChild.Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Project Child/Create New Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public string name;
        public int level;
        public int xp;

        public WeaponData defaultWeapon;
        // TODO: add armor data

        [Range(Stat.STARTING_STAT, Stat.MAX_BASE_STAT_VALUES)]
        public int shieldRecovery, staminaRecovery, dodgeRate, criticalRate,
            piercingDmg, kineticDmg, energyDmg, piercingRes, kineticRes, energyRes,
            attackSpd, movementSpd, fireRate;
    }
}
