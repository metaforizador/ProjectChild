using System.Collections;
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

    private List<ConsumableSO> inventoryConsumables = new List<ConsumableSO>();

    void Start() {
        // Add test items
        List<ConsumableSO> consumables = SOCreator.Instance.GetAllConsumables();

        foreach (ConsumableSO con in consumables)
            AddConsumable(con);
    }

    public void LoadInventory(Save save) {
        SOCreator creator = SOCreator.Instance;
        equippedWeapon = creator.CreateWeapon(save.equippedWeapon);
        equippedArmor = creator.CreateArmor(save.equippedArmor);
        // Load consumables
        inventoryConsumables.Clear();
        foreach (SerializableConsumableSO serialized in save.inventoryConsumables) {
            ConsumableSO con = creator.CreateConsumable(serialized);
            inventoryConsumables.Add(con);
        }
    }

    public void SaveInventory(Save save) {
        save.equippedWeapon = new SerializablePickableSO(equippedWeapon);
        save.equippedArmor = new SerializablePickableSO(equippedArmor);
        // Save consumables
        List<SerializableConsumableSO> serializableConsumables = new List<SerializableConsumableSO>();
        foreach (ConsumableSO con in inventoryConsumables) {
            SerializableConsumableSO serialized = new SerializableConsumableSO(con);
            serializableConsumables.Add(serialized);
        }
        save.inventoryConsumables = serializableConsumables;
    }

    public void AddConsumable(ConsumableSO consumable) {
        // If the consumable is already in inventory, add +1 to quantity and return
        foreach (ConsumableSO item in inventoryConsumables) {
            // Check using different equals methods based on ConsumableType
            if (item.EqualsConsumable(consumable)) {
                item.quantity++;
                return;
            }
        }

        // Else add new item
        consumable.quantity = 1;
        inventoryConsumables.Add(consumable);
    }

    public List<ConsumableSO> GetConsumables() {
        // Sort list by name
        var sortedList = inventoryConsumables.OrderBy(go => go.name).ToList();

        return sortedList;
    }

    public void UseConsumable(ConsumableSO consumable) {
        bool removeItem = true;
        // Use items (Might have to be moved to somewhere else later)
        switch (consumable.consumableType) {
            case ConsumableType.Battery:
            case ConsumableType.ComsatLink:
            case ConsumableType.Rig:
                Player player = PlayerStats.Instance.player;
                player.UseConsumable(consumable);
                break;
            case ConsumableType.Toy:
                PlayerStats.Instance.UseToy(consumable);
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
                ItemSelectorCanvas ic = CanvasMaster.Instance.itemSelectorCanvas;
                ic.OpenIdentifyCanvas(consumable);

                // If there are no identifiable items, close the canvas
                if (ic.isEmpty) {
                    CanvasMaster.Instance.topInfoCanvas.ShowIdentifiableEmpty();
                    ic.CloseItemSelectorCanvas();
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
            inventoryConsumables.Remove(consumable);
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
