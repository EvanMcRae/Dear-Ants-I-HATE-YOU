using System.Collections.Generic;
using UnityEngine;

public class stageManager : MonoBehaviour
{
    /*
     * first object in list is enemy spawn
     * last object in list is base
     * use for pathfinding if you so desire
     * objects in path1 are not in path2 and so on, base is last index in every path
     */

    public GameObject home;
    public List<List<GameObject>> path = new List<List<GameObject>>();
    public List<GameObject> allPathsInOne;

    public List<GameObject> everyTile;
    public List<GameObject> everyTileOrdered;

    public int stage;
    public int stageCount;
    public int level;
    public List<GameObject> levelAssets;

    public static stageManager main;
    public WaveManager waveManager;
    public List<Vector3> SpawnLocations = new List<Vector3>();

    void Awake()
    {
        main = this;
        Debug.Log("Stage manager: " + gameObject);

        GameObject levels = GameObject.Find("Levels");
        levelAssets = new List<GameObject>();
        foreach (Transform level in levels.transform) 
        {
            levelAssets.Add(level.gameObject);
        }

        loadLevel(1,1);
    }

    public void loadLevel(int level, int goToStage) 
    {
        this.level = level;
        this.stage = 1;
        stageCount = getStageCount(level);
        levelAssets[level-1].SetActive(true);
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
        advanceToStage(goToStage);
    }

    public void advanceStage()
    {
        stage++;
        enablePath(stage);
        if (stage == stageCount + 1) { endLevel(); }
    }

    private void enablePath(int stage)
    {
        foreach (GameObject gm in path[stage - 1])
        {
            gm.GetComponent<MeshRenderer>().material.color = Color.gray;
            gm.GetComponent<tileScript>().activate();
        }

        if (stage == 1)
        {
            GameObject.Find("base").GetComponent<tileScript>().activate();
            GameObject.Find("base").GetComponent<MeshRenderer>().material.color = Color.gray;
        }
    }

    public void advanceToStage(int goTo)
    {
        create2DPathList();
        enablePath(1);
        for (int i = 0; i < goTo - 1; i++)
        {
            advanceStage();
        }
    }

    private void Start()
    {
        UpdateEnemySpawnLocations();
    }

    public void UpdateEnemySpawnLocations()
    {
        /*CHANGED*/
        List<List<GameObject>> paths = new List<List<GameObject>>();
        foreach (List<GameObject> ls in path) 
        {
            paths.Add(ls);
        }

        foreach (List<GameObject> path in paths)
        {
            if (path.Count > 0)
                SpawnLocations.Add(path[0].transform.position + Vector3.up * 2);
        }
    }

    private void create2DPathList() 
    {
        GameObject pathGrandParent = GameObject.Find("pathGrandParent");

        foreach (Transform child in pathGrandParent.transform)
        {
            List<GameObject> temp = new List<GameObject>();
            foreach (Transform grandChild in child.transform)
            {
                temp.Add(grandChild.gameObject);
                allPathsInOne.Add(grandChild.gameObject);
            }
            path.Add(temp);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            advanceStage();
        }
    }

    public void updateTileList(int x, int y, GameObject toBeInserted)
    {
        int index = (y * getLength(level)) + x - getLength(level) - 1;
        everyTileOrdered[index] = toBeInserted;
    }

    public int getLevel() 
    {
        return 1;
    }

    public int getLength(int level)
    {
        /*UPDATED LATER WITH MORE LEVELS*/
        if (level == 1) 
        {
            return 11;
        }
        return 999999999;
    }

    public int getStageCount(int level)
    {
        if (level == 1) { return 5; }
        else if (level == 2) { return 5; }
        return 999999;
    }

    private int getTileCount(int level) 
    {
        /*UPDATED LATER WITH MORE LEVELS*/
        if (level == 1)
        {
            return 77;
        }
        return 999999999;
    }

    public GameObject getTileFromCords(int x, int y) 
    {
        return everyTileOrdered[(y * getLength(level)) + x - getLength(level) - 1];
    }

    public List<GameObject> getTilesOrdered() 
    {
        return everyTileOrdered;
    }

    private void endLevel() 
    {
        Debug.Log("Level Ended");
    }

    public tileScript GetNextTileInPath(tileScript startTile)
    {
        /*CHANGED*/
        List<List<GameObject>> paths = new List<List<GameObject>>();
        foreach (List<GameObject> ls in path)
        {
            paths.Add(ls);
        }

        foreach (List<GameObject> path in paths)
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (path[i] == startTile.gameObject)
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
