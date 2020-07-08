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
        shieldRecoveryPercentage, boostStaminaRecoverySpeed, boostAmmoRecoverySpeed, boostTimeInSeconds;

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
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(nameProp);
        EditorGUILayout.PropertyField(condition);
        EditorGUILayout.PropertyField(consumableType);

        ConsumableType type = (ConsumableType)consumableType.enumValueIndex;

        switch (type) {
            case ConsumableType.Scanner:
                EditorGUILayout.LabelField("Percentage chance to be successful", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(identificationChance);
                break;

            case ConsumableType.Battery:
                EditorGUILayout.LabelField("Percentage amount to recover", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(shieldRecoveryPercentage);
                EditorGUILayout.LabelField("Boost speed by multiplying with this value", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(boostStaminaRecoverySpeed);
                EditorGUILayout.PropertyField(boostAmmoRecoverySpeed);
                EditorGUILayout.LabelField("How long boost lasts", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(boostTimeInSeconds);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
