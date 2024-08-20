using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShopListing : ShopListing
{
    public string TowerName;


    public override void OnClick()
    {
        base.OnClick();
        if (GameplayManager.main.resourcePoints < cost)
            return;
        clickToSpawnManager.PlacingMode = clickToSpawnManager.PlacingBehaviour.tower;
        clickToSpawnManager.playerTowerChoice = TowerName;
        clickToSpawnManager.currentPlacementCost = cost;

    }
}
