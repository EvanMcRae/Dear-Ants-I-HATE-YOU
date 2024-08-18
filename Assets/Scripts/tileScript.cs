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

    // Start is called before the first frame update
    void Start()
    {
        gm = this.gameObject;
        tf =  gm.GetComponent<Transform>();
        //cords set manually for level 1 will be done programatically later
        camera = GameObject.Find("Main Camera");
        camera.GetComponent<stageManager>().updateTileList(xcord, ycord, gm);
    }

    void OnMouseDown()
    {
        //do something with the game object after clicking on it
        print("name: " + gm.name + " xcord: " + xcord + " ycord: " + ycord);
    }
}
