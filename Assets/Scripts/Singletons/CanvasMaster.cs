using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasMaster : MonoBehaviour {
    // Make class static and destroy if script already exists
    private static CanvasMaster _instance; // **<- reference link to the class
    public static CanvasMaster Instance { get { return _instance; } }

    private void Awake() {
        // If instance not yet created
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private GameMaster gm;

    public GameObject canvasBackground, crosshair;
    public GameObject dialogueCanvas, statsCanvas,
        HUDCanvas, inventoryCanvas,
        gameOverCanvas, mainMenuCanvas;
    public ChestCanvas chestCanvas;
    public TopInfoCanvas topInfoCanvas;
    public HotbarCanvas hotbarCanvas;
    public ItemSelectorCanvas itemSelectorCanvas;
    public CanvasSounds canvasSounds;
    public UIAnimator uiAnimator;

    // Store questions and replies so they can be looped through
    public Dictionary<Mood, List<string>> askedQuestions { get; private set; }
    public Dictionary<WordsType, List<string>> givenReplies { get; private set; }

    void Start() {
        // Retrieve GameMaster instance
        gm = GameMaster.Instance;
        
        // Enable canvases when game starts to fix fps hiccups when opening them
        dialogueCanvas.GetComponent<DialogueScript>().Initialize();
        topInfoCanvas.Initialize();
        hotbarCanvas.Initialize();  // Fixes bugs when loading a save

        // Disable some canvases in case they are left open
        canvasBackground.SetActive(false);
        itemSelectorCanvas.gameObject.SetActive(false);
        HUDCanvas.SetActive(false);

        // Enable main menu canvas if game starts at main menu
        mainMenuCanvas.SetActive(SceneManager.GetActiveScene().name.Equals("MainMenu") ? true : false);

        // Initialize saved questions and replies
        askedQuestions = new Dictionary<Mood, List<string>>();
        givenReplies = new Dictionary<WordsType, List<string>>();
    }

    void Update() {
        bool inputEnabled = gm.gameState.Equals(GameState.Movement) || gm.gameState.Equals(GameState.Menu);

        // Toggle menu
        if (Input.GetButtonDown("Menu") && inputEnabled) {
            inventoryCanvas.GetComponent<InventoryCanvas>().ToggleMenu();
        }
    }

    public void ShowCanvasBackround(bool show) {
        canvasBackground.SetActive(show);
    }

    public void ShowHUDCanvas(bool show) {
        HUDCanvas.SetActive(show);
    }

    public void ShowCrosshair(bool show) {
        crosshair.SetActive(show);
    }

    public void SaveCanvasValues(Save save) {
        save.askedQuestions = askedQuestions;
        save.givenReplies = givenReplies;
        hotbarCanvas.SaveHotbar(save);
    }

    public void LoadCanvasValues(Save save) {
        askedQuestions = save.askedQuestions;
        givenReplies = save.givenReplies;
        hotbarCanvas.LoadHotbar(save);
    }

    public void OpenDialogue() {
        dialogueCanvas.SetActive(true);
    }

    public void ShowStats() {
        statsCanvas.SetActive(!statsCanvas.activeSelf);
    }

    public void ShowGameOverCanvas(bool show) {
        gameOverCanvas.SetActive(show);
        GameMaster.Instance.SetState(GameState.Dead);
    }
}
