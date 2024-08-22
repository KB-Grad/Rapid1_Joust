using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Prefab References")]
    [SerializeField] GameObject eggPrefab;

    [Header("Characteristics")]
    [SerializeField] bool isSpawning = true;
    [SerializeField] public float scoreValue = 500;
    [SerializeField] float groundAccelleration = 1.0f;
    [SerializeField] float cruiseSpeed = 3.0f;
    [SerializeField] float maxMoveSpeed = 5.0f;
    [SerializeField] float trackingAngle = 15f; // The max angle, in degrees, off of horizontal the player can be seen by this enemy
    [SerializeField] float trackingDistance = 5f; // The furthest distance the player can be seen by this enemy
    [SerializeField] bool DEBUG_doDie = false;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // DEBUG check if dying logic works
        if (DEBUG_doDie)
        {
            Die();
            return;
        }

        // Don't try to move while spawning
        if (!isSpawning)
            return;

        // If velocity is 0, pick a random direction to go in, otherwise, continue in the previous direction.
        int dir = 0;
        if (rb.velocity.x == 0f)
        {
            dir = (int)Mathf.Sign(Random.Range(-1, 1));
        }
        else
        {
            dir = (int)Mathf.Sign(rb.velocity.x);
        }

        // Change velocity
        Vector2 deltaV = Vector2.right * dir * groundAccelleration * Time.deltaTime;
        if (Mathf.Abs(rb.velocity.x + deltaV.x) < maxMoveSpeed)
            rb.velocity += deltaV;
        else
            rb.velocity = new Vector2(dir * maxMoveSpeed, rb.velocity.y);
    }

    public void Die()
    {
        // Scoring
        GameObject scoreController = GameObject.FindGameObjectWithTag("GameController");
        if (scoreController != null)
        {
            // TODO:: Add scoreValue to the game controller's player score.
        }

        // Spawn Egg
        GameObject newEgg = Instantiate(eggPrefab, transform.position, Quaternion.identity);
        newEgg.GetComponent<Rigidbody2D>().velocity = rb.velocity;

        // Cleanup
        Destroy(gameObject);
    }
}
