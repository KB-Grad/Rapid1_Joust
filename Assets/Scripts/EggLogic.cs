using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EggLogic : MonoBehaviour
{
    [Header("Prefab References")]
    [SerializeField] GameObject enemyPrefab;

    [Header("Characteristics")]
    [SerializeField] float countdownTimer = 5f; // Enemy respawn timer in seconds
    [SerializeField] public float scoreValue = 500;


    // Update is called once per frame
    void Update()
    {
        countdownTimer -= Time.deltaTime;

        if (countdownTimer <= 0) 
        {
            // Scoring
            GameObject scoreController = GameObject.FindGameObjectWithTag("GameController");
            if (scoreController != null)
            {
                // TODO:: Add scoreValue to the game controller's player score.
            }

            // Spawn knight on current location
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyMovement>().scoreValue = 0;

            // Cleanup
            Destroy(gameObject);
        }
    }
}
