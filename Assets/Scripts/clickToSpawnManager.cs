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