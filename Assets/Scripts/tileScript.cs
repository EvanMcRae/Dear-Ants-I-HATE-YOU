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

    // Start is called before the first frame update
    void Start()
    {
        gm = this.gameObject;
        tf =  gm.GetComponent<Transform>();
        //cords set manually for level 1 will be done programatically later
        camera = GameObject.Find("Main Camera");
        camera.GetComponent<stageManager>().updateTileList(xcord, ycord, gm);
    }

    public void activate() 
    {
        activated = true;
    }
}
