using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
