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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currPathId >= path.path.Count)
        {
            //Hit end of path
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
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
    
