using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    //string towerType;
    //public int xcord;
    //public int ycord;

    [SerializeField]
    protected int maxHealth;
    [SerializeField]
    protected int currHealth;
    //denotes if health has been depleted
    public bool dead = false;
    //denotes if currenly attacking, used to retract hitboxes if dieing with them out
    protected bool attacking = false;

    //timeToAttack/attackSpeed = how long (in seconds) it takes for tower to fire
    protected int attackSpeed = 4;
    protected int timeToAttack = 2;
    protected float timeSinceLastAttack;
    protected float[] timeSinceActivated;
    //attackpower for individual hits
    [SerializeField]
    protected int power;

    [SerializeField]
    protected GameObject[] hitboxes;
    [SerializeField]
    protected towerSightBox[] sightBoxes;

    protected int numAttachments = 0;
    protected int maxAttachments = 3;

    //[SerializeField]
    //stageManager stageManager;

    /*enum upgradeTypes
    {
        speedUp = 1
    }*/

    //factor that speed boost upgrade increases tower's speed
    int speedBoost = 2;
    //factor that battery attachment increases tower's health
    int healthBoost = 2;
    //ammount attack power is boosted by with attack upgrade
    int powerBoost = 6;

    public Animator anim;
    [SerializeField] private Image powerBar;

    public int towerIndex;
    public TowerData towerData;
    

    // Start is called before the first frame update
    protected virtual void Start()
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

    public void UnpackData()
    {
        currHealth = towerData.health;
        power = towerData.power;
        attackSpeed = towerData.attackSpeed;
        dead = towerData.dead;
        attacking = towerData.attacking;
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected virtual void FixedUpdate()
    {
        if (GameplayManager.quit || GameplayManager.won || GameplayManager.lost || GameplayManager.playingAgain) return;
        timeSinceLastAttack += attackSpeed * Time.deltaTime;
        
        //calculate total number of ants seen in everysightbox combined
        int antsInRange = 0;
        foreach(towerSightBox box in sightBoxes){
            antsInRange += box.InDepthCount();
        }

        if(timeSinceLastAttack >= timeToAttack && antsInRange > 0 && !dead){
            attack();
        }
        else if(antsInRange <= 0 && attacking){
            stopAttacking();
        }

        powerBar.fillAmount = (float)currHealth / maxHealth;

        towerData.health = currHealth;
        towerData.power = power;
        towerData.attackSpeed = attackSpeed;
        towerData.dead = dead;
        towerData.attacking = attacking;
        clickToSpawnManager.placedTowers[towerIndex] = towerData;
    }

    //take damage from enemies
    public int takeDamage(int damage){
        currHealth -= damage;

        if(currHealth <= 0){
            currHealth = 0;

            dead = true;
            anim.Play("off");
            //then anything else that needs to happen if dead
            stopAttacking();
        }
        else if(currHealth > maxHealth){
            currHealth = maxHealth;
        }
        return currHealth;
    }

    public void healToFull(){
        currHealth = maxHealth;
        dead = false;
        anim.Play("Attack");
    }

    protected virtual void attack(){
        //activate colliders for hitboxes
        attacking = !attacking;
        anim.Play("Attack");
        foreach(GameObject box in hitboxes){ 
            box.SetActive(attacking);
            //might want to change this to set delay/allow cooler visual effects
            //or keep the same so attack also retracts faster at high speeds
        }
        timeSinceLastAttack = 0;
    }

    //used to stop attacking when dead or enemies out of range
    protected virtual void stopAttacking(){
        attacking = false;
        foreach(GameObject box in hitboxes){ 
            box.SetActive(attacking);
        }
    }

    //attach upgrade to tower and give it corresponding boost
    bool attachUpgrade(int upgradeType){
        bool attached = false;
        if(numAttachments < maxAttachments)
            //add new attack=hment
            numAttachments += 1;
            attached = true;

            if(upgradeType == 1){
                attackSpeed += speedBoost;
            }
            else if(upgradeType == 2){
                maxHealth += healthBoost;
            }
            else if(upgradeType == 3){
                power += powerBoost;
            }
            else if(upgradeType == 4){
                healToFull();
            }

            //if other stat boosting type
                //boost stat
            //else if(upgradeType = activated ability)
                //???need to do anything?

            //create child object to render upgrade
        

        return attached;
    }

    //might not use if not doing "active" upgrades like auto heal
    void activateUpgrades(){
        //for each upgrade
            //if upgrade is active type
                //check time and run its function
    }

    void OnMouseDown()
    {
        // Reject tower placement if game is not playable at the moment
        // TODO add condition for start cutscene
        if (GameplayManager.paused || GameplayManager.lost || GameplayManager.won || !ScreenWipe.over)
            return;


        // Spawn the prefab on this tile
        if (clickToSpawnManager.PlacingMode == clickToSpawnManager.PlacingBehaviour.upgrade)
        {
            AttemptUpgrade();
        }
    }

    public void AttemptUpgrade()
    {
        Debug.Log("Upgraded Tower");
        attachUpgrade(clickToSpawnManager.currentUpgradeChoice);
        clickToSpawnManager.PlacingMode = clickToSpawnManager.PlacingBehaviour.none;
        //GameplayManager.main.spendResource(clickToSpawnManager.currentPlacementCost);
    }


}
