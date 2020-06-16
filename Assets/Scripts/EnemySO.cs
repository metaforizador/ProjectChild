using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemySO : ScriptableObject {

    public new string name;

    [Range(Stat.STARTING_STAT, Stat.MAX_BASE_STAT_VALUES)]
    public int shieldRecovery, staminaRecovery, ammoRecovery, dodgeRate, criticalRate,
        piercingDmg, kineticDmg, energyDmg, piercingRes, kineticRes, energyRes,
        attackSpd, movementSpd, fireRate;
}
