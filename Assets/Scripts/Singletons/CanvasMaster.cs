using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject canvasBackground, crosshair;
    public GameObject dialogueCanvas, statGainCanvas, statsCanvas,
        HUDCanvas, chestCanvas, hotbarCanvas, inventoryCanvas,
        gameOverCanvas;
    public CanvasSounds canvasSounds;
    public UIAnimator uiAnimator;

    // Store questions and replies so they can be looped through
    public Dictionary<Mood, List<string>> askedQuestions { get; private set; }
    public Dictionary<WordsType, List<string>> givenReplies { get; private set; }

    void Start() {
        // Enable canvases when game starts to fix fps hiccups when opening them
        dialogueCanvas.GetComponent<DialogueScript>().Initialize();
        statGainCanvas.GetComponent<StatGainCanvas>().Initialize();
        canvasBackground.SetActive(false);

        // Initialize saved questions and replies
        askedQuestions = new Dictionary<Mood, List<string>>();
        givenReplies = new Dictionary<WordsType, List<string>>();
    }

    void Update() {
        // Toggle menu
        if (Input.GetButtonDown("Menu")) {
            inventoryCanvas.GetComponent<InventoryCanvas>().ToggleMenu();
        }
    }

    public void ShowCanvasBackround(bool show) {
        canvasBackground.SetActive(show);
        // Toggle crosshair visibility
        crosshair.SetActive(!show);
        // Show / hide cursor
        GameMaster.Instance.ShowCursor(show);

        // Pause / Unpause time
        Time.timeScale = show ? 0 : 1;
    }

    public void ShowHUDCanvas(bool show) {
        HUDCanvas.SetActive(show);
    }

    public void SaveCanvasValues(Save save) {
        save.askedQuestions = askedQuestions;
        save.givenReplies = givenReplies;
        hotbarCanvas.GetComponent<HotbarCanvas>().SaveHotbar(save);
    }

    public void LoadCanvasValues(Save save) {
        askedQuestions = save.askedQuestions;
        givenReplies = save.givenReplies;
        hotbarCanvas.GetComponent<HotbarCanvas>().LoadHotbar(save);
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

    public void ShowGameOverCanvas(bool show) {
        gameOverCanvas.SetActive(show);
        GameMaster.Instance.ShowCursor(show);
    }
}
