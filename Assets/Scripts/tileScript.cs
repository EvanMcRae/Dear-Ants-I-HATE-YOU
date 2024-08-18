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

    // Add a public field for the prefab to spawn
    private GameObject prefabToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        gm = this.gameObject;
        tf =  gm.GetComponent<Transform>();
        //cords set manually for level 1 will be done programatically later
        camera = GameObject.Find("Main Camera");
        camera.GetComponent<stageManager>().updateTileList(xcord, ycord, gm);

        prefabToSpawn = GameObject.Find("EventSystem").GetComponent<clickToSpawnManager>().meleeTower;
    }

    void OnMouseDown()
    {
        //do something with the game object after clicking on it
        print("name: " + gm.name + " xcord: " + xcord + " ycord: " + ycord);

        // Spawn the prefab on this tile
        if (prefabToSpawn != null)
        {
            // Calculate the spawn position (tile position + 1 unit up)
            Vector3 spawnPosition = tf.position + Vector3.up;

            // Instantiate the prefab at the calculated position
            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No prefab assigned to spawn on tile: " + gm.name);
        }
    }
}
