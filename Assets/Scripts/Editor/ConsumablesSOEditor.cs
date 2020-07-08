using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConsumablesSOEditor)), CanEditMultipleObjects]
public class ConsumablesSOEditor : Editor {

    public SerializedProperty
        nameProp, condition, consumableType,
        // Scanner

        // Battery
        shieldRecoveryPercentage, boostStaminaRecoverySpeed, boostAmmoRecoverySpeed, boostTimeInSeconds;

    void OnEnable() {
        // Setup the SerializedProperties
        nameProp = serializedObject.FindProperty("name");
        condition = serializedObject.FindProperty("condition");
        consumableType = serializedObject.FindProperty("consumableType");
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
            case ConsumableType.Battery:
                EditorGUILayout.LabelField("Shield recovery percentage", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(shieldRecoveryPercentage);
                EditorGUILayout.LabelField("Boost by multiplying by this value", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(boostStaminaRecoverySpeed);
                EditorGUILayout.PropertyField(boostAmmoRecoverySpeed);
                EditorGUILayout.LabelField("How long boost lasts", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(boostTimeInSeconds);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
