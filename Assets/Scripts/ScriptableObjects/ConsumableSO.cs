using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType { Scanner, Battery, ComsatLink, Rig, Scrap, Toy }

[CreateAssetMenu(fileName = "New Consumable", menuName = "Consumable")]
public class ConsumableSO : PickableSO {

    public ConsumableType consumableType;
    public int quantity = 1;

    // Scanner
    public const string DESCRIPTION_SCANNER = "As a consumable, Scanner can identify items.It can also be used " +
                    "on certain stations to display the location of enemies, loot in a given map and traps.";
    [Range(0f, 100f)]
    public float identificationChance;

    // Battery
    public const string DESCRIPTION_BATTERY = "As a consumable, it can be used to restore either shields, increase " +
                    "the speed of stamina recovery or ammo recovery. To know which one of those three the battery will increase, it " +
                    "needs to be identified with a scanner.";
    [Range(0f, 100f)]
    public float shieldRecoveryPercentage;
    [Range(1.1f, 3f)]
    public float boostStaminaRecoverySpeed = 1.1f, boostAmmoRecoverySpeed = 1.1f;
    [Range(1, 60)]
    public int boostTimeInSeconds = 1;

    // Comsat Link & Rig
    public const string DESCRIPTION_COMSAT_LINK = "As a consumable, it can call up an airstrike. It can also be used on item " +
                    "chests to send the found item back to the home base so the player can start the game with it." +
                    "When used in level exists, it can work as a town portal to send the player back to the beginning " +
                    "of the game with all his money and stats (items are lost, though).";
    public const string DESCRIPTION_RIG = "As a consumable, it repairs some HP. On stations, it can also be used to upgrade " +
                    "weapons and armor.";
    [Range(0f, 100f)]
    public float chanceToBeSuccessful;

    // Scrap
    public const string DESCRIPTION_SCRAP = "As a consumable, using it has a chance of turning it into a toy, which gives XP to the " +
                    "child. Success replaces item 'Scrap' with the item 'Toy'. On stations, it can be used to craft new weapons " +
                    "or armor and in vendors, turned in for credits. Credits allow the purchase of items on vendor. Once the scrap " +
                    "gets turned into a toy, it can no longer be used on stations or sold.";
    [Range(0f, 100f)]
    public float chanceToTurnIntoToy;
    public int creditValue;
    [Range(1, 4)]
    public int craftValue = 1;

    // Toy
    public const string DESCRIPTION_TOY = "As a consumable, it gives exp to the player.";
    [Range(0f, 100f)]
    public float expToGain;
}
