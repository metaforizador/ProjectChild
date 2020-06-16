using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private EnemySO scriptableObject;

    private float hp, shield, stamina, ammo;
    private bool alive;

    private float shieldRecovery, staminaRecovery, ammoRecovery, dodgeRate, criticalRate,
        piercingDmg, kineticDmg, energyDmg, piercingRes, kineticRes, energyRes,
        attackSpd, movementSpd, fireRate;

    void Start() {
        hp = 100;
        shield = 100;
        stamina = 100;
        ammo = 100;
        alive = true;

        shieldRecovery = Stat.CalculateValue(Stat.RECOVERY_MIN_SPEED, Stat.RECOVERY_MAX_SPEED, scriptableObject.shieldRecovery);
        staminaRecovery = Stat.CalculateValue(Stat.RECOVERY_MIN_SPEED, Stat.RECOVERY_MAX_SPEED, scriptableObject.staminaRecovery);
        ammoRecovery = Stat.CalculateValue(Stat.RECOVERY_MIN_SPEED, Stat.RECOVERY_MAX_SPEED, scriptableObject.ammoRecovery);

        dodgeRate = Stat.CalculateValue(Stat.DODGE_MIN_PERCENT, Stat.DODGE_MAX_PERCENT, scriptableObject.dodgeRate);
        criticalRate = Stat.CalculateValue(Stat.CRITICAL_MIN_PERCENT, Stat.CRITICAL_MAX_PERCENT, scriptableObject.criticalRate);

        piercingDmg = Stat.CalculateValue(Stat.DAMAGE_MIN_PERCENT, Stat.DAMAGE_MAX_PERCENT, scriptableObject.piercingDmg);
        kineticDmg = Stat.CalculateValue(Stat.DAMAGE_MIN_PERCENT, Stat.DAMAGE_MAX_PERCENT, scriptableObject.kineticDmg);
        energyDmg = Stat.CalculateValue(Stat.DAMAGE_MIN_PERCENT, Stat.DAMAGE_MAX_PERCENT, scriptableObject.energyDmg);

        piercingRes = Stat.CalculateValue(Stat.RESISTANCE_MIN_PERCENT, Stat.RESISTANCE_MAX_PERCENT, scriptableObject.piercingRes);
        kineticRes = Stat.CalculateValue(Stat.RESISTANCE_MIN_PERCENT, Stat.RESISTANCE_MAX_PERCENT, scriptableObject.kineticRes);
        energyRes = Stat.CalculateValue(Stat.RESISTANCE_MIN_PERCENT, Stat.RESISTANCE_MAX_PERCENT, scriptableObject.energyRes);

        attackSpd = Stat.CalculateValue(Stat.ATTACK_MIN_SPEED, Stat.ATTACK_MAX_SPEED, scriptableObject.attackSpd);
        movementSpd = Stat.CalculateValue(Stat.MOVEMENT_MIN_SPEED, Stat.MOVEMENT_MAX_SPEED, scriptableObject.movementSpd);
        fireRate = Stat.CalculateValue(Stat.FIRE_RATE_MIN_SPEED, Stat.FIRE_RATE_MAX_SPEED, scriptableObject.fireRate);
    }

    public void TakeDamage(float amount) {
        hp -= amount;

        if (hp <= 0) {
            Die();
        }
    }

    private void Die() {
        hp = 0;
        alive = false;
        Destroy(gameObject); // Destroy for now
    }
}
