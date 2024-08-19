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


    public TowerData[] towers;
}



public class SaveManager : MonoBehaviour
{

    public GameObject StageManagerObject;
    
    private stageManager StageManager;
    private SaveData data = new SaveData();


    string saveFile;

    private void Awake()
    {
        saveFile = Application.persistentDataPath + "SaveData.json";
    }

    private void Start()
    {
        StageManager = StageManagerObject.GetComponent<stageManager>();
    }


    public bool SaveExists()
    {
        return (File.Exists(saveFile));
    }

    public void LoadFromSave()
    {
        string fileContents = File.ReadAllText(saveFile);

        data = JsonUtility.FromJson<SaveData>(fileContents);

        Debug.Log("save loaded!");
        //Debug.Log("level: " + data.level);
        //Debug.Log("stage: " + data.stage);

        //TODO: implement loading specific level and stage in stageManager class
        //StageManager.LoadLevel(data.level, data.stage);

        //TODO: use data from loaded data to place towers
        if (data.towers != null)
        {
            for (int i = 0; i < data.towers.Length; i++)
            {
                //TowerManager.BuildTowerFromData(data.towers[i]);
                Debug.Log(data.towers[i].type);
            }
        }
        
    }


    //will be called after stage/level to save the current game state
    public void SaveGame()
    {

        Debug.Log("saving game!");

        data.level = stageManager.level;
        data.stage = StageManager.stage;



        //TODO: implement a method on TowerManager to serialize all placed towers
        //data.towers = TowerManager.SerializeTowers();

        TowerData tower1 = new TowerData();
        tower1.type = "basic robot";

        TowerData tower2 = new TowerData();
        tower2.type = "cooler robot";

        TowerData[] towers = { tower1, tower2 };
        data.towers = towers;

        string jsonString = JsonUtility.ToJson(data);

        File.WriteAllText(saveFile, jsonString);
    }
}
