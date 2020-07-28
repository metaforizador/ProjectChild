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
        // If instance not yet created, or player goes back to the MainMenu, create new instance
        if (_instance == null || SceneManager.GetActiveScene().name.Equals("MainMenu")) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private CanvasMaster cm;

    public Save latestSave { get; private set; }

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

    void Start() {
        cm = CanvasMaster.Instance;
    }

    private Save CreateSaveGameObject() {
        Save save = new Save();
        // Save values of other classes
        PlayerStats.Instance.SavePlayerStats(save);
        CanvasMaster.Instance.SaveCanvasValues(save);
        Inventory.Instance.SaveInventory(save);
        Storage.Instance.SaveStorage(save);

        // Save Scene name
        save.sceneName = SceneManager.GetActiveScene().name;

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
            latestSave = save; // Player needs to access this when it spawns

            // Load values of other classes
            PlayerStats.Instance.LoadPlayerStats(save);
            CanvasMaster.Instance.LoadCanvasValues(save);
            Inventory.Instance.LoadInventory(save);
            Storage.Instance.LoadStorage(save);

            // Load saved scene
            SceneManager.LoadScene(save.sceneName);

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
