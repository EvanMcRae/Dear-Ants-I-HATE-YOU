using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerSightBox : MonoBehaviour
{
    public int antsSeen = 0;
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Enemy"){
            antsSeen += 1;                
        }
    }
    void OnCollisionExit(Collision collision){
        if(collision.gameObject.tag == "Enemy"){
            antsSeen -= 1;                
        }
    }
}
