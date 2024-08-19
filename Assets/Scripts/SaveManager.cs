using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Playables;
using JetBrains.Annotations;

[System.Serializable]
public class TowerData
{
    public string type;
    public int xPos;
    public int yPos;

    public string[] upgrades;
}



[System.Serializable]
public class SaveData
{
    public int level;
    public int stage;
    public int currency;
    public int health;
    public int wave;
    public TowerData[] towers;
}



public class SaveManager : MonoBehaviour
{
    public GameObject StageManagerObject;
    
    private stageManager StageManager;
    private SaveData data = new SaveData();

    public static bool loadingFromSave;
    public static bool loadingFromAutoSave;

    string saveFile, autoSaveFile;

    private void Awake()
    {
        saveFile = Application.persistentDataPath + "/SaveData.json";
        autoSaveFile = Application.persistentDataPath + "/AutoSave.json";
    }

    private void Start()
    {
        StageManager = StageManagerObject.GetComponent<stageManager>();
        if (loadingFromSave)
        {
            LoadFromSave(loadingFromAutoSave);
        }
    }


    public bool SaveExists(bool autosave = false)
    {
        return File.Exists(autosave ? autoSaveFile : saveFile);
    }

    public void LoadFromSave(bool autosave = false)
    {
        loadingFromSave = false;
        loadingFromAutoSave = false;
        if (!SaveExists(autosave)) return;
        
        string fileContents = File.ReadAllText(autosave ? autoSaveFile : saveFile);

        data = JsonUtility.FromJson<SaveData>(fileContents);
        if (data == null)
        {
            Debug.LogError("Failed to load save data");
            return;
        }

        Debug.Log("save loaded!");
        Debug.Log("level: " + data.level);
        Debug.Log("stage: " + data.stage);

        StageManager.loadLevel(data.level, data.stage);
        clickToSpawnManager.placedTowers.Clear();
        WaveManager.CurrentWave = data.wave;
        GameplayManager.main.currPlayerHealth = data.health;
        GameplayManager.main.resourcePoints = data.currency;
        HUDManager.main.UpdateEXP();
        HUDManager.main.UpdateHealth();

        if (data.towers != null)
        {
            for (int i = 0; i < data.towers.Length; i++)
            {
                StageManager.getTileFromCords(data.towers[i].xPos, data.towers[i].yPos).GetComponent<tileScript>().BuildTowerFromData(data.towers[i]);
            }
        }
    }

    //will be called after stage/level to save the current game state
    public void SaveGame(bool autosave = false)
    {
        Debug.Log("saving game to " + (autosave ? autoSaveFile : saveFile));

        data.level = stageManager.level;
        data.stage = stageManager.stage;
        data.wave = WaveManager.CurrentWave;
        data.currency = GameplayManager.main.resourcePoints;
        data.health = GameplayManager.main.currPlayerHealth;
        data.towers = SerializeTowers();

        string jsonString = JsonUtility.ToJson(data);

        File.WriteAllText(autosave ? autoSaveFile : saveFile, jsonString);
    }

    public TowerData[] SerializeTowers()
    {
        return clickToSpawnManager.placedTowers.ToArray();
    }
}
