using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public Path path;
    public int currPathId = 0;

    public float moveSpeed = 1;

    public int health = 10;

    public float lastAttack;
    public float attackCooldown;
    public int attackDamage = 1;

    //resource points awarded when killed
    public int resourceAward = 1;
    
    GameplayManager manager;

    public Animator visuals;

    public GameObject DeathExplosion;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameplayManager>();
    }

    // Update is called once per frame
    void Update()
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

        if (nextTile.hasTower)
        {
            //AttackTower
            if(lastAttack + attackCooldown < Time.time)
            {
                lastAttack = Time.time;
                nextTile.spawnedtower.GetComponent<Tower>().takeDamage(attackDamage);
            }
            return;
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

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        manager.addResource(resourceAward);
        GameObject addedExplosion = Instantiate(DeathExplosion, transform.position, Quaternion.Euler(90, 0, 0));
        addedExplosion.transform.localScale = Vector3.one * Random.Range(.2f, .3f);
        Destroy(gameObject);
    }
}
    
