using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Condition { Damaged, Intact }

public class PickableSO : ScriptableObject {

    public new string name;
    public Sprite sprite;
    public Condition condition;
}
