using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();

    public static int CurrentWave;

    public bool inWave = false;

    public GameplayManager gameManager;
    public TextMeshProUGUI WaveLabel, WaveValue;
    public static bool captureWaveMonitor = false;

    // Start is called before the first frame update
    void Start()
    {
        //StartWave();
        gameManager = FindObjectOfType<GameplayManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameplayManager.quit || GameplayManager.won || GameplayManager.lost || GameplayManager.playingAgain) return;

        if (inWave)
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
        StartCoroutine(StartWaveDelayed(10));
    }

    public IEnumerator StartWaveDelayed(int time)
    {
        captureWaveMonitor = true;
        for (int i = time; i > 0; i--)
        {
            WaveLabel.text = "<voffset=0em><size=50%> NEXT WAVE\nSTARTS IN</size></margin>";
            WaveValue.text = i + "";
            yield return new WaitForSeconds(1);
        }
        WaveLabel.text = "WAVE";
        WaveValue.text = CurrentWave + 1 + "";
        captureWaveMonitor = false;
        waves[CurrentWave].StartWave();
        inWave = true;
        Debug.Log("Starting wave " + CurrentWave);
    }

    public void OnFinishWave()
    {
        CurrentWave++;
        inWave = false;
        GameplayManager.AutoSave();

        if (CurrentWave >= waves.Count)
        {
            WonMap();
            return;
        }

        stageManager.main.advanceStage();
        

        

        //Starts next wave on prior one finishing
        StartWave();
    }

    public void WonMap()
    {
        if (stageManager.level == 3)
        {
            gameManager.WinGame();
            return;
        }
        gameManager.Win();
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
            

            while(enemiesToSpawn.Count > 0 && enemiesToSpawn[0].timeToSpawn + startTime < Time.time)
            {
                SpawnEnemy(enemiesToSpawn[0]);
                enemiesToSpawn.RemoveAt(0);
            }
            if (enemiesToSpawn.Count <= 0)
            {
                currentlySpawning = false;
            }
        }

        public void SpawnEnemy(EnemySpawnDetails enemy)
        {
            Path path = stageManager.main.GetPathByName(enemy.spawnLocationID);
            spawnedEnemies.Add(Instantiate(enemy.enemyToSpawn, path.path[0].transform.position + Vector3.up * 2, Quaternion.identity));
            EnemyAI spawnedEnemy = spawnedEnemies[^1].GetComponent<EnemyAI>();
            spawnedEnemy.path = path;

            Debug.Log("Spawned creature at: " + enemy.timeToSpawn);
        }

        public bool HasFinished()
        {
            for(int i = spawnedEnemies.Count - 1; i >= 0; i--)
            {
                if (spawnedEnemies[i] == null)
                    spawnedEnemies.RemoveAt(i);
            }

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
        public string spawnLocationID;

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
