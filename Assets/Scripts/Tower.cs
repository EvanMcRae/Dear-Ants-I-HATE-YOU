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
    float timeSinceLastAttack;
    float[] timeSinceActivated;
    //attackpower for individual hits
    [SerializeField]
    int power;

    //TODO replace with actual hitboxes
    [SerializeField]
    GameObject[] hitboxes;
    [SerializeField]
    towerSightBox[] sightBoxes;

    int numAttachments = 0;
    int maxAttachments = 3;

    //[SerializeField]
    //stageManager stageManager;

    /*enum upgradeTypes
    {
        speedUp = 1
    }*/

    //factor that speed boost upgrade increases tower's speed
    int speedBoost = 2;

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
        
        //calculate total number of ants seen in everysightbox combined
        int antsInRange = 0;
        foreach(towerSightBox box in sightBoxes){
            antsInRange += box.antsSeen;
        }

        if(timeSinceLastAttack >= timeToAttack && antsInRange > 0){
            attack();
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
        //activate colliders for hitboxes
        foreach(GameObject box in hitboxes){
            box.SetActive(!box.activeSelf);
            //might want to change this to set delay/allow cooler visual effects
            //or keep the same so attack also retracts faster at high speeds
        }
        timeSinceLastAttack = 0;
    }

    //check attackable tiles for an enemy to attack
    /*bool checkTiles(){
        bool foundEnemy = false;
        return foundEnemy;
    }*/

    //
    bool attachUpgrade(int upgradeType){
        bool attached = false;
        if(numAttachments < maxAttachments)
            //add new attack=hment
            numAttachments += 1;
            attached = true;

            if(upgradeType == 1){
                attackSpeed *= speedBoost;
            }
            //if other stat boosting type
                //boost stat
            //else if(upgradeType = activated ability)
                //???need to do anything?

            //create child object to render upgrade
        

        return attached;
    }


    void activateUpgrades(){
        //for each upgrade
            //if upgrade is active type
                //check time and run its function
    }
    
}
