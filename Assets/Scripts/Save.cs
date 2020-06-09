﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
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
    // string = areas, List<string> = questions
    public Dictionary<string, List<string>> askedQuestions;
    // WordsType = type of reply, List<string> = replies
    public Dictionary<WordsType, List<string>> givenReplies;
}
