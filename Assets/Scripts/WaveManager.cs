using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();

    public int CurrentWave;

    public bool inWave = false;

    

    // Start is called before the first frame update
    void Start()
    {
        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        if(inWave)
        {
            waves[CurrentWave].WaveUpdate();

            inWave = !waves[CurrentWave].HasFinished();

            if(!inWave)
            {
                OnFinishWave();
            }
        }
    }

    public void StartWave()
    {
        if (CurrentWave >= waves.Count)
        {
            Debug.LogError("Attempt starting invalid wave num " + CurrentWave + "!");
            return;
        }

        waves[CurrentWave].StartWave();
        inWave = true;
        Debug.Log("Starting wave " + CurrentWave);
    }

    public void OnFinishWave()
    {
        CurrentWave++;
        inWave = false;

        if (CurrentWave >= waves.Count)
            WonMap();
    }

    public void WonMap()
    {
        Debug.Log("Winner");
    }

    

    [System.Serializable]
    public class Wave
    {
        public List<EnemySpawnDetails> enemiesToSpawn = new List<EnemySpawnDetails>();
        public bool currentlySpawning = false;

        List<GameObject> spawnedEnemies = new List<GameObject>();

        public float startTime;

        public void StartWave()
        {
            enemiesToSpawn.Sort();
            spawnedEnemies.Clear();
            
            currentlySpawning = true;
            startTime = Time.time;
        }

        public void WaveUpdate()
        {
            if(enemiesToSpawn.Count <= 0)
            {
                currentlySpawning = false;
                return;
            }

            if (enemiesToSpawn[0].timeToSpawn + startTime < Time.time)
            {
                SpawnEnemy(enemiesToSpawn[0]);
                enemiesToSpawn.RemoveAt(0);
            }
        }

        public void SpawnEnemy(EnemySpawnDetails enemy)
        {
            //spawnedEnemies.Add() //Add spawned enemy gameObj once spawned
            if(enemy.spawnLocationID >= stageManager.main.SpawnLocations.Count)
            {
                Debug.LogError("spawnLocationID " + enemy.spawnLocationID + " is greater than spawn locations: " + (stageManager.main.SpawnLocations.Count - 1));
                return;
            }
            spawnedEnemies.Add(Instantiate(enemy.enemyToSpawn, stageManager.main.SpawnLocations[enemy.spawnLocationID], Quaternion.identity));
            Debug.Log("Spawned creature at: " + enemy.timeToSpawn);
        }

        public bool HasFinished()
        {
            return enemiesToSpawn.Count == 0 && spawnedEnemies.Count == 0;
        }
    }
    [System.Serializable]
    public  class EnemySpawnDetails : IComparable
    {
        //Need enemy to spawn w/ prefab and modifiers
        //Should there be multiple wave managers, one for each spawn point, or should the wave manager handle all, with each spawn details specifying where
        public float timeToSpawn;
        public GameObject enemyToSpawn;
        public int spawnLocationID;

        public int CompareTo(object obj)
        {
            EnemySpawnDetails rhs = (EnemySpawnDetails)obj;
            if (rhs != null)
            {
                if (timeToSpawn < rhs.timeToSpawn)
                    return -1;
                else if (timeToSpawn > rhs.timeToSpawn)
                    return 1;
                else
                    return 0;
            }
            else return 0;
        }


    }
}