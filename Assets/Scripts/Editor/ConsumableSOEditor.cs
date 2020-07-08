using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConsumableSO)), CanEditMultipleObjects]
public class ConsumableSOEditor : Editor {

    public SerializedProperty
        nameProp, condition, consumableType,
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

    void OnEnable() {
        // Setup the SerializedProperties
        nameProp = serializedObject.FindProperty("name");
        condition = serializedObject.FindProperty("condition");
        consumableType = serializedObject.FindProperty("consumableType");

        // Scanner
        identificationChance = serializedObject.FindProperty("identificationChance");

        // Battery
        shieldRecoveryPercentage = serializedObject.FindProperty("shieldRecoveryPercentage");
        boostStaminaRecoverySpeed = serializedObject.FindProperty("boostStaminaRecoverySpeed");
        boostAmmoRecoverySpeed = serializedObject.FindProperty("boostAmmoRecoverySpeed");
        boostTimeInSeconds = serializedObject.FindProperty("boostTimeInSeconds");

        // Comsat Link & Rig
        chanceToBeSuccessful = serializedObject.FindProperty("chanceToBeSuccessful");

        // Scrap
        chanceToTurnIntoToy = serializedObject.FindProperty("chanceToTurnIntoToy");
        creditValue = serializedObject.FindProperty("creditValue");
        craftValue = serializedObject.FindProperty("craftValue");

        // Toy
        expToGain = serializedObject.FindProperty("expToGain");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(nameProp);
        EditorGUILayout.PropertyField(condition);
        EditorGUILayout.PropertyField(consumableType);

        ConsumableType type = (ConsumableType)consumableType.enumValueIndex;

        switch (type) {
            case ConsumableType.Scanner:
                EditorGUILayout.HelpBox("As a consumable, Scanner can identify items. It can also be used " +
                    "on certain stations to display the location of enemies, loot in a given map and traps.", MessageType.Info);
                EditorGUILayout.LabelField("Percentage chance to be successful", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(identificationChance);
                break;

            case ConsumableType.Battery:
                EditorGUILayout.HelpBox("As a consumable, it can be used to restore either shields, increase " +
                    "the speed of stamina recovery or ammo recovery. To know which one of those three it will increase, players " +
                    "would need to use a Scanner.", MessageType.Info);
                EditorGUILayout.LabelField("Percentage amount to recover", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(shieldRecoveryPercentage);
                EditorGUILayout.LabelField("Boost speed by multiplying with this value", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(boostStaminaRecoverySpeed);
                EditorGUILayout.PropertyField(boostAmmoRecoverySpeed);
                EditorGUILayout.LabelField("How long boost lasts", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(boostTimeInSeconds);
                break;

            case ConsumableType.ComsatLink:
                EditorGUILayout.HelpBox("As a consumable, it can call up an airstrike. It can also be used on item " +
                    "chests to send the found item back to the home base so the player can start the game with it." +
                    "When used in level exists, it can work as a town portal to send the player back to the beginning " +
                    "of the game with all his money and stats (items are lost, though).", MessageType.Info);
                EditorGUILayout.LabelField("Percentage chance", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(chanceToBeSuccessful);
                break;

            case ConsumableType.Rig:
                EditorGUILayout.HelpBox("As a consumable, it repairs some HP. On stations, it can also be used to upgrade " +
                    "weapons and armor.", MessageType.Info);
                EditorGUILayout.LabelField("Percentage chance", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(chanceToBeSuccessful);
                break;

            case ConsumableType.Scrap:
                EditorGUILayout.HelpBox("As a consumable, using it has a chance of turning it into a toy, which gives XP to the " +
                    "child. Success replaces item 'Scrap' with the item 'Toy'. On stations, it can be used to craft new weapons " +
                    "or armor and in vendors, turned in for credits. Credits allow the purchase of items on vendor. Once the scrap " +
                    "gets turned into a toy, it can no longer be used on stations or sold.", MessageType.Info);
                EditorGUILayout.LabelField("Percentage chance", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(chanceToTurnIntoToy);
                EditorGUILayout.LabelField("Credit amounts are not yet decided", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(creditValue);
                EditorGUILayout.LabelField("Craft values are not yet decided", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(craftValue);
                break;

            case ConsumableType.Toy:
                EditorGUILayout.HelpBox("As a consumable, it gives exp to the player.", MessageType.Info);
                EditorGUILayout.LabelField("Gives percentage amount of exp needed for next level", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(expToGain);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
