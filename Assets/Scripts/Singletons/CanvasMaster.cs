using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMaster : MonoBehaviour {
    // Make class static and destroy if script already exists
    private static CanvasMaster _instance; // **<- reference link to the class
    public static CanvasMaster Instance { get { return _instance; } }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public GameObject dialogueCanvas;

    void Start() {
        // Enable canvases when game starts to fix fps hiccups when opening them
        dialogueCanvas.SetActive(true);
    }

    public void OpenDialogue() {
        dialogueCanvas.GetComponent<DialogueScript>().ShowDialogue();
    }

    public void ShowStatGain(string gainedStats) {

    }
}
