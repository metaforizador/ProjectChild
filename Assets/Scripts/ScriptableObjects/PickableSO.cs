using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Use this to save and load PickableSO items.
/// 
/// ConsumableSO extends this to determine the name and itemtype of the
/// PickableSO. Weapon and armor rely solely on this, since they only
/// need to save the name and itemType.
/// </summary>
[System.Serializable]
public class SerializablePickableSO {
    public string name;
    public int itemType;

    public const int WEAPON_TYPE = 0, ARMOR_TYPE = 1, CONSUMABLE_TYPE = 2;

    public SerializablePickableSO(PickableSO item) {
        name = item.name;
        if (item is WeaponSO) {
            itemType = WEAPON_TYPE;
        } else if (item is ArmorSO) {
            itemType = ARMOR_TYPE;
        } else if (item is ConsumableSO) {
            itemType = CONSUMABLE_TYPE;
        }
    }
}

public enum Condition { Volatile, Damaged, Intact, Supercharged }

public class PickableSO : ScriptableObject {

    public new string name;
    public Sprite sprite;
    public Condition condition;

    // Use item's name as id
    public bool EqualsItem(PickableSO otherItem) {
        return this.name.Equals(otherItem.name);
    }
}
