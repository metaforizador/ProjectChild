using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class ArmorSO : ScriptableObject {

    public new string name;
    public GameObject armorPrefab;

    [Header("Decrease delay by %")]
    public float decreaseShieldRecoveryDelay;

    [Header("Default shield capacity is 100")]
    public int increaseShield;

    [Header("Decrease rate by %")]
    public float decreaseOpponentCriticalRate;

    [Header("Decrease multiplier by % (default is 200%)")]
    public float decreaseOpponentCriticalMultiplier;

    [Header("Decrease speed by % (because armor is heavy)")]
    public float reduceMovementSpeed;

    [Header("Decrease rate by % (because armor is heavy)")]
    public float reduceStaminaRecoveryRate;
}
