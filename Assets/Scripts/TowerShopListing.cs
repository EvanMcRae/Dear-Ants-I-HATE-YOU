using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        backing.color = Color.white;
        GameplayManager.main.spendResource(clickToSpawnManager.currentPlacementCost);
        //button.Select();

    }

    public override void Update()
    {
        base.Update();

        if (clickToSpawnManager.PlacingMode != clickToSpawnManager.PlacingBehaviour.tower || (clickToSpawnManager.playerTowerChoice != TowerName))
        {
            backing.color = new Color(.8f, .8f, .8f);
            //EventSystem.current.SetSelectedGameObject(null);
        }

    }
}
