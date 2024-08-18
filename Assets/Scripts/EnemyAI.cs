using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public tileScript currentTile;
    public tileScript incomingTile;
    public tileScript nextTile;


    public float moveSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTile == null || nextTile == null)
            return;

        transform.position += (nextTile.transform.position - currentTile.transform.position).normalized * Time.deltaTime;
    }

    public void ChangeTile(tileScript newTile)
    {
        currentTile = newTile;
        nextTile = stageManager.main.GetNextTileInPath(newTile);
    }

    public void PrimeTile(tileScript tile)
    {
        incomingTile = tile;
        if (currentTile == null)
            AcceptChange();
    }

    public void AcceptChange()
    {
        ChangeTile(incomingTile);
    }
}
