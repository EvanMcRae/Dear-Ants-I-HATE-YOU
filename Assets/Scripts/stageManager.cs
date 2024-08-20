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

    public int stageCount;
    public static int stage, level, levelToLoad = 1;
    public List<GameObject> levelAssets;

    public static stageManager main;
    public WaveManager waveManager;
    public List<Vector3> SpawnLocations = new List<Vector3>();

    public List<List<GameObject>> EnemyPathings = new List<List<GameObject>>();

    public List<PathsData> AIPathings = new List<PathsData>();

    public AK.Wwise.Event stageUp, levelReset;
    public List<GameObject> levelTransforms;

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

        Debug.Log("level to load = " + levelToLoad);
        loadLevel(levelToLoad, 1);
    }

    public void loadLevel(int goToLevel, int goToStage) 
    {
        level = goToLevel;
        stage = 1;
        stageCount = getStageCount(level);
        foreach (GameObject l in levelAssets)
        {
            l.SetActive(false);
        }
        levelAssets[level-1].SetActive(true);
        waveManager = levelAssets[level - 1].GetComponent<WaveManager>();
        everyTile.AddRange(FindObjectsOfType<GameObject>());
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
        transform.SetPositionAndRotation(levelTransforms[level-1].transform.position, levelTransforms[level-1].transform.rotation);
        Debug.Log("level = " + level + " stage = " + stage);
    }

    public void advanceStage()
    {
        stage++;
        if (stage > stageCount)
            endLevel();

        enablePath(stage);
        // stageUp.Post(gameObject);
        if (stage == stageCount + 1) { endLevel(); }
    }

    private void enablePath(int stage)
    {
        foreach (GameObject gm in path[stage - 1])
        {
            gm.GetComponent<MeshRenderer>().material.color = Color.gray;
            Transform tf = gm.GetComponent<Transform>();
            /*CHANGE TO ADD MATERIAL WHEN LUKE IS READY*/
            /*
            tf.position = new Vector3(tf.position.x, tf.position.y - .5f, tf.position.z);
            gm.GetComponent<tileScript>()?.activate();
            */
        }

        if (stage == 1)
        {
            GameObject.Find("base").GetComponent<tileScript>().activate();
            GameObject.Find("base").GetComponent<MeshRenderer>().material.color = Color.gray;
        }

        UpdateEnemySpawnLocations();
    }

    public void advanceToStage(int goTo)
    {
        create2DPathList();
        enablePath(1);
        WaveManager.CurrentWave = goTo-1;
        for (int i = 0; i < goTo - 1; i++)
        {
            advanceStage();
        }

        //Starts as soon as loaded
        waveManager.StartWave();
    }

    private void Start()
    {
        UpdateEnemySpawnLocations();
    }

    public void UpdateEnemySpawnLocations()
    {
        //List<List<GameObject>> paths = new List<List<GameObject>>();
        //foreach (List<GameObject> ls in path) 
        //{
        //    paths.Add(ls);
        //}
        /*CHANGED*/

        foreach (List<GameObject> path in path)
        {
            if (path.Count > 0 && path[0].gameObject.activeSelf)
                SpawnLocations.Add(path[0].transform.position + Vector3.up * 2);
        }
    }


    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            /*doesnt really work but kinda if you really spam that mfer*/
            advanceStage();
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
        if (GameplayManager.quit || GameplayManager.won || GameplayManager.lost || GameplayManager.playingAgain) return;

        AkSoundEngine.SetRTPCValue("LevelProgress", stage);
    }

    public void updateTileList(int x, int y, GameObject toBeInserted)
    {
        int index = (y * getLength(level)) + x - getLength(level) - 1;
        everyTileOrdered[index] = toBeInserted;
    }

    public int getLevel() 
    {
        return level;
    }

    public int getLength(int level)
    {
        /*UPDATED LATER WITH MORE LEVELS*/
        if (level == 1)
        {
            return 11;
        }
        if (level == 2)
        {
            return 9;
        }
        if (level == 3) 
        {
            return 10;
        }
        return 999999999;
    }

    public int getStageCount(int level)
    {
        if (level == 1) { return 5; }
        else if (level == 2) { return 5; }
        else if (level == 3) { return 3; }
        return 999999;
    }

    private int getTileCount(int level) 
    {
        /*UPDATED LATER WITH MORE LEVELS*/
        if (level == 1)
        {
            return 77;
        }
        else if (level == 2)
        {
            return 63;
        }
        else if (level == 3) 
        {
            return 50;
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

    public Path GetPathByName(string name)
    {
        int separatorIndex = name.IndexOf('/');
        string pathCollectionName = name.Substring(0, separatorIndex);
        PathsData pathGroup = null;

        foreach(PathsData group in AIPathings)
        {
            if(group.folderName == pathCollectionName)
            {
                pathGroup = group;
                break;
            }
        }

        if(pathGroup == null)
        {
            Debug.LogError("No path group with name: " + pathCollectionName);
            return null;
        }

        foreach(Path path in pathGroup.paths)
        {
            if (path.pathName == name.Substring(separatorIndex + 1))
            {
                return path;
            }
        }

        Debug.LogError("No path with name " + name.Substring(separatorIndex + 1) + " Full path: " + name);
        return null;

    }
}

//public class StagePathSave
//{
//    int[][] grid;

//    public Vector2[] GetCoordsInOrder()
//    {
//        List<Vector2> outList = new List<Vector2>();
//        List<int> outListOrder = new List<int>();

//        for(int i = 0; i < grid.Length; i++)
//        {
//            for(int j = 0; j < grid[i].Length; j++)
//            {
//                if(grid[i][j] > 0)
//                {
//                    outList.Add(new Vector2(i, j));
//                    outListOrder.Add(grid[i][j]);
//                }
//            }
//        }

//        Vector2[] returnArray = new Vector2[outList.Count];

//        for(int i = 0; i < outList.Count; i++)
//        {
//            returnArray[outListOrder[i]] = outList[i];
//        }

//        return returnArray;
//    }
//}
