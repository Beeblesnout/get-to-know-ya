using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
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
    private GameObject[] existingEnemies;
    private int spawnedEnemies;

    // Start is called before the first frame update
    void Start()
    {
        currentSpawnTime = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        //existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (currentSpawnTime < 0)
        {
            currentSpawnTime = spawnTime;
        }
        
        currentSpawnTime -= Time.deltaTime;

        if (spawnedEnemies < enemyLimit && currentSpawnTime <= 0)
        {
            positionX = Random.Range(spawnPositionXMin, spawnPositionXMax);
            positionY = Random.Range(spawnPositionYMin, spawnPositionYMax);

            Instantiate(enemy, new Vector3(positionX, positionY), Quaternion.identity);
            spawnedEnemies++;
        }
    }
}
