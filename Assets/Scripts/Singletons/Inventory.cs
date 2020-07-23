﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    // Equipped weapon and armor
    public WeaponSO equippedWeapon;
    public ArmorSO equippedArmor;

    private List<PickableSO> pickableItems = new List<PickableSO>();

    void Start() {
        // Add test items
        PickableSO[] pickArray = Resources.LoadAll<PickableSO>("ScriptableObjects/PickableItems/Consumables/");

        foreach (PickableSO so in pickArray) {
            PickableSO item = Instantiate(so);
            if (item is ConsumableSO)
                AddConsumable((ConsumableSO)item);
            else {
                pickableItems.Add(item);
            }
        }

        // Test different type battery
        ConsumableSO con = Instantiate(Resources.Load<ConsumableSO>("ScriptableObjects/PickableItems/Consumables/Battery IV"));
        con.batteryType = ConsumableSO.BatteryType.Ammo;
        AddConsumable(con);
    }

    public void LoadInventory(Save save) {
        save.equippedWeapon = equippedWeapon;
        save.equippedArmor = equippedArmor;
        save.inventoryItems = pickableItems;
    }

    public void SaveInventory(Save save) {
        equippedWeapon = save.equippedWeapon;
        equippedArmor = save.equippedArmor;
        pickableItems = save.inventoryItems;
    }

    public void AddConsumable(ConsumableSO consumable) {
        // If the consumable is already in inventory, add +1 to quantity and return
        foreach (PickableSO item in pickableItems) {
            if (item is ConsumableSO) {
                ConsumableSO con = (ConsumableSO)item;
                // Check using different equals methods based on ConsumableType
                if (con.EqualsConsumable(consumable)) {
                    con.quantity++;
                    return;
                }
            }
        }

        // Else add new item
        consumable.quantity = 1;
        pickableItems.Add(consumable);
    }

    public List<ConsumableSO> GetConsumables() {
        List<ConsumableSO> consumables = new List<ConsumableSO>();

        foreach (PickableSO item in pickableItems) {
            if (item is ConsumableSO)
                consumables.Add((ConsumableSO) item);
        }

        // Sort list by name
        var sortedList = consumables.OrderBy(go => go.name).ToList();

        return sortedList;
    }

    public void UseConsumable(ConsumableSO consumable) {
        bool removeItem = true;
        // Use items (Might have to be moved to somewhere else later)
        switch (consumable.consumableType) {
            case ConsumableType.Battery:
            case ConsumableType.ComsatLink:
            case ConsumableType.Rig:
            case ConsumableType.Toy:
                Player player = PlayerStats.Instance.player;
                player.UseConsumable(consumable);
                break;
            case ConsumableType.Scrap:
                if (consumable.CheckIfUsageSuccessful()) {
                    ConsumableSO toy = consumable.ConvertScrapToToy();
                    AddConsumable(toy);
                    CanvasMaster.Instance.topInfoCanvas.ShowScrapToToy(consumable.name, toy.name);
                }
                break;
            case ConsumableType.Scanner:
                removeItem = false; // Don't remove scanner at this point
                IdentifyCanvas ic = CanvasMaster.Instance.identifyCanvas;
                ic.OpenIdentifyCanvas(consumable);

                // If there are no identifiable items, close the canvas
                if (ic.isEmpty) {
                    CanvasMaster.Instance.topInfoCanvas.ShowIdentifiableEmpty();
                    ic.CloseIdenfityCanvas();
                }
                break;
        }

        if (removeItem)
            RemoveConsumable(consumable);
    }

    public void RemoveConsumable(ConsumableSO consumable) {
        consumable.quantity--;
        // If all consumables are used, remove item from inventory
        if (consumable.quantity <= 0) {
            pickableItems.Remove(consumable);
        }

        RefreshInventoryItems();
    }

    public void RefreshInventoryItems() {
        // Refresh hotbar and inventorycanvas items
        CanvasMaster cm = CanvasMaster.Instance;
        cm.hotbarCanvas.GetComponent<HotbarCanvas>().RefreshHotbarImages();
        cm.inventoryCanvas.GetComponent<InventoryCanvas>().RefreshConsumables();
    }
}