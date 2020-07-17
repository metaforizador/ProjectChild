using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
