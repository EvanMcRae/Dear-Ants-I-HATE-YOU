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

    void Awake()
    {
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

    //gets a single tile
    public GameObject getTileFromCords(int x, int y) 
    {
        return everyTileOrdered[(y*11) + x - 12];
    }

    //gets all tiles
    public List<GameObject> getTilesOrdered() 
    {
        return everyTileOrdered;
    }
}
