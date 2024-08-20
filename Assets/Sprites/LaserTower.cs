using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
    public int DirectionFacing = 0;
    public float originalScale;
    //0 - up
    //1 - down
    //2 - left
    //3 - right


    protected override void Start()
    {
        base.Start();
        originalScale = anim.transform.localScale.x;
    }

    protected override void FixedUpdate()
    {
        if (GameplayManager.quit || GameplayManager.won || GameplayManager.lost || GameplayManager.playingAgain) return;
        timeSinceLastAttack += attackSpeed * Time.deltaTime;

        //calculate total number of ants seen in everysightbox combined
        int antsInRange = sightBoxes[0].InDepthCount() + sightBoxes[1].InDepthCount();
        if (antsInRange <= 0)
        {
            if (attacking)
                stopAttacking();
            return;
        }

        int[] directionCounts = new int[4];
        for (int i = 0; i < sightBoxes[0].seenAnts.Count; i++)
        {
            if (sightBoxes[0].seenAnts[i].transform.position.x < transform.position.x)
                directionCounts[2]++;
            else
                directionCounts[3]++;
        }
        for (int i = 0; i < sightBoxes[1].seenAnts.Count; i++)
        {
            if (sightBoxes[1].seenAnts[i].transform.position.z < transform.position.z)
                directionCounts[1]++;
            else
                directionCounts[0]++;
        }

        int largestDir = -1;
        int largestCount = -1;

        for(int i = 0; i < directionCounts.Length;i++)
        {
            if (directionCounts[i] > largestCount)
            {
                largestDir = i;
                largestCount = directionCounts[i];
            }
        }

        
        anim.SetFloat("Blend", Mathf.Clamp(largestDir, 0, 2));
        if (largestDir == 2)
            anim.transform.localScale = new Vector3(-1, 1, 1) * originalScale;
        else
            anim.transform.localScale = Vector3.one * originalScale;

        if (timeSinceLastAttack >= timeToAttack && antsInRange > 0 && !dead)
        {
            attack();
        }
        else if (antsInRange <= 0 && attacking)
        {
            stopAttacking();
        }
    }
}
