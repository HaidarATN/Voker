using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public float spawnInterval = 5, difficultCounter, difficultRaiserValue, enemySpawnNumber;
    public GameObject[] enemyPrefab;

    float enemySpawnNumberTreshold;
    // Start is called before the first frame update
    void Start()
    {
        enemySpawnNumberTreshold = enemySpawnNumber;

        AssignSpawnPoints();

        StartCoroutine(DifficultOverTime());

        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AssignSpawnPoints()
    {
        GameObject[] tempObject = GameObject.FindGameObjectsWithTag("SpawnPoint");

        spawnPoints = new Transform[tempObject.Length];

        for(int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = tempObject[i].transform;
        }
    }

    IEnumerator DifficultOverTime()
    {
        float dur = 0;

        while(true){
            if(dur >= difficultCounter)
            {
                if(spawnInterval > 1)
                {
                    spawnInterval -= difficultRaiserValue;
                    enemySpawnNumberTreshold += 0.2f;
                    enemySpawnNumber = Mathf.RoundToInt(enemySpawnNumberTreshold);
                }

                dur = 0;
            }
            
            dur += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator SpawnEnemy()
    {
        float dur = 0;

        while (true)
        {
            if(dur >= spawnInterval)
            {
                //spawn enemy at random point
                print("Spawn");

                for(int i = 0; i < enemySpawnNumber; i++)
                {
                    Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
                }

                dur = 0;
            }

            dur += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
