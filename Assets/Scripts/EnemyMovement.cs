using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Prefab References")]
    [SerializeField] GameObject eggPrefab;

    [Header("Component References")]
    [SerializeField] Collider2D groundCheckCollider;
    Rigidbody2D rb;

    [Header("State Tracking")]
    [SerializeField] bool isSpawning = true;
    [SerializeField] bool isGrounded = true;
    [SerializeField] bool isChasing = false;
    GameObject chaseTarget;
    [SerializeField][Range(-1, 1)] int wanderDir_Vertical = 0;

    [Header("Characteristics")]
    [SerializeField] public float scoreValue = 500;
    [SerializeField] Vector2 groundAccelleration = new Vector2(1, 1);
    [SerializeField] Vector2 airAccelleration = new Vector2(.5f, 1);
    [SerializeField] Vector2 wanderSpeedLimits = new Vector2(3.0f, 1.0f);
    [SerializeField] Vector2 chaseSpeedLimits = new Vector2(5.0f, 1.0f);
    [SerializeField] float trackingAngle = 15f; // The max angle, in degrees, off of horizontal the player can be seen by this enemy
    [SerializeField] float trackingDistance = 5f; // The furthest distance the player can be seen by this enemy
    [SerializeField][Range(0, 30)] float wanderDelay_UpperBound = 10f;
    [SerializeField][Range(0, 30)] float wanderDelay_LowerBound = 7f;
    float wanderTimer = 0f;
    [SerializeField] bool DEBUG_doDie = false;

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

        // Check for grounded
        List<Collider2D> col2D_List = new();
        ContactFilter2D contactFilter = new();
        groundCheckCollider.OverlapCollider(contactFilter, col2D_List);
        isGrounded = false;
        foreach (Collider2D col2D in col2D_List) 
        {
            if (col2D.gameObject.tag.ToLower().Equals("ground"))
            {
                isGrounded = true;
                break;
            }
        }
        
        int dir_Horizontal = 0;
        Vector2 newVelocity = Vector2.zero;
        if (isChasing) // ----------------------- Chase State -----------------------
        {
            dir_Horizontal = (int)Mathf.Sign(chaseTarget.transform.position.x - transform.position.x);
        }
        else // ----------------------- Wander State -----------------------
        {
            // Update Horizontal Wander Timer
            if (wanderTimer <= 0)
            {
                wanderDir_Vertical = UnityEngine.Random.Range(-1, 2);
                print(wanderDir_Vertical);
                wanderTimer = UnityEngine.Random.Range(wanderDelay_LowerBound, wanderDelay_UpperBound);
            }
            else 
            {
                wanderTimer -= Time.deltaTime;
            }

            // If velocity is 0, pick a random direction to go in, otherwise, continue in the previous direction.
            if (rb.velocity.x == 0f)
            {
                dir_Horizontal = (int)Mathf.Sign(UnityEngine.Random.Range(-1, 1));
            }
            else
            {
                dir_Horizontal = (int)Mathf.Sign(rb.velocity.x);
            }

            // Calculate Potential Velocity
            Vector2 deltaV =
                new Vector2(dir_Horizontal, Mathf.Clamp((isGrounded ? 1 : 0) + wanderDir_Vertical, -1, 1)) * // get the wander direction
                (isGrounded ? groundAccelleration : airAccelleration) * // multiply it by the relevant acceleration
                Time.deltaTime; // account for time

            // Horizontal
            if (Mathf.Abs(rb.velocity.x + deltaV.x) < wanderSpeedLimits.x || (int)Mathf.Sign(rb.velocity.x * deltaV.x) == -1)
                newVelocity += Vector2.right * (rb.velocity.x + deltaV.x);
            else
                newVelocity += Vector2.right * dir_Horizontal * wanderSpeedLimits.x;

            // Vertical
            if (Mathf.Abs(rb.velocity.y + deltaV.y) < wanderSpeedLimits.y || (int)Mathf.Sign(rb.velocity.y * deltaV.y) == -1)
                newVelocity += Vector2.up * (rb.velocity.y + deltaV.y);
            else
                newVelocity += Vector2.up * wanderDir_Vertical * wanderSpeedLimits.y; // TODO:: need to consider vertical collisions.

            // Set Velocity
            print (newVelocity);
            rb.velocity = newVelocity;
        }
        
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
