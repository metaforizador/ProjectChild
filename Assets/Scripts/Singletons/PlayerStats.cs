using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    
    // Make class static and destroy if script already exists
    private static PlayerStats _instance; // **<- reference link to the class
    public static PlayerStats Instance { get { return _instance; } }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SetDefaultStats();
        } else {
            Destroy(gameObject);
        }
    }

    private const int MAX_HEALTH_VALUES = 999, MAX_LEVEL = 99, MAX_BASE_STAT_VALUES = 25, MAX_PERCENTAGE = 70;

    // Stats to save and load
    [SerializeField]
    private int maxHp, maxShield, level, xp, shieldRegen, armor, resistance, attackSpd, fireRate, movementSpd, dodge, critical;

    // Temporary stats
    [SerializeField]
    private int hp, shield;

    // STATS WHICH HAS NOT BEEN TALKED ABOUT:
    /*- Damages and resistances: Electric, Fire, Plasma, Explosive
     *- Damage stat itself?
     *- Item type finding rate
     *- Item rarity finding rate
     * */

    private void SetDefaultStats() {
        maxHp = 70;         // max = MAX_HEALTH_VALUES
        maxShield = 70;     // max = MAX_HEALTH_VALUES
        level = 1;          // max = MAX_LEVEL
        xp = 0;             
        shieldRegen = 1;    // max = MAX_BASE_STAT_VALUES
        armor = 1;          // max = MAX_BASE_STAT_VALUES
        resistance = 1;     // max = MAX_BASE_STAT_VALUES
        attackSpd = 1;      // max = MAX_BASE_STAT_VALUES    
        fireRate = 1;       // max = MAX_BASE_STAT_VALUES
        movementSpd = 1;    // max = MAX_BASE_STAT_VALUES
        dodge = 10;         // max = MAX_PERCENTAGE
        critical = 10;      // max = MAX_PERCENTAGE

        hp = maxHp;
        shield = maxShield;
    }

    public void SavePlayerStats(Save save) {
        save.maxHp = maxHp;
        save.maxShield = maxShield;
        save.level = level;
        save.xp = xp;
        save.shieldRegen = shieldRegen;
        save.armor = armor;
        save.resistance = resistance;
        save.attackSpd = attackSpd;
        save.fireRate = fireRate;
        save.movementSpd = movementSpd;
        save.dodge = dodge;
        save.critical = critical;
    }

    public void LoadPlayerStats(Save save) {
        maxHp = save.maxHp;
        maxShield = save.maxShield;
        level = save.level;
        xp = save.xp;
        shieldRegen = save.shieldRegen;
        armor = save.armor;
        resistance = save.resistance;
        attackSpd = save.attackSpd;
        fireRate = save.fireRate;
        movementSpd = save.movementSpd;
        dodge = save.dodge;
        critical = save.critical;

        hp = maxHp;
        shield = maxShield;
    }

    public void IncreaseMaxHp(int amount) {
        maxHp += amount;

        if (maxHp >= MAX_HEALTH_VALUES)
            maxHp = MAX_HEALTH_VALUES;
    }

    public void IncreaseMaxShield(int amount) {
        maxShield += amount;

        if (maxShield >= MAX_HEALTH_VALUES)
            maxShield = MAX_HEALTH_VALUES;
    }

    public void IncreaseShieldRegen(int amount) {
        shieldRegen += amount;

        if (shieldRegen >= MAX_BASE_STAT_VALUES)
            shieldRegen = MAX_BASE_STAT_VALUES;
    }

    public void IncreaseArmor(int amount) {
        armor += amount;

        if (armor >= MAX_BASE_STAT_VALUES)
            armor = MAX_BASE_STAT_VALUES;
    }

    public void IncreaseResistance(int amount) {
        resistance += amount;

        if (resistance >= MAX_BASE_STAT_VALUES)
            resistance = MAX_BASE_STAT_VALUES;
    }

    public void IncreaseAttackSpd(int amount) {
        attackSpd += amount;

        if (attackSpd >= MAX_BASE_STAT_VALUES)
            attackSpd = MAX_BASE_STAT_VALUES;
    }

    public void IncreaseFireRate(int amount) {
        fireRate += amount;

        if (fireRate >= MAX_BASE_STAT_VALUES)
            fireRate = MAX_BASE_STAT_VALUES;
    }

    public void IncreaseMovementSpd(int amount) {
        movementSpd += amount;

        if (movementSpd >= MAX_BASE_STAT_VALUES)
            movementSpd = MAX_BASE_STAT_VALUES;
    }

    public void IncreaseDodge(int amount) {
        dodge += amount;

        if (dodge >= MAX_PERCENTAGE)
            dodge = MAX_PERCENTAGE;
    }

    public void IncreaseCritical(int amount) {
        critical += amount;

        if (critical >= MAX_PERCENTAGE)
            critical = MAX_PERCENTAGE;
    }
}
