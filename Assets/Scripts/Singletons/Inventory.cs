﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    // Make class singleton and destroy if script already exists
    private static Inventory _instance; // **<- reference link to the class
    public static Inventory Instance { get { return _instance; } }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private List<PickableSO> pickableItems;

    public void LoadInventory(Save save) {
        save.inventoryItems = pickableItems;
    }

    public void SaveInventory(Save save) {
        pickableItems = save.inventoryItems;
    }
}
