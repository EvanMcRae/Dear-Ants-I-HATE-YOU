using System.Collections.Generic;
using UnityEngine;

public class stageManager : MonoBehaviour
{
    /*
     * first object in list is enemy spawn
     * last object in list is base
     * use for pathfinding if you so desire
     * objects in path1 are not in path2 and so on exluding base, base is last index in every path
     * tiles in editor are names p(pathnumber).(index in respective list + 1)
     */

    public GameObject home;
    public List<GameObject> path1;
    public List<GameObject> path2;
    public List<GameObject> path3;
    public List<GameObject> path4;
    public List<GameObject> path5;

    public List<GameObject> everyTile;
    public List<GameObject> everyTileOrdered;

    public static stageManager main;

    public WaveManager waveManager;

    public List<Vector3> SpawnLocations = new List<Vector3>();

    public Transform enemyHolder;


    void Awake()
    {
        main = this;

        Debug.Log("Stage manager: " + gameObject);

        everyTile.AddRange(GameObject.FindObjectsOfType<GameObject>());
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject gm in everyTile) 
        {
            if (gm.GetComponent<tileScript>() != null) 
            {
                temp.Add(gm);
            }
        }
        everyTile = temp;
        everyTileOrdered = everyTile;

    }

    private void Start()
    {
        UpdateEnemySpawnLocations();
    }

    public void UpdateEnemySpawnLocations()
    {
        List<List<GameObject>> paths = new List<List<GameObject>>();
        paths.AddRange(new List<GameObject>[]
        {
            path1,
            path2,
            path3,
            path4,
            path5
        });

        foreach (List<GameObject> path in paths)
        {
            if (path.Count > 0)
                SpawnLocations.Add(path[0].transform.position + Vector3.up * 2);
        }
    }

    public void updateTileList(int x, int y, GameObject toBeInserted) 
    {
        int index = (y * 11) + x - 12;
        everyTileOrdered[index] = toBeInserted;
    }

    public int getLevel() 
    {
        return 1;
    }

    public int getLength(int level) 
    {
        /*UPDATED LATER WITH MORE LEVELS*/
        return 11;
    }

    private int getTileCount(int level) 
    {
        /*UPDATED LATER WITH MORE LEVELS*/
        return 77;
    }

    public GameObject getTileFromCords(int x, int y) 
    {
        return everyTileOrdered[(y*11) + x - 12];
    }

    public List<GameObject> getTilesOrdered() 
    {
        return everyTileOrdered;
    }

    public tileScript GetNextTileInPath(tileScript startTile)
    {
        List<List<GameObject>> paths = new List<List<GameObject>>();
        paths.AddRange(new List<GameObject>[]
        {
            path1,
            path2,
            path3,
            path4,
            path5
        });

        foreach(List<GameObject> path in paths)
        {
            for(int i = 0; i < path.Count; i++)
            {
                if(path[i] == startTile.gameObject)
                {
                    if (i + 1 < path.Count)
                        return path[i + 1].GetComponent<tileScript>();
                    else
                        return null;
                }
            }
        }

        return null;

    }
}
