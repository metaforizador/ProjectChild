using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
<<<<<<< HEAD
    // Player stats
    public int maxHp, maxShield, level, xp, shieldRegen, armor, resistance, attackSpd, fireRate, dodge, critical, movementSpd;
=======
    //////// Player stats ////////
    // Nurturing
    public Stat shieldRecovery, staminaRecovery, ammoRecovery;
    // Rational
    public Stat dodgeRate, criticalRate, rareItemFindRate;
    // Idealistic
    public Stat piercingDmg, kineticDmg, fireDmg, energyDmg, slashDmg;
    // Stoic
    public Stat piercingRes, kineticRes, fireRes, energyRes, slashRes;
    // Nihilistic
    public Stat attackSpd, movementSpd, fireRate, skillCooldown;
    // Other stats
    public int level, xp, nextLevelUpXp;

    //////// Player stats ////////
    // Mood = mood for question, List<string> = questions
    public Dictionary<Mood, List<string>> askedQuestions;
    // WordsType = type of reply, List<string> = replies
    public Dictionary<WordsType, List<string>> givenReplies;
>>>>>>> toni
}
