using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadTest : MonoBehaviour
{
    public GameObject saveManagerObject;
    private SaveManager SaveManager;
    

    // Start is called before the first frame update
    void Start()
    {
        SaveManager = saveManagerObject.GetComponent<SaveManager>();

        if(SaveManager.SaveExists())
        {
            //saved data exists
            SaveManager.LoadFromSave();
        } else
        {
            print("no save exists :(");
            //no saved data
            //set level and stage to 1 and start game
        }

    }

    //test for saving... StageManager should call SaveManager.SaveGame() after completion of a stage
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveManager.SaveGame();
        }
    }

}
