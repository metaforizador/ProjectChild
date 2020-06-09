using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stat {
    public float value;

    public Stat(float value) {
        this.value = value;
    }
}

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

    private const float MAX_LEVEL = 99, MAX_BASE_STAT_VALUES = 25, STARTING_STAT = 1;

    // Stats to save and load
    // Nurturing
    public Stat shieldRecovery { get; private set; }
    public Stat staminaRecovery { get; private set; }
    public Stat ammoRecovery { get; private set; }

    // Rational
    public Stat dodgeRate { get; private set; }
    public Stat criticalRate { get; private set; }
    public Stat rareItemFindRate { get; private set; }

    // Idealistic
    public Stat piercingDmg { get; private set; }
    public Stat kineticDmg { get; private set; }
    public Stat energyDmg { get; private set; }

    // Stoic
    public Stat piercingRes { get; private set; }
    public Stat kineticRes { get; private set; }
    public Stat energyRes { get; private set; }

    // Nihilistic
    public Stat attackSpd { get; private set; }
    public Stat movementSpd { get; private set; }
    public Stat fireRate { get; private set; }

    [SerializeField]    // Other stats
    private int level, xp;

    // Temporary stats
    [SerializeField]
    private float hp, shield, stamina, ammo;

    private void SetDefaultStats() {
        shieldRecovery = new Stat(STARTING_STAT);
        staminaRecovery = new Stat(STARTING_STAT);
        ammoRecovery = new Stat(STARTING_STAT);

        dodgeRate = new Stat(STARTING_STAT);
        criticalRate = new Stat(STARTING_STAT);
        rareItemFindRate = new Stat(STARTING_STAT);

        piercingDmg = new Stat(STARTING_STAT);
        kineticDmg = new Stat(STARTING_STAT);
        energyDmg = new Stat(STARTING_STAT);

        piercingRes = new Stat(STARTING_STAT);
        kineticRes = new Stat(STARTING_STAT);
        energyRes = new Stat(STARTING_STAT);

        attackSpd = new Stat(STARTING_STAT);
        movementSpd = new Stat(STARTING_STAT);
        fireRate = new Stat(STARTING_STAT);

        level = 1;
        xp = 0;

        hp = 100;
        shield = 100;
        stamina = 100;
        ammo = 100;
    }

    public void SavePlayerStats(Save save) {
        save.shieldRecovery = shieldRecovery;
        save.staminaRecovery = staminaRecovery;
        save.ammoRecovery = ammoRecovery;

        save.dodgeRate = dodgeRate;
        save.criticalRate = criticalRate;
        save.rareItemFindRate = rareItemFindRate;

        save.piercingDmg = piercingDmg;
        save.kineticDmg = kineticDmg;
        save.energyDmg = energyDmg;

        save.piercingRes = piercingRes;
        save.kineticRes = kineticRes;
        save.energyRes = energyRes;

        save.attackSpd = attackSpd;
        save.movementSpd = movementSpd;
        save.fireRate = fireRate;

        save.level = level;
        save.xp = xp;
    }

    public void LoadPlayerStats(Save save) {
        shieldRecovery = save.shieldRecovery;
        staminaRecovery = save.staminaRecovery;
        ammoRecovery = save.ammoRecovery;

        dodgeRate = save.dodgeRate;
        criticalRate = save.criticalRate;
        rareItemFindRate = save.rareItemFindRate;

        piercingDmg = save.piercingDmg;
        kineticDmg = save.kineticDmg;
        energyDmg = save.energyDmg;

        piercingRes = save.piercingRes;
        kineticRes = save.kineticRes;
        energyRes = save.energyRes;

        attackSpd = save.attackSpd;
        movementSpd = save.movementSpd;
        fireRate = save.fireRate;

        level = save.level;
        xp = save.xp;
    }

    private void IncreaseStatValue(Stat stat, float amount) {
        if (stat.value == MAX_BASE_STAT_VALUES) {
            // Inform player somehow (ADD LATER)
            return;
        }

        stat.value += amount;

        if (stat.value >= MAX_BASE_STAT_VALUES) {
            // Inform player that stat is now maxed out (ADD LATER)
            stat.value = MAX_BASE_STAT_VALUES;
        }
    }


}
