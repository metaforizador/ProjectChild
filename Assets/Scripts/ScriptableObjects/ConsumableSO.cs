using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType { Scanner, Battery, ComsatLink, Rig, Scrap, Toy }

[CreateAssetMenu(fileName = "New Consumable", menuName = "Consumable")]
public class ConsumableSO : PickableSO {

    public ConsumableType consumableType;

    // Battery
    [Range(0f, 100f)]
    public float shieldRecoveryPercentage;
    [Range(1.1f, 3f)]
    public float boostStaminaRecoverySpeed = 1.1f, boostAmmoRecoverySpeed = 1.1f;
    [Range(1, 60)]
    public int boostTimeInSeconds = 1;
}
