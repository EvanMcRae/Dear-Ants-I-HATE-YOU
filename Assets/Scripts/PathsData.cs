using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathsData : MonoBehaviour
{
    [SerializeField]
    public List<Path> paths = new List<Path>();

    public string folderName;

    private void Start()
    {
        //stageManager.main.AIPathings.AddRange(paths.ToArray());
        stageManager.main.AIPathings.Add(this);

    }
}

[System.Serializable]
public class Path
{
    public List<GameObject> path = new List<GameObject>();
    public string pathName;
}
