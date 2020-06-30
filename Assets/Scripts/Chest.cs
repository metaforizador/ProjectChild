using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    private enum Rarity { Damaged, Intact }

    private PickableSO[] items;
    private int maxItems = 4;

    void Start() {
        // Randomize chest contents
        int itemAmount = Random.Range(1, maxItems + 1); // + 1 because max is not inclusive
        Rarity rarity = (Rarity)Rarity.ToObject(typeof(Rarity), Random.Range(0, System.Enum.GetValues(typeof(Rarity)).Length));
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
        cc.ShowChest(items);
    }
}
