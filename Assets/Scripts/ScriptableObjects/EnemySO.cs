﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemySO : ScriptableObject {

    public new string name;
    public int level;
    public int xp;

    public WeaponSO startingWeapon;
    public ArmorSO startingArmor;

    [Range(Stat.STARTING_STAT, Stat.MAX_BASE_STAT_VALUES)]
    public int shieldRecovery, staminaRecovery, dodgeRate, criticalRate,
        piercingDmg, kineticDmg, energyDmg, piercingRes, kineticRes, energyRes,
        attackSpd, movementSpd, fireRate;
}