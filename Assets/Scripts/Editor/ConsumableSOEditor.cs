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

        // Comsat Link
        chanceToBeSuccessful;

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

        // Comsat Link
        chanceToBeSuccessful = serializedObject.FindProperty("chanceToBeSuccessful");
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
                EditorGUILayout.PropertyField(chanceToBeSuccessful);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
