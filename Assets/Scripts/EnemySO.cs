using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies / New Enemy")]
public class EnemyData : EnemySO { }

[System.Serializable]
public class EnemySO : ScriptableObject {

    public new string name;

    [Range(PlayerStats.STARTING_STAT, PlayerStats.MAX_BASE_STAT_VALUES)]
    public int shieldRecovery, staminaRecovery, ammoRecovery, dodgeRate, criticalRate,
        piercingDmg, kineticDmg, energyDmg, piercingRes, kineticRes, energyRes,
        attackSpd, movementSpd, fireRate;
}
