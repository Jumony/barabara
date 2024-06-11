using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Vector2 center;
    public Vector2 size;

    public List<SpawnArea> spawnAreas;
    public GameObject[] enemies;
    public int waves;
    public float timeBetweenWaves;
    public int enemyCount;
    public float spawnIntervalMin;
    public float spawnIntervalMax;

    private DayNightManager dayNightManager;
    private ObjectPooler objectPooler;

    private bool isSpawning = false;
    private Coroutine spawnCoroutine;
    public bool enableGizmos;
    private SpawnArea selectedArea;

    private void Start()
    {
        dayNightManager = GameObject.Find("DayNightManager").GetComponent<DayNightManager>();
        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();

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
                    spawnCoroutine = StartCoroutine(SpawnWaves(enemies, waves, timeBetweenWaves));
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
            selectedArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
            yield return StartCoroutine(SpawnEnemies(enemies, enemyCount, spawnIntervalMin, spawnIntervalMax));
            yield return new WaitForSeconds(timeBetweenWaves);
        }
        isSpawning = false; // Reset isSpawning after waves are complete
    }

    // Precondition: enemies is a valid array of GameObjects
    //  'amount' is a valid float representing number of enemies
    //  'minTime' is a valid float representing the minimum time in between spawning enemies
    //  'maxTime' is a valid float representing the maximum time in between spawning enemies
    // Postcondition: Instantiates the given amount of enemies with a random time in beetween
    //  buffer
    private IEnumerator SpawnEnemies(GameObject[] enemies, float amount, float minTime, float maxTime)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 spawnPosition = GetRandomPointInSpawnArea(selectedArea);
            GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
            objectPooler.SpawnFromPool(randomEnemy.name, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }

    // Selects a random spawn area and then returns a random Vector2 in the selected spawn area
    private Vector2 GetRandomPointInSpawnArea(SpawnArea area)
    {
        float randX = Random.Range(area.center.x - area.size.x / 2, area.center.x + area.size.x / 2);
        float randY = Random.Range(area.center.y - area.size.y / 2, area.center.y + area.size.y / 2);

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
