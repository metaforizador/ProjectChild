using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Answer", menuName ="Dialogue / New Answer")]
public class AnswerData : Answer { }

[System.Serializable]
public class Answer : ScriptableObject {

    public new string name;
    public string text;

}
