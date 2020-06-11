using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {
    public string name;
    public float value;

    public Stat(string name, float value) {
        this.name = name;
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

    private const float MAX_BASE_STAT_VALUES = 15, STARTING_STAT = 1;
    private const int XP_MULTIPLIER = 100;

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

    // Temporary stats
    [SerializeField]
    private float hp, shield, stamina, ammo;

    private void SetDefaultStats() {
        shieldRecovery = new Stat("Shield recovery", STARTING_STAT);
        staminaRecovery = new Stat("Stamina recovery", STARTING_STAT);
        ammoRecovery = new Stat("Ammo recovery", STARTING_STAT);

        dodgeRate = new Stat("Dodge rate", STARTING_STAT);
        criticalRate = new Stat("Critical rate", STARTING_STAT);
        rareItemFindRate = new Stat("Rare item find rate", STARTING_STAT);

        piercingDmg = new Stat("Piercing damage", STARTING_STAT);
        kineticDmg = new Stat("Kinetic damage", STARTING_STAT);
        energyDmg = new Stat("Energy damage", STARTING_STAT);

        piercingRes = new Stat("Piercing resistance", STARTING_STAT);
        kineticRes = new Stat("Kinetic resistance", STARTING_STAT);
        energyRes = new Stat("Energy resistance", STARTING_STAT);

        attackSpd = new Stat("Attack speed", STARTING_STAT);
        movementSpd = new Stat("Movement speed", STARTING_STAT);
        fireRate = new Stat("Fire rate", STARTING_STAT);

        level = 1;
        xp = 0;
        nextLevelUpXp = XP_MULTIPLIER;

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
        save.nextLevelUpXp = nextLevelUpXp;
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
        nextLevelUpXp = save.nextLevelUpXp;
    }

    private bool IncreaseStatValue(Stat stat) {
        // If stats is already full, return false
        if (stat.value == MAX_BASE_STAT_VALUES)
            return false;

        stat.value += 1;

        if (stat.value >= MAX_BASE_STAT_VALUES) {
            stat.value = MAX_BASE_STAT_VALUES;
        }

        return true;
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
            bool increased = IncreaseStatValue(stat);

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

        if (xp >= nextLevelUpXp) {
            LevelUp();
        }
    }

    private void LevelUp() {
        level ++; // Increase level

        // Get next xp multiplier
        int xpToAdd = level * XP_MULTIPLIER;   // Level 5 = 500;

        nextLevelUpXp += xpToAdd;

        // ADD LEVEL UP STUFF TO OPEN DIALOGUE LATER
    }
}
