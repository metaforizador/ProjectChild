﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterParent {

    [SerializeField]
    private EnemySO scriptableObject;

    private Player player;

    public override void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        // Calculate stats from scriptableObject values
        shieldRecovery = Stat.CalculateValue(Stat.RECOVERY_MIN_SPEED, Stat.RECOVERY_MAX_SPEED, scriptableObject.shieldRecovery);
        staminaRecovery = Stat.CalculateValue(Stat.RECOVERY_MIN_SPEED, Stat.RECOVERY_MAX_SPEED, scriptableObject.staminaRecovery);
        ammoRecovery = Stat.CalculateValue(Stat.RECOVERY_MIN_SPEED, Stat.RECOVERY_MAX_SPEED, scriptableObject.ammoRecovery);

        dodgeRate = Stat.CalculateValue(Stat.DODGE_MIN_PERCENT, Stat.DODGE_MAX_PERCENT, scriptableObject.dodgeRate);
        criticalRate = Stat.CalculateValue(Stat.CRITICAL_MIN_PERCENT, Stat.CRITICAL_MAX_PERCENT, scriptableObject.criticalRate);

        piercingDmg = Stat.CalculateValue(Stat.DAMAGE_MIN_BOOST, Stat.DAMAGE_MAX_BOOST, scriptableObject.piercingDmg);
        kineticDmg = Stat.CalculateValue(Stat.DAMAGE_MIN_BOOST, Stat.DAMAGE_MAX_BOOST, scriptableObject.kineticDmg);
        energyDmg = Stat.CalculateValue(Stat.DAMAGE_MIN_BOOST, Stat.DAMAGE_MAX_BOOST, scriptableObject.energyDmg);

        piercingRes = Stat.CalculateValue(Stat.RESISTANCE_MIN_PERCENT, Stat.RESISTANCE_MAX_PERCENT, scriptableObject.piercingRes);
        kineticRes = Stat.CalculateValue(Stat.RESISTANCE_MIN_PERCENT, Stat.RESISTANCE_MAX_PERCENT, scriptableObject.kineticRes);
        energyRes = Stat.CalculateValue(Stat.RESISTANCE_MIN_PERCENT, Stat.RESISTANCE_MAX_PERCENT, scriptableObject.energyRes);

        attackSpd = Stat.CalculateValue(Stat.ATTACK_MIN_SPEED, Stat.ATTACK_MAX_SPEED, scriptableObject.attackSpd);
        movementSpd = Stat.CalculateValue(Stat.MOVEMENT_MIN_SPEED, Stat.MOVEMENT_MAX_SPEED, scriptableObject.movementSpd);
        fireRate = Stat.CalculateValue(Stat.FIRE_RATE_MIN_SPEED, Stat.FIRE_RATE_MAX_SPEED, scriptableObject.fireRate);

        base.Start();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("PlayerBullet")) {
            float damage;
            DamageType type;
            player.CalculateBulletDamage(out type, out damage);
            TakeDamage(type, damage);
        }
    }
}
