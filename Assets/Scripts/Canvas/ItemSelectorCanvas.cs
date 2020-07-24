using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemSelectorCanvas : MonoBehaviour {

    [SerializeField]
    private ConsumablesScrollSystem consumablesScrollSystem;

    [SerializeField]
    private TextMeshProUGUI headerText;

    private const string IDENTIFY_HEADER = "Select which item to identify:";
    private const string STORAGE_HEADER = "Select Comsat Link to use:";

    // Identify items
    private ConsumableSO usedScanner;

    // Send items to storage
    private PickableSO itemToSend;

    private GameState previousGameState;
    public bool isEmpty { get; private set; }

    private List<ConsumableSO> OpenThisCanvas() {
        gameObject.SetActive(true);
        previousGameState = GameMaster.Instance.gameState;
        GameMaster.Instance.SetState(GameState.ItemSelector);

        // Clear old items from the scroll system
        consumablesScrollSystem.ClearAllItems();

        // Load all consumables from inventory
        return Inventory.Instance.GetConsumables();
    }

    public void OpenIdentifyCanvas(ConsumableSO usedScanner) {
        List<ConsumableSO> consumables = OpenThisCanvas();
        this.usedScanner = usedScanner;
        headerText.text = IDENTIFY_HEADER;

        // Get only items which are identifiable
        List<ConsumableSO> identifiableItems = new List<ConsumableSO>();
        foreach (ConsumableSO con in consumables) {
            if (con.consumableType.Equals(ConsumableType.Battery)) {
                if (con.batteryType.Equals(ConsumableSO.BatteryType.Unknown)) {
                    identifiableItems.Add(con);
                }
            }
        }

        // Add items to the scroll system and listen for their clicks
        foreach (ConsumableSO con in identifiableItems) {
            consumablesScrollSystem.AddItem(con)
                .onClick.AddListener(() => IdentifyItem(con));
        }

        isEmpty = identifiableItems.Count == 0;
    }

    public void OpenSendWeaponOrArmorToStorageCanvas(PickableSO itemToSend) {
        List<ConsumableSO> consumables = OpenThisCanvas();
        this.itemToSend = itemToSend;
        headerText.text = STORAGE_HEADER;

        // Get only items which are identifiable
        List<ConsumableSO> comsatLinks = new List<ConsumableSO>();
        foreach (ConsumableSO con in consumables) {
            if (con.consumableType.Equals(ConsumableType.ComsatLink)) {
                comsatLinks.Add(con);
            }
        }

        // Add items to the scroll system and listen for their clicks
        foreach (ConsumableSO con in comsatLinks) {
            consumablesScrollSystem.AddItem(con)
                .onClick.AddListener(() => SendItemToStorage(con));
        }

        isEmpty = comsatLinks.Count == 0;
    }

    private void IdentifyItem(ConsumableSO item) {
        Inventory inv = Inventory.Instance;

        if (usedScanner.CheckIfUsageSuccessful()) {
            // Remove the item from the inventory
            inv.RemoveConsumable(item);

            // Change battery type and add it as a new item
            item.DetermineFinalBatteryType();
            inv.AddConsumable(item);

            // Show info about battery change
            CanvasMaster.Instance.topInfoCanvas.ShowBatteryIdentified(item.batteryType.ToString());
        }

        // Remove the scanner from inventory and close the canvas
        inv.RemoveConsumable(usedScanner);
        CloseItemSelectorCanvas();
    }

    private void SendItemToStorage(ConsumableSO comsatLink) {
        Inventory inv = Inventory.Instance;
        CanvasMaster cv = CanvasMaster.Instance;

        if (comsatLink.CheckIfUsageSuccessful()) {
            // Remove the item from the chest
            cv.chestCanvas.RemoveItemFromChest();

            // Add item to the storage
            Storage.Instance.AddToStorage(itemToSend);

            // Show info about sending the item
            cv.topInfoCanvas.ShowItemSentToStorage(itemToSend.name);
        }

        // Remove the comsat link from inventory and close the canvas
        inv.RemoveConsumable(comsatLink);
        CloseItemSelectorCanvas();
    }

    public void CloseItemSelectorCanvas() {
        GameMaster.Instance.SetState(previousGameState);
        gameObject.SetActive(false);

        Inventory.Instance.RefreshInventoryItems();
    }
}
