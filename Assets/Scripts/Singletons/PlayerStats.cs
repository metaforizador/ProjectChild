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

    // Stats to save and load (Declare defaults here)
    [SerializeField]
    private int maxHp, maxShield, level, xp, shieldRegen, armor, resistance, attackSpd, fireRate, dodge, critical, movementSpd;

    // Temporary stats
    [SerializeField]
    private int hp, shield;

    private void SetDefaultStats() {
        maxHp = 70;
        maxShield = 70;
        level = 1;
        xp = 0;
        shieldRegen = 12;
        armor = 8;
        resistance = 8;
        attackSpd = 13;
        fireRate = 13;
        dodge = 20;
        critical = 20;
        movementSpd = 12;

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
        save.dodge = dodge;
        save.critical = critical;
        save.movementSpd = movementSpd;
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
        dodge = save.dodge;
        critical = save.critical;
        movementSpd = save.movementSpd;

        hp = maxHp;
        shield = maxShield;
    }
}
