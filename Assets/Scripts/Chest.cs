using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    private enum Rarity { Common, Uncommon, Rare, Epic }

    private PickableSO[] items;

    void Start() {
        int itemAmount = 0;
        Rarity rarity = (Rarity)Rarity.ToObject(typeof(Rarity), Random.Range(0, System.Enum.GetValues(typeof(Rarity)).Length));

        switch (rarity) {
            case Rarity.Common:
                itemAmount = 1;
                break;
            case Rarity.Uncommon:
                itemAmount = 2;
                break;
            case Rarity.Rare:
                itemAmount = 3;
                break;
            case Rarity.Epic:
                itemAmount = 4;
                break;
        }

        items = new PickableSO[0];
    }

    public void OpenChest() {
        ChestCanvas cc = CanvasMaster.Instance.chestCanvas.GetComponent<ChestCanvas>();
        cc.ShowChest(items);
    }
}
