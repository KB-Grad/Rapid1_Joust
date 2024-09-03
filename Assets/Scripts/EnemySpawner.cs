using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] List<Transform> spawnPoints;
    Transform lastSpawnPoint;
    [SerializeField] List<Wave> waves;
    Wave currentWave = null;
    [SerializeField] float spawnDelay = .5f;
    float spawnTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        // Detect if current wave is over
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && currentWave == null) 
        {
            if (waves.Count > 0)
            {
                currentWave = waves[0];
                waves.RemoveAt(0);
            }
            else 
            {
                // TODO:: End Game
            }
        }
        
        // Spawn the current wave
        if (currentWave != null)
        {
            if (spawnTimer <= 0)
            {
                if (currentWave.enemies.Count > 0)
                {
                    // Pick the next spawn location
                    Transform spawnPoint;
                    do
                    {
                        spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                    } while (lastSpawnPoint == spawnPoint);
                    lastSpawnPoint = spawnPoint;

                    // Spawn the enemy
                    Instantiate(currentWave.enemies[0], spawnPoint.position, Quaternion.identity);
                    currentWave.enemies.RemoveAt(0);

                    // Restart the spawn timer
                    spawnTimer = spawnDelay;
                }
                else 
                {
                    currentWave = null;
                }
            }
            else 
            {
                spawnTimer -= Time.deltaTime;
                print(spawnTimer);
            }
        }
    }

    [System.Serializable]
    private class Wave
    {
        public List<GameObject> enemies;
    }
}


