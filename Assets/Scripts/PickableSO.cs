using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Condition { Volatile, Damaged, Intact, Supercharged }

public class PickableSO : ScriptableObject {

    public Sprite sprite;
    public Condition condition;
}
