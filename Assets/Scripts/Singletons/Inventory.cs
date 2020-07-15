using System.Collections;
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

    // Equipped weapon and armor
    public WeaponSO equippedWeapon;
    public ArmorSO equippedArmor;

    private List<PickableSO> pickableItems = new List<PickableSO>();

    void Start() {
        // Add test items
        PickableSO[] pickArray = Resources.LoadAll<PickableSO>("ScriptableObjects/PickableItems/Consumables/");

        foreach (PickableSO item in pickArray) {
            if (item is ConsumableSO)
                AddConsumable((ConsumableSO)item);
            else {
                pickableItems.Add(item);
            }
        }
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
            // Contains can't be used since consumable and item in inventory have different pointers
            // So use name (since it's unique) to check for items
            if (item.name.Equals(consumable.name)) {
                ConsumableSO con = (ConsumableSO)item;
                con.quantity++;
                return;
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

        return consumables;
    }

    public void UseConsumable(ConsumableSO consumable) {
        consumable.quantity--;
        CanvasMaster cm = CanvasMaster.Instance;
        cm.hotbarCanvas.GetComponent<HotbarCanvas>().RefreshHotbarImages();
        cm.inventoryCanvas.GetComponent<InventoryCanvas>().RefreshConsumables();

        // If all consumables are used, remove item from inventory
        if (consumable.quantity <= 0) {
            pickableItems.Remove(consumable);
        }
    }
}
