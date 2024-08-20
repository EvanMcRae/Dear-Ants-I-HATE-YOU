using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterAntScript : EnemyAI
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void Update()
    {
        if (GameplayManager.quit || GameplayManager.won || GameplayManager.lost || GameplayManager.playingAgain) return;

        //Hit end of path
        if (currPathId >= path.path.Count)
        {
            //deal damage to the player
            manager.takeDamage();

            Destroy(gameObject);
            return;
        }

        tileScript nextTile = path.path[currPathId].GetComponent<tileScript>();

        if (nextTile.hasTower && !nextTile.spawnedtower.GetComponent<Tower>().dead)
        {
            //AttackTower
            if (lastAttack + attackCooldown < Time.time)
            {
                lastAttack = Time.time;
                nextTile.spawnedtower.GetComponent<Tower>().takeDamage(attackDamage);
            }
            return;
        }

        if (currPathId + 1 < path.path.Count)
        {
            tileScript nextNextTile = path.path[currPathId + 1].GetComponent<tileScript>();

            if (nextNextTile.hasTower && !nextNextTile.spawnedtower.GetComponent<Tower>().dead)
            {
                //AttackTower
                if (lastAttack + attackCooldown < Time.time)
                {
                    lastAttack = Time.time;
                    nextNextTile.spawnedtower.GetComponent<Tower>().takeDamage(attackDamage);
                }
                return;
            }
        }

        Vector2 targetPosition = new Vector2(path.path[currPathId].transform.position.x, path.path[currPathId].transform.position.z);
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), targetPosition) <= .1f)
        {
            currPathId++;
        }



        Vector2 moveDir = (targetPosition - new Vector2(transform.position.x, transform.position.z)).normalized * moveSpeed * Time.deltaTime;
        transform.position += (new Vector3(moveDir.x, 0, moveDir.y));

        Debug.DrawLine(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.y));

        if (visuals == null)
            return;

        if (moveDir.x > 0)
            visuals.transform.localScale = new Vector3(1, 1, 1);
        else
            visuals.transform.localScale = new Vector3(-1, 1, 1);


    }
}
