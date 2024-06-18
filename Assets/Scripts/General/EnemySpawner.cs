using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<SpawnArea> spawnAreas;
    public List<EnemySpawnData> enemySpawnDataList;

    public float spawnIntervalMin;
    public float spawnIntervalMax;
    private DayNightManager dayNightManager;
    private ObjectPooler objectPooler;

    private bool isSpawning = false;
    public bool gracePeriod;
    private Coroutine spawnCoroutine;
    public bool enableGizmos;
    private SpawnArea selectedArea;

    [Header("Spawning Behaviour")]
    public int initialWaves;
    public float initialTimeBetweenWaves;
    public int initialEnemyCount;

    private float currentTimeBetweenWaves;
    private int currentEnemyCount;

    private int activeEnemies = 0; // Counter to track active enemies

    private void Start()
    {
        dayNightManager = GameObject.Find("DayNightManager").GetComponent<DayNightManager>();
        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();

        currentTimeBetweenWaves = initialTimeBetweenWaves;
        currentEnemyCount = initialEnemyCount;

        // Subscribe to the night start event
        DayNightManager.OnNightStart += IncreaseDifficulty;

        // Start spawning coroutine
        StartCoroutine(SpawnDuringNight());
    }

    private void OnDestroy()
    {
        // Unsubscribe from the night start event to prevent memory leaks
        DayNightManager.OnNightStart -= IncreaseDifficulty;
    }

    private IEnumerator SpawnDuringNight()
    {
        while (true)
        {
            // Check if it's nighttime
            if (IsNightTime())
            {
                // Start spawning enemies
                if (!isSpawning && !gracePeriod)
                {
                    isSpawning = true;
                    spawnCoroutine = StartCoroutine(SpawnWaves(currentTimeBetweenWaves));
                }
            }
            else
            {
                // Stop spawning enemies during daytime
                if (isSpawning)
                {
                    isSpawning = false;
                    if (spawnCoroutine != null)
                    {
                        StopCoroutine(spawnCoroutine);
                        spawnCoroutine = null;
                    }
                }
            }

            yield return null; // Wait for the next frame
        }
    }

    private bool IsNightTime()
    {
        return dayNightManager.currentTimeOfDay == DayNightManager.TimeOfDay.Night;
    }

    private IEnumerator SpawnWaves(float timeBetweenWaves)
    {
        while (true)
        {
            selectedArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
            yield return StartCoroutine(SpawnEnemies(currentEnemyCount, spawnIntervalMin, spawnIntervalMax));

            // Wait for all enemies to be defeated before proceeding to the next wave
            yield return new WaitUntil(() => activeEnemies == 0);

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private IEnumerator SpawnEnemies(float enemyCount, float minTime, float maxTime)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 spawnPosition = GetRandomPointInSpawnArea(selectedArea);
            GameObject randomEnemy = GetRandomEnemyBasedOnWeight();
            objectPooler.SpawnFromPool(randomEnemy.name, spawnPosition, Quaternion.identity);

            // Increment the active enemies counter when an enemy is spawned
            activeEnemies++;

            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }

    private GameObject GetRandomEnemyBasedOnWeight()
    {
        float totalWeight = 0f;
        foreach (EnemySpawnData spawnData in enemySpawnDataList)
        {
            totalWeight += spawnData.spawnWeight;
        }

        float randomWeight = Random.Range(0, totalWeight);
        foreach (EnemySpawnData spawnData in enemySpawnDataList)
        {
            if (randomWeight < spawnData.spawnWeight)
            {
                return spawnData.enemyType.prefab;
            }

            randomWeight -= spawnData.spawnWeight;
        }

        return enemySpawnDataList[0].enemyType.prefab;
    }

    private Vector2 GetRandomPointInSpawnArea(SpawnArea area)
    {
        float randX = Random.Range(area.center.x - area.size.x / 2, area.center.x + area.size.x / 2);
        float randY = Random.Range(area.center.y - area.size.y / 2, area.center.y + area.size.y / 2);

        return new Vector2(randX, randY);
    }

    private void IncreaseDifficulty()
    {
        currentEnemyCount += 2;

        spawnIntervalMin = Mathf.Max(0.5f, spawnIntervalMin - 0.1f);
        spawnIntervalMax = Mathf.Max(1f, spawnIntervalMax - 0.2f);
    }

    public void EnemyDefeated()
    {
        activeEnemies--; // Decrement the active enemies counter when an enemy is defeated
    }

    private void OnDrawGizmos()
    {
        if (enableGizmos)
        {
            // Draw gizmo for each SpawnArea
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            foreach (SpawnArea area in spawnAreas)
            {
                // DrawCube expects center and size as arguments, so calculate center based on area.center
                Vector3 areaCenter = new Vector3(area.center.x, area.center.y, 0);
                Gizmos.DrawCube(areaCenter, area.size);
            }
        }
    }
}
