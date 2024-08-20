using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShopeListing : ShopListing
{
    public int upgradeID;

    public override void OnClick()
    {
        base.OnClick();

        if (cost > GameplayManager.main.resourcePoints)
            return;

        clickToSpawnManager.currentUpgradeChoice = upgradeID;
        clickToSpawnManager.PlacingMode = clickToSpawnManager.PlacingBehaviour.upgrade;
        clickToSpawnManager.currentPlacementCost = cost;
        backing.color = Color.white;
        GameplayManager.main.spendResource(clickToSpawnManager.currentPlacementCost);
        //button.Select();
    }

    public override void Update()
    {
        base.Update();


        if (clickToSpawnManager.PlacingMode != clickToSpawnManager.PlacingBehaviour.upgrade || (clickToSpawnManager.currentUpgradeChoice != upgradeID))
        {
            backing.color = new Color(.8f, .8f, .8f);
        }
    }
}
