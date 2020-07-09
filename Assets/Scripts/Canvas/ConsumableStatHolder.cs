using UnityEngine;
using TMPro;

public class ConsumableStatHolder : MonoBehaviour{
    public TextMeshProUGUI
        name,

        // Scanner
        identificationChance,

        // Battery
        shieldRecoveryPercentage, boostStaminaRecoverySpeed, boostAmmoRecoverySpeed, boostTimeInSeconds,

        // Comsat Link & Rig
        chanceToBeSuccessful,

        // Scrap
        chanceToTurnIntoToy, creditValue, craftValue,

        // Toy
        expToGain;
}
