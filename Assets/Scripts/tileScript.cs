using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileScript : MonoBehaviour
{
    Transform tf;
    GameObject gm;
    public int xcord;
    public int ycord;
    private GameObject camera;
    public bool activated = false;

    // Add a public field for the prefab to spawn
    private GameObject prefabToSpawn;
    private TowerData towerToPlace;

    private List<TowerData> placedTowers = new List<TowerData>();

    // Start is called before the first frame update
    void Start()
    {
        gm = this.gameObject;
        tf =  gm.GetComponent<Transform>();
        //cords set manually for level 1 will be done programatically later
        camera = GameObject.Find("Main Camera");
        camera.GetComponent<stageManager>().updateTileList(xcord, ycord, gm);

        //prefabToSpawn = GameObject.Find("EventSystem").GetComponent<clickToSpawnManager>().meleeTower;
    }
    
    void OnMouseDown()
    {
        //do something with the game object after clicking on it
        print("name: " + gm.name + " xcord: " + xcord + " ycord: " + ycord);

        // Spawn the prefab on this tile
        towerToPlace.towerType = "melee";
        towerToPlace.xPos = xcord;
        towerToPlace.yPos = ycord;  
        BuildTowerFromData(towerToPlace);
    }

    public void BuildTowerFromData(TowerData data)
    {
        // Spawn the prefab on this tile
        if (prefabToSpawn != null)
        {
            // Calculate the spawn position (tile position + 1 unit up)
            Vector3 spawnPosition = tf.position + Vector3.up;

            // Instantiate the prefab at the calculated position
            if(data.towerType == "melee"){
                prefabToSpawn = GameObject.Find("EventSystem").GetComponent<clickToSpawnManager>().meleeTower;
            }
            else{
                print("I don't think we have any other tower types other than melee");
            }
            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            // Add the spawned tower data to the list
            TowerData newTower = new TowerData
            {
                towerType = prefabToSpawn.name, // Assuming the prefab name represents the tower type
                xPos = xcord,
                yPos = ycord
            };
            placedTowers.Add(newTower);
        }
        else
        {
            Debug.LogWarning("No prefab assigned to spawn on tile: " + gm.name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyAI enteringEnemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enteringEnemy != null)
            enteringEnemy.PrimeTile(this);
    }

    private void OnCollisionExit(Collision collision)
    {
        EnemyAI enteringEnemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enteringEnemy != null)
            enteringEnemy.AcceptChange();
    }

    public void activate() 
    {
        activated = true;
    }
    
    public TowerData[] SerializeTowers()
    {
        return placedTowers.ToArray();
    }
}
