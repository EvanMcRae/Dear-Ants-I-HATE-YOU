using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerHitbox : MonoBehaviour
{
    public int power;
    void OnCollisionEnter(Collision collision){
        //if object is an enemy
        //call its takedamage function using power

        EnemyAI hitEnemy = collision.gameObject.GetComponent<EnemyAI>();

        if(hitEnemy != null && !hitEnemy.dead)
        {
            hitEnemy.TakeDamage(power);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyAI hitEnemy = other.gameObject.GetComponent<EnemyAI>();

        if (hitEnemy != null && !hitEnemy.dead)
        {
            hitEnemy.TakeDamage(power);
        }
    }
}
