using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueScript : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI question;
    
    void Start() {
        question.text = "Hellurei";
    }

    void Update() {
        
    }
}
