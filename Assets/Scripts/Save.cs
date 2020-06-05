using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Save {
    // Player stats
    public int maxHp, maxShield, level, xp, shieldRegen, armor, resistance, attackSpd, fireRate, dodge, critical, movementSpd;

    private Save CreateSaveGameObject() {
        Save save = new Save();
        PlayerStats.Instance.SavePlayerStats(save);

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
            file.Close();

            Debug.Log("Game Loaded");
        } else {
            Debug.Log("No game saved!");
        }
    }
}
