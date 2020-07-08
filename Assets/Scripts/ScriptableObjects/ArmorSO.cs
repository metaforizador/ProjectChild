﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class ArmorSO : PickableSO {

    public GameObject armorPrefab;

    [Header("Decrease shield recovery delay by %")]
    [Range(0f, 100f)]
    public float decreaseShieldRecoveryDelay;

    [Header("Starting shield capacity is 100")]
    public int increaseShield;

    [Header("Decrease opponent's critical rate by %")]
    [Range(0f, Stat.CRITICAL_MAX_PERCENT)]
    public float decreaseOpponentCriticalRate;

    [Header("Decrease opponent's critical multiplier by %")]
    [Range(0f, 100f)]
    public float decreaseOpponentCriticalMultiplier;

    [Header("Decrease movement speed by % (because armor is heavy)")]
    public float reduceMovementSpeed;

    [Header("Decrease stamina recovery rate by % (because armor is heavy)")]
    public float reduceStaminaRecoveryRate;
}
