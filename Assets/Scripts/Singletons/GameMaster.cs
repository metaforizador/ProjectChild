using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public enum GameState { Movement, Menu, Dialogue, Chest, Hotbar, ItemSelector, Dead };

public class GameMaster : MonoBehaviour {
    
    // Make class static and destroy if script already exists
    private static GameMaster _instance; // **<- reference link to the class
    public static GameMaster Instance { get { return _instance; } }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private CanvasMaster cm;

    // Handle game state
    public GameState gameState { get; private set; }
    public void SetState(GameState state) {
        gameState = state;

        // Show crosshair if state is movement
        cm.ShowCrosshair(gameState.Equals(GameState.Movement) ? true : false);

        // Pause time if gamestate is not movement
        Time.timeScale = gameState.Equals(GameState.Movement) ? 1 : 0;

        // Hide cursor only when the state is movement
        ShowCursor(gameState.Equals(GameState.Movement) ? false : true);

        switch (gameState) {
            case GameState.Movement:
                cm.ShowCanvasBackround(false);
                cm.ShowHUDCanvas(true);
                break;
            case GameState.Menu:
            case GameState.Hotbar:
                cm.ShowCanvasBackround(true);
                cm.ShowHUDCanvas(true);
                break;
            case GameState.Dialogue:
            case GameState.Chest:
            case GameState.Dead:
                cm.ShowCanvasBackround(true);
                cm.ShowHUDCanvas(false);
                break;
            case GameState.ItemSelector:
                cm.ShowCanvasBackround(true);
                break;
        }
    }

    // Handle cursor
    private bool cursorVisible;
    private void ShowCursor(bool show) {
        cursorVisible = show;
        Cursor.visible = cursorVisible;

        if (cursorVisible)
            Cursor.lockState = CursorLockMode.Confined;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    // Storage chest
    private int unlockedStorageChestsAmount = 1;


    void Start() {
        cm = CanvasMaster.Instance;
    }

    private Save CreateSaveGameObject() {
        Save save = new Save();
        PlayerStats.Instance.SavePlayerStats(save);
        CanvasMaster.Instance.SaveCanvasValues(save);
        Inventory.Instance.SaveInventory(save);

        return save;
    }

    public void SaveGame() {
        Save save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public void LoadGame() {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);

            // Load different class values
            PlayerStats.Instance.LoadPlayerStats(save);
            CanvasMaster.Instance.LoadCanvasValues(save);
            Inventory.Instance.LoadInventory(save);

            file.Close();

            Debug.Log("Game Loaded");
        } else {
            Debug.Log("No game saved!");
        }
    }

    // For testing purposes
    public void Restart() {
        SceneManager.LoadScene(0);
        CanvasMaster.Instance.ShowGameOverCanvas(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
