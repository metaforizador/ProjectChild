using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { Piercing, Kinetic, Energy };

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

    private const int XP_MULTIPLIER = 100;

    // Real min and max values for stats
    private const float RECOVERY_MIN_SPEED = 1, RECOVERY_MAX_SPEED = 4;
    private const float RESISTANCE_MIN_PERCENT = 0, RESISTANCE_MAX_PERCENT = 60;
    private const float DODGE_MIN_PERCENT = 0, DODGE_MAX_PERCENT = 50;

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

    // Other stats
    public int level { get; private set; }
    public int xp { get; private set; }
    public int nextLevelUpXp { get; private set; }

    private void SetDefaultStats() {
        shieldRecovery = new Stat("Shield recovery", Stat.STARTING_STAT, RECOVERY_MIN_SPEED, RECOVERY_MAX_SPEED);
        staminaRecovery = new Stat("Stamina recovery", Stat.STARTING_STAT, RECOVERY_MIN_SPEED, RECOVERY_MAX_SPEED);
        ammoRecovery = new Stat("Ammo recovery", Stat.STARTING_STAT, RECOVERY_MIN_SPEED, RECOVERY_MAX_SPEED);

        dodgeRate = new Stat("Dodge rate", Stat.STARTING_STAT, DODGE_MIN_PERCENT, DODGE_MAX_PERCENT);
        criticalRate = new Stat("Critical rate", Stat.STARTING_STAT, 0, 0);
        rareItemFindRate = new Stat("Rare item find rate", Stat.STARTING_STAT, 0, 0);

        piercingDmg = new Stat("Piercing damage", Stat.STARTING_STAT, 0, 0);
        kineticDmg = new Stat("Kinetic damage", Stat.STARTING_STAT, 0, 0);
        energyDmg = new Stat("Energy damage", Stat.STARTING_STAT, 0, 0);

        piercingRes = new Stat("Piercing resistance", Stat.STARTING_STAT, RESISTANCE_MIN_PERCENT, RESISTANCE_MAX_PERCENT);
        kineticRes = new Stat("Kinetic resistance", Stat.STARTING_STAT, RESISTANCE_MIN_PERCENT, RESISTANCE_MAX_PERCENT);
        energyRes = new Stat("Energy resistance", Stat.STARTING_STAT, RESISTANCE_MIN_PERCENT, RESISTANCE_MAX_PERCENT);

        attackSpd = new Stat("Attack speed", Stat.STARTING_STAT, 0, 0);
        movementSpd = new Stat("Movement speed", Stat.STARTING_STAT, 0, 0);
        fireRate = new Stat("Fire rate", Stat.STARTING_STAT, 0, 0);

        level = 1;
        xp = 0;
        nextLevelUpXp = XP_MULTIPLIER;
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
        save.nextLevelUpXp = nextLevelUpXp;
    }

    public void LoadPlayerStats(Save save) {
        Debug.Log(save.piercingDmg.name);
        shieldRecovery.LoadStat(save.shieldRecovery);
        staminaRecovery.LoadStat(save.staminaRecovery);
        ammoRecovery.LoadStat(save.ammoRecovery);

        dodgeRate.LoadStat(save.dodgeRate);
        criticalRate.LoadStat(save.criticalRate);
        rareItemFindRate.LoadStat(save.rareItemFindRate);

        piercingDmg.LoadStat(save.piercingDmg);
        kineticDmg.LoadStat(save.kineticDmg);
        energyDmg.LoadStat(save.energyDmg);

        piercingRes.LoadStat(save.piercingRes);
        kineticRes.LoadStat(save.kineticRes);
        energyRes.LoadStat(save.energyRes);

        attackSpd.LoadStat(save.attackSpd);
        movementSpd.LoadStat(save.movementSpd);
        fireRate.LoadStat(save.fireRate);

        level = save.level;
        xp = save.xp;
        nextLevelUpXp = save.nextLevelUpXp;
    }

    public void RandomizeGainedStat(WordsType type) {
        Stat[] stats;

        // Set correct stats to the array
        switch (type) {
            case WordsType.Nurturing:
                stats = new Stat[] { shieldRecovery, staminaRecovery, ammoRecovery };
                break;
            case WordsType.Rational:
                stats = new Stat[] { dodgeRate, criticalRate, rareItemFindRate };
                break;
            case WordsType.Idealistic:
                stats = new Stat[] { piercingDmg, kineticDmg, energyDmg };
                break;
            case WordsType.Stoic:
                stats = new Stat[] { piercingRes, kineticRes, energyRes };
                break;
            case WordsType.Nihilistic:
                stats = new Stat[] { attackSpd, movementSpd, fireRate };
                break;
            default:
                stats = new Stat[3]; // Not possible, but removes error "unassigned variable"
                break;
        }

        // Increase random stat from the array
        List<int> maxedStats = new List<int>();
        while (true) {

            int index = Random.Range(0, 3);

            // If stat is already checked and maxed out, randomize new index
            if (maxedStats.Contains(index))
                continue;

            Stat stat = stats[index];
            bool increased = stat.IncreaseLevel();

            if (!increased) {
                maxedStats.Add(index);
            } else {
                CanvasMaster.Instance.ShowStatGain(StatGainCanvas.CreateGainStatText(stat));
                break;
            }

            if (maxedStats.Count == stats.Length) {
                // Inform player that all stats are maxed out
                CanvasMaster.Instance.ShowStatGain(StatGainCanvas.CreateStatsMaxedText(type));
                break;
            }
        }
    }

    public void GainXP(int amount) {
        xp += amount;

        // Check if you gain level or multiple levels
        while (xp >= nextLevelUpXp)
                LevelUp();
    }

    private void LevelUp() {
        level ++; // Increase level

        // Get next xp multiplier
        int xpToAdd = level * XP_MULTIPLIER;   // Level 5 = 500;

        nextLevelUpXp += xpToAdd;

        // ADD LEVEL UP STUFF TO OPEN DIALOGUE LATER
    }
}
