using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public GameObject dialogueCanvas, statGainCanvas, statsCanvas, testCanvas;

    public Image hpBar, shieldBar, staminaBar;
    public TextMeshProUGUI ammoText;

    // Store questions and replies so they can be looped through
    public Dictionary<Mood, List<string>> askedQuestions { get; private set; }
    public Dictionary<WordsType, List<string>> givenReplies { get; private set; }

    void Start() {
        // Enable canvases when game starts to fix fps hiccups when opening them
        dialogueCanvas.GetComponent<DialogueScript>().Initialize();
        statGainCanvas.GetComponent<StatGainCanvas>().Initialize();

        // Initialize saved questions and replies
        askedQuestions = new Dictionary<Mood, List<string>>();
        givenReplies = new Dictionary<WordsType, List<string>>();
    }

    void Update() {
        // Open Debug menu
        if (Input.GetKeyDown(KeyCode.Escape)) {
            testCanvas.SetActive(!testCanvas.activeSelf);
        }
    }

    public void SaveCanvasValues(Save save) {
        save.askedQuestions = askedQuestions;
        save.givenReplies = givenReplies;
    }

    public void LoadCanvasValues(Save save) {
        askedQuestions = save.askedQuestions;
        givenReplies = save.givenReplies;
    }

    public void OpenDialogue() {
        dialogueCanvas.SetActive(true);
    }

    public void ShowStatGain(string gainedStats) {
        statGainCanvas.SetActive(true);
        statGainCanvas.GetComponent<StatGainCanvas>().ShowStatGain(gainedStats);
    }

    public void ShowStats() {
        statsCanvas.SetActive(!statsCanvas.activeSelf);
    }

    public void AdjustHUDBar(Image bar, float amount) {
        bar.fillAmount = amount / 100;
    }

    public void AdjustAmmoAmount(int max, float current) {
        int currentAmmo = (int) Mathf.Floor(max * (current / 100));
        ammoText.text = $"{currentAmmo}/{max}";
    }
}
