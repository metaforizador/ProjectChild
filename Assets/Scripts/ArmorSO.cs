using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class ArmorSO : ScriptableObject {

    public new string name;
    public GameObject armorPrefab;

    [Header("Decrease shield recovery delay by %")]
    [Range(0f, 100f)]
    public float decreaseShieldRecoveryDelay;

    [Header("Default shield capacity is 100")]
    public int increaseShield;

    [Header("Decrease opponent's critical rate by %")]
    public float decreaseOpponentCriticalRate;

    [Header("Decrease opponent's critical multiplier by % (default is 200%)")]
    public float decreaseOpponentCriticalMultiplier;

    [Header("Decrease movement speed by % (because armor is heavy)")]
    public float reduceMovementSpeed;

    [Header("Decrease stamina recovery rate by % (because armor is heavy)")]
    public float reduceStaminaRecoveryRate;
}
