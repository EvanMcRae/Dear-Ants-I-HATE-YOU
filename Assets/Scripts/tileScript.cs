using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class tileScript : MonoBehaviour
{
    public int xcord;
    public int ycord;
    public bool activated = false;

    // Add a public field for the prefab to spawn
    private GameObject prefabToSpawn;
    private TowerData towerToPlace;

    public bool hasTower = false;
    public bool canPlaceTower = false;

    //reference to tower on top of block
    GameObject spawnedtower;

    // Start is called before the first frame update
    void Start()
    {
        //cords set manually for level 1 will be done programatically later
        if (Camera.main.GetComponent<stageManager>().getLevel() != 1) 
        {
            xcord = (int)this.transform.position.x;
            ycord = (int)this.transform.position.z;
        }
        Camera.main.GetComponent<stageManager>().updateTileList(xcord, ycord, gameObject);

        //prefabToSpawn = GameObject.Find("EventSystem").GetComponent<clickToSpawnManager>().meleeTower;
        if((this.tag == "pathable" || this.tag == "highground") && this.name != "base" && this.name != "enemySpawn"){
            canPlaceTower = true;
        }
        else{
            canPlaceTower = false;
        }
    }
    
    void OnMouseDown()
    {
        // Reject tower placement if game is not playable at the moment
        // TODO add condition for start cutscene
        if (GameplayManager.paused || GameplayManager.lost || GameplayManager.won || !ScreenWipe.over)
            return;

        //do something with the game object after clicking on it
        print("name: " + gameObject.name + " xcord: " + xcord + " ycord: " + ycord);

        // Spawn the prefab on this tile
        if (hasTower == false && canPlaceTower == true)
        {
            TowerData towerToPlace = new()
            {
                type = clickToSpawnManager.playerTowerChoice,
                xPos = xcord,
                yPos = ycord
            };
            BuildTowerFromData(towerToPlace);
        }
    }

    public void BuildTowerFromData(TowerData data)
    {
        // Instantiate the prefab at the calculated position
        if (data.type == "melee")
        {
            prefabToSpawn = GameplayManager.main.spawnManager.meleeTower;
        }
        else if (data.type == "laser")
        {
            prefabToSpawn = GameplayManager.main.spawnManager.laserTower;
        }
        else if (data.type == "bomb")
        {
            prefabToSpawn = GameplayManager.main.spawnManager.bombTower;
        }
        else
        {
            print("I don't think we have that type");
            hasTower = false;
        }
        
        // Spawn the prefab on this tile
        if (prefabToSpawn != null)
        {
            // Calculate the spawn position (tile position + 1 unit up)
            Vector3 spawnPosition = transform.position + Vector3.up;

            spawnedtower = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            clickToSpawnManager.placedTowers.Add(data);
            hasTower = true;
        }
        else
        {
            Debug.LogWarning("No prefab assigned to spawn on tile: " + gameObject.name);
        }
    }

    public void activate() 
    {
        activated = true;
    }
    
    public TowerData[] SerializeTowers()
    {
        //return placedTowers.ToArray();
        return clickToSpawnManager.placedTowers.ToArray();
    }
}
