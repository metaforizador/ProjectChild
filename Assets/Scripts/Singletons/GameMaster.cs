using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

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

    // Handle cursor
    public bool cursorVisible { get; private set; }
    public void ShowCursor(bool show) {
        cursorVisible = show;
        Cursor.visible = cursorVisible;

        if (cursorVisible)
            Cursor.lockState = CursorLockMode.Confined;
        else
            Cursor.lockState = CursorLockMode.Locked;
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
    }

    public void QuitGame() {
        Application.Quit();
    }
}
