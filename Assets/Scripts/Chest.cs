using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    private enum Rarity { Damaged, Intact }

    private PickableSO[] items;
    private int maxItems = 4;

    private const int INTACT_VALUE = 50;

    void Start() {
        // Randomize chest contents
        int itemAmount = Random.Range(1, maxItems + 1); // + 1 because max is not inclusive

        // Randomize rarity
        Rarity rarity;
        float number = Random.Range(1, 101);
        number += PlayerStats.Instance.rareItemFindRate.currentValue;

        if (number >= INTACT_VALUE) {
            rarity = Rarity.Intact;
        } else {
            rarity = Rarity.Damaged;
        }

        string rarityPath = rarity.ToString();

        // Load all pickable items from resources which have correct rarity
        PickableSO[] pickArray = Resources.LoadAll<PickableSO>("ScriptableObjects/PickableItems/" + rarityPath + "/");

        // Convert array to list
        List<PickableSO> pickList = new List<PickableSO>(pickArray);

        // Create new items array based on random size
        items = new PickableSO[itemAmount];

        // Add random items to the chest items array
        for (int i = 0; i < itemAmount; i++) {
            int randomIndex = Random.Range(0, pickList.Count);
            PickableSO pickable = pickList[randomIndex];
            items[i] = pickable;
            pickList.Remove(pickable);
        }
    }

    public void OpenChest() {
        ChestCanvas cc = CanvasMaster.Instance.chestCanvas.GetComponent<ChestCanvas>();
        cc.ShowChest(this, items);
    }

    public void ChangeItems(PickableSO[] items) {
        this.items = items;
    }
}
