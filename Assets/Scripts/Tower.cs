using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    string towerType;
    //public int xcord;
    //public int ycord;

    [SerializeField]
    int maxHealth;
    [SerializeField]
    int currHealth;

    //timeToAttack/attackSpeed = how long (in seconds) it takes for tower to fire
    int attackSpeed = 4;
    int timeToAttack = 2;
    [SerializeField]
    float timeSinceLastAttack;
    float[] timeSinceActivated;
    //attackpower for individual hits
    [SerializeField]
    int power;

    //TODO replace with actual hitboxes
    [SerializeField]
    GameObject[] hitboxes;

    [SerializeField]
    stageManager stageManager;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastAttack = 0;
        timeSinceActivated = new float[3];
        foreach(GameObject box in hitboxes){
            box.GetComponent<towerHitbox>().power = power;
        }
        //set x and y coordinate
        //get stagemanager
        //set list of coordiantes to check
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        timeSinceLastAttack += attackSpeed * Time.deltaTime;
        if(timeSinceLastAttack >= timeToAttack){
            attack();
            timeSinceLastAttack = 0;
        }
    }


    public int takeDamage(int damage){
        currHealth -= damage;
        if(currHealth < 0){
            currHealth = 0;
            //then die function I guess
        }
        else if(currHealth > maxHealth){
            currHealth = maxHealth;
        }
        return currHealth;
    }

    void attack(){
        //activate colliders
        foreach(GameObject box in hitboxes){
            box.SetActive(!box.activeSelf);
        }
    }

    //check attackable tiles for an enemy to attack
    bool checkTiles(){
        bool foundEnemy = false;
        return foundEnemy;
    }

    void activateUpgrades(){
        //for each upgrade
            //if upgrade is active type
                //check time and run its function
    }
    
}
