using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Vector2 center;
    public Vector2 size;

    public List<SpawnArea> spawnAreas;
    private List<string> poolKeys;
    public GameObject[] enemies;
    public int waves;
    public float timeBetweenWaves;
    public int enemyCount;
    public float spawnIntervalMin;
    public float spawnIntervalMax;

    private DayNightManager dayNightManager;
    private ObjectPooler objectPooler;

    private bool isSpawning = false;
    public bool enableGizmos;

    private void Start()
    {
        dayNightManager = GameObject.Find("DayNightManager").GetComponent<DayNightManager>();
        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
        poolKeys = new List<string>(objectPooler.poolDictionary.Keys);

        // Start spawning coroutine
        StartCoroutine(SpawnDuringNight());
    }

    private IEnumerator SpawnDuringNight()
    {
        while (true)
        {
            // Check if it's nighttime
            if (IsNightTime())
            {
                // Start spawning enemies
                if (!isSpawning)
                {
                    isSpawning = true;
                    StartCoroutine(SpawnWaves(enemies, waves, timeBetweenWaves));
                }
            }
            else
            {
                // Stop spawning enemies during daytime
                if (isSpawning)
                {
                    isSpawning = false;
                    StopAllCoroutines(); // Stop all coroutines
                }
            }

            yield return null; // Wait for the next frame
        }
    }

    private bool IsNightTime()
    {
        if (dayNightManager.currentTimeOfDay == DayNightManager.TimeOfDay.Night)
        {
            return true;
        }
        return false;
    }

    // Precondition: 'enemies' is a valid array of GameObjects
    //  'waves' is a valid float representing number of waves
    //  'timeBetweenWaves' is a valid float representing number of time between waves
    // Postcondition: Calls the SpawnEnemies function 'waves' times waiting a certain number
    //  between waves. Repeats until it has successfully spawned the given number of waves
    private IEnumerator SpawnWaves(GameObject[] enemies, int waves, float timeBetweenWaves)
    {
        for (int i = 0; i < waves; i++)
        {
            yield return StartCoroutine(SpawnEnemies(enemies, enemyCount, spawnIntervalMin, spawnIntervalMax));
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    // Precondition: enemies is a valid array of GameObjects
    //  'amount' is a valid float representing number of enemies
    //  'minTime' is a valid float representing the minimum time in between spawning enemies
    //  'maxTime' is a valid float representing the maximum time in between spawning enemies
    // Postcondition: Instantiates the given amount of enemies with a random time in beetween
    //  buffer
    private IEnumerator SpawnEnemies(GameObject[] enemies, float amount, float minTime, float maxTime)
    {
        Vector2 spawnPosition = GetRandomPointInSpawnArea();
        for (int i = 0; i < amount; i++)
        {
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
            objectPooler.SpawnFromPool(randomEnemy.name, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }

    // Selects a random spawn area and then returns a random Vector2 in the selected spawn area
    private Vector2 GetRandomPointInSpawnArea()
    {
        SpawnArea selectedArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
        float randX = Random.Range(selectedArea.center.x - selectedArea.size.x / 2, selectedArea.center.x + selectedArea.size.x / 2);
        float randY = Random.Range(selectedArea.center.y - selectedArea.size.y / 2, selectedArea.center.y + selectedArea.size.y / 2);

        return new Vector2(randX, randY);
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
