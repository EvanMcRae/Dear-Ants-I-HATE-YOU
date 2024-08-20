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
    }
}
