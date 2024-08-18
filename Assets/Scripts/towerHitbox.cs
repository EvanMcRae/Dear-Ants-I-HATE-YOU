using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerHitbox : MonoBehaviour
{
    public int power;
    void OnCollisionEnter(Collision collision){
        //if object is an ant
            //can see it?
            //call its dealdamage
        Debug.Log("I see collider");
    }
}
