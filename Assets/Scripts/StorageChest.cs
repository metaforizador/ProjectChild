using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageChest : MonoBehaviour, Chests {

    [SerializeField]
    [Header("Current values: 1 - 6")]
    private int storageSlot;

    private PickableSO[] items;
    private int itemAmount = 1;

    void Start() {
        // Create item array so ChangeItems works correctly and there's a possibility
        // to modify the amount later easily
        items = new PickableSO[itemAmount];

        // Destroy chest if player has not unlocked enough slots
        if (Storage.Instance.GetUnlockedStorageSlotsAmount() < storageSlot) {
            Destroy(gameObject);
        }
    }

    public void OpenChest() {
        // Retrieve item from storage, this could be moved to Start(), but for
        // testing purposes it's simpler if it's here
        items[0] = Storage.Instance.GetFromStorageSlot(storageSlot);
        CanvasMaster.Instance.chestCanvas.ShowChest(this, items);
    }

    public void ChangeItems(PickableSO[] items) {
        this.items = items;
    }
}
