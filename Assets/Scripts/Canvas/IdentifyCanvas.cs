using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyCanvas : MonoBehaviour {

    [SerializeField]
    private ConsumablesScrollSystem consumablesScrollSystem;

    private ConsumableSO usedScanner;
    private GameState previousGameState;
    public bool isEmpty { get; private set; }

    public void OpenIdentifyCanvas(ConsumableSO usedScanner) {
        gameObject.SetActive(true);
        this.usedScanner = usedScanner;
        previousGameState = GameMaster.Instance.gameState;
        GameMaster.Instance.SetState(GameState.Identify);

        // Load all consumables from inventory
        List<ConsumableSO> consumables = Inventory.Instance.GetConsumables();

        // Get only items which are identifiable
        List<ConsumableSO> identifiableItems = new List<ConsumableSO>();
        foreach (ConsumableSO con in consumables) {
            if (con.consumableType.Equals(ConsumableType.Battery)) {
                if (con.batteryType.Equals(ConsumableSO.BatteryType.Unknown)) {
                    identifiableItems.Add(con);
                }
            }
        }

        // Clear old items from the scroll system
        consumablesScrollSystem.ClearAllItems();

        // Add items to the scroll system and listen for their clicks
        foreach (ConsumableSO con in identifiableItems) {
            consumablesScrollSystem.AddItem(con)
                .onClick.AddListener(() => IdentifyItem(con));
        }

        isEmpty = identifiableItems.Count == 0;
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
        CloseIdenfityCanvas();
    }

    public void CloseIdenfityCanvas() {
        GameMaster.Instance.SetState(previousGameState);
        gameObject.SetActive(false);

        Inventory.Instance.RefreshInventoryItems();
    }
}
