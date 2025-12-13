using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpawnControl : MonoBehaviour
{

    [Header("Spawn Properties")]
    [Tooltip("How many Enemies can be spawned at a time")]
    public int spawnLimit;
    [Tooltip("The maximum time between spawns")]
    public float spawnRate = 5.0f;

    // The time at which the next enemy will be spawned
    private float nextSpawnTime = Mathf.NegativeInfinity;

    private List<EnemySpawner> children;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        children = GetComponentsInChildren<EnemySpawner>(false).ToList();
        foreach (EnemySpawner spawner in children)
        {
            spawner.spawnMethod = EnemySpawner.SpawnMethod.Controlled;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (children != null && Enemy.ActiveEnemies.Count < spawnLimit && Time.timeSinceLevelLoad > nextSpawnTime)
        {
            nextSpawnTime = Time.timeSinceLevelLoad + spawnRate;
            children[Random.Range(0, children.Count)].Spawn();
        }
    }
}
