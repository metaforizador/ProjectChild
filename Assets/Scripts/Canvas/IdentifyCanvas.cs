using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyCanvas : MonoBehaviour {

    public GameObject identifiableItemsParent;

    void OnEnable() {
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


    }
}
