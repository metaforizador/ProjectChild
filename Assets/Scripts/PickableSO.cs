using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Condition { Damaged, Intact }

public class PickableSO : ScriptableObject {

    public new string name;
    public Condition condition;
}
