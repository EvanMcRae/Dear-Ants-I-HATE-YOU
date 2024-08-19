using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileScript : MonoBehaviour
{
    Transform tf;
    GameObject gm;
    public int xcord;
    public int ycord;
    public bool activated = false;

    // Add a public field for the prefab to spawn
    private GameObject prefabToSpawn;
    private TowerData towerToPlace;

    private bool hasTower = false;
    public bool canPlaceTower = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = this.gameObject;
        tf =  gm.GetComponent<Transform>();
        //cords set manually for level 1 will be done programatically later
        Camera.main.GetComponent<stageManager>().updateTileList(xcord, ycord, gm);

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
        print("name: " + gm.name + " xcord: " + xcord + " ycord: " + ycord);

        // Spawn the prefab on this tile
        if (hasTower == false && canPlaceTower == true)
        {
            TowerData towerToPlace = new TowerData();
            towerToPlace.type = clickToSpawnManager.playerTowerChoice;
            towerToPlace.xPos = xcord;
            towerToPlace.yPos = ycord;  
            BuildTowerFromData(towerToPlace);   
            hasTower = true;
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
            Vector3 spawnPosition = tf.position + Vector3.up;

            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            // Add the spawned tower data to the list
            TowerData newTower = new TowerData
            {
                type = prefabToSpawn.name, // Assuming the prefab name represents the tower type
                xPos = xcord,
                yPos = ycord
            };
            clickToSpawnManager.placedTowers.Add(newTower);
        }
        else
        {
            Debug.LogWarning("No prefab assigned to spawn on tile: " + gm.name);
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
