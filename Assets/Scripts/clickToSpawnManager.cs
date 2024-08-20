using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickToSpawnManager : MonoBehaviour
{
    public GameObject meleeTower;
    public GameObject laserTower;
    public GameObject bombTower;
    public static List<TowerData> placedTowers = new List<TowerData>();

    public static string playerTowerChoice = "laser";

    public static int currentPlacementCost;
    public static int currentUpgradeChoice = 0;

    public enum PlacingBehaviour
    {
        none,
        tower,
        upgrade
    }

    public static PlacingBehaviour PlacingMode = PlacingBehaviour.none;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //pick different types of towers to place
    public void pickNothing(){
        playerTowerChoice = "";
    }
    public void pickMelee(){
        playerTowerChoice = "melee";
    }
    public void pickLaser(){
        playerTowerChoice = "laser";
    }
    public void pickBomb(){
        playerTowerChoice = "bomb";
    }
}
