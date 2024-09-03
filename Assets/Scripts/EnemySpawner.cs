using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
    GameObject temp = null;


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
                    int i = 0;
                    // Pick the next spawn location
                    Transform spawnPoint;
                    do
                    {
                        spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                    } while (lastSpawnPoint == spawnPoint);
                    lastSpawnPoint = spawnPoint;

                    // Spawn the enemy
                    temp = Instantiate(currentWave.enemies[0], spawnPoint.position, Quaternion.identity);
                    temp.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    Invoke("delay", 3);
                    //temp.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    currentWave.enemies.RemoveAt(0);
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
                //print(spawnTimer);
            }
        }
    }

    void delay()
    {
        temp.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void enemyProjection(GameObject obj)
    {
        /*float delayed = Time.time;
        obj.GetComponent<Rigidbody2D>().Sleep();
        for (float tm = Time.time; delayed >= 300f; delayed = Time.time - tm)
        {
            if (delayed % 1 == 0)
            {
                obj.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.green, 1f);
            }
        }
        obj.GetComponent<Rigidbody2D>().WakeUp();*/
    }

    [System.Serializable]
    private class Wave
    {
        public List<GameObject> enemies;
    }
}


