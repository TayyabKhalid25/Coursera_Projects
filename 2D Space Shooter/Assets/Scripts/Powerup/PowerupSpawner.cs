using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which spawns powerups in an area around it.
/// </summary>
public class PowerupSpawner : MonoBehaviour
{
    [Header("GameObject References")]
    [Tooltip("The enemy prefab to use when spawning enemies")]
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    [Header("Spawn Position")]
    [Tooltip("The distance within which enemies can spawn in the X direction")]
    [Min(0)]
    public float spawnRangeX = 10.0f;
    [Tooltip("The distance within which enemies can spawn in the Y direction")]
    [Min(0)]
    public float spawnRangeY = 10.0f;

    [Header("Spawn Variables")]
    [Tooltip("The maximum number of enemies that can be spawned from this spawner")]
    public int maxSpawn = 20;
    [Tooltip("Ignores the max spawn limit if true")]
    public bool spawnInfinite = true;

    // The number of enemies that have been spawned
    private int currentlySpawned = 0;

    [Tooltip("The time delay between spawning enemies")]
    public float spawnDelay = 2.5f;

    [Tooltip("Link UIManager to show timers for powerups")]    
    public UIManager uiManager;


    // The most recent spawn time
    private float lastSpawnTime = Mathf.NegativeInfinity;

    /// <summary>
    /// Description:
    /// Standard Unity function called every frame
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    private void Update()
    {
        CheckSpawnTimer();
    }

    /// <summary>
    /// Description:
    /// Checks if it is time to spawn an enemy
    /// Spawns an enemy if it is time
    /// Inputs: 
    /// none
    /// Returns: 
    /// void (no return)
    /// </summary>
    private void CheckSpawnTimer()
    {
        // If it is time for an enemy to be spawned
        if (Time.timeSinceLevelLoad > lastSpawnTime + spawnDelay && (currentlySpawned < maxSpawn || spawnInfinite))
        {
            // Determine spawn location
            Vector3 spawnLocation = GetSpawnLocation();

            // Spawn an enemy
            SpawnEnemy(spawnLocation);
        }
    }

    /// <summary>
    /// Description:
    /// Spawn and set up an instance of the enemy prefab
    /// Inputs: 
    /// Vector3 spawnLocation
    /// Returns: 
    /// void (no return)
    /// </summary>
    /// <param name="spawnLocation">The location to spawn an enmy at</param>
    private void SpawnEnemy(Vector3 spawnLocation)
    {
         // Check that the prefab list is valid and not empty
        if (enemyPrefabs != null && enemyPrefabs.Count > 0)
        {
            // Pick a random projectile from the list
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyPrefab = enemyPrefabs[randomIndex];

            if (enemyPrefab != null)
            {
                // Create the enemy gameobject
                GameObject enemyGameObject = Instantiate(enemyPrefab, spawnLocation, enemyPrefab.transform.rotation, null);
                enemyGameObject.GetComponent<Powerup>().uiManager = uiManager;

                // Incremment the spawn count
                currentlySpawned++;
                lastSpawnTime = Time.timeSinceLevelLoad;
            }
        }
    }

    /// <summary>
    /// Description:
    /// Returns a generated spawn location for an enemy
    /// Inputs: 
    /// none
    /// Returns: 
    /// Vector3
    /// </summary>
    /// <returns>Vector3: The spawn location as determined by the function</returns>
    protected virtual Vector3 GetSpawnLocation()
    {
        // Get random coordinates
        float x = Random.Range(0 - spawnRangeX, spawnRangeX);
        float y = Random.Range(0 - spawnRangeY, spawnRangeY);
        // Return the coordinates as a vector
        return new Vector3(transform.position.x + x, transform.position.y + y, 0);
    }
}
