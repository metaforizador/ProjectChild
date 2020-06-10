using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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

    private Save CreateSaveGameObject() {
        Save save = new Save();
        PlayerStats.Instance.SavePlayerStats(save);
<<<<<<< HEAD
=======
        CanvasMaster.Instance.SaveCanvasValues(save);
>>>>>>> toni

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
            PlayerStats.Instance.LoadPlayerStats(save);
<<<<<<< HEAD
=======
            CanvasMaster.Instance.LoadCanvasValues(save);
>>>>>>> toni
            file.Close();

            Debug.Log("Game Loaded");
        } else {
            Debug.Log("No game saved!");
        }
    }

    public void QuitGame() {
        if (Application.isEditor) {
            // Helps when testing in editor
<<<<<<< HEAD
            //UnityEditor.EditorApplication.ExitPlaymode();
=======
            UnityEditor.EditorApplication.ExitPlaymode();
>>>>>>> toni
        } else {
            Application.Quit();
        }
    }
}
