using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    private enum Rarity { Common, Uncommon, Rare, Epic }

    WeaponSO[] weapons;
    ArmorSO[] armors;

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
    }
}
