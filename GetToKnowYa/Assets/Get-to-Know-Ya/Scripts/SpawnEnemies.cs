using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public QuestionManager qMan;
    public bool isSpawning = false;
    
    [Header("Enemy Type")]
    public GameObject enemy;

    [Header("Spawned Enemies Limit")]
    public int enemyLimit;

    [Header("Time between Enemy Spawns")]
    public float spawnTime;
    
    [Header("Enemy Spawn Position Range")]
    public float spawnPositionXMin;
    public float spawnPositionXMax;
    public float spawnPositionYMin;
    public float spawnPositionYMax;

    private float positionX;
    private float positionY;
    private float currentSpawnTime;
    public List<GameObject> existingEnemies = new List<GameObject>();
    private int spawnedEnemies;

    void Start()
    {
        SpawnWave(15);
    }

    void Update()
    {
        if (isSpawning) Spawn();
        existingEnemies.RemoveAll(e => e == null);
    }

    void Spawn()
    {
        currentSpawnTime -= Time.deltaTime;
        if (currentSpawnTime < 0)
        {
            currentSpawnTime = spawnTime;
            if (spawnedEnemies+1 < enemyLimit)
            {
                positionX = Random.Range(spawnPositionXMin, spawnPositionXMax);
                positionY = Random.Range(spawnPositionYMin, spawnPositionYMax);

                existingEnemies.Add(Instantiate(enemy, new Vector3(positionX, positionY), Quaternion.identity));
                spawnedEnemies++;
            }
            else
            {
                isSpawning = false;
            }
        }
    }

    public void SpawnWave(int amount)
    {
        enemyLimit = amount;
        spawnedEnemies = 0;
        existingEnemies = new List<GameObject>();
        currentSpawnTime = spawnTime;
        isSpawning = true;
    }
}
