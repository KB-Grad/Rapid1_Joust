using System;
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
    GameController gc;

    [Header("Characteristics")]
    [SerializeField] float countdownTimer = 5f; // Enemy respawn timer in seconds
    [SerializeField] public float scoreValue = 500;

    private void Start()
    {
        gc = GameObject.FindAnyObjectByType<GameController>();
    }


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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Add Score to GameController
            gc.AddScore(scoreValue);

            Destroy(gameObject);
        }
    }
}
