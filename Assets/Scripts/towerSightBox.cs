using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerSightBox : MonoBehaviour
{
    public int antsSeen = 0;
    List<GameObject> seenAnts = new List<GameObject>();

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            antsSeen += 1;
            seenAnts.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            antsSeen -= 1;
            seenAnts.Remove(other.gameObject);
        }
    }

    public int InDepthCount()
    {
        for(int i = seenAnts.Count - 1; i >= 0; i--)
        {
            if (seenAnts[i] == null)
                seenAnts.RemoveAt(i);
        }
        return seenAnts.Count;
    }
}
