using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
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

    //TODO replace with actual hitboxes
    [SerializeField]
    GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastAttack = 0;
        timeSinceActivated = new float[3];
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
        cube.SetActive(!cube.activeSelf);
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
