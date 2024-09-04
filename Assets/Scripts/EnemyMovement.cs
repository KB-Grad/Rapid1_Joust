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
    ParticleSystem ps;
    GameObject model;
    Rigidbody2D rb;
    GameController gc;

    [Header("State Tracking")]
    [SerializeField] bool isSpawning = true;
    [SerializeField] bool isGrounded = true;
    [SerializeField][Range(-1, 1)] int wanderDir_Vertical = 0;

    [Header("Characteristics")]
    [SerializeField] private EnemyState currentState = EnemyState.WANDER;
    [SerializeField] public float scoreValue = 500;
    [SerializeField] Vector2 groundAccelleration = new Vector2(1, 1);
    [SerializeField] Vector2 airAccelleration = new Vector2(.5f, 1);
    [SerializeField] Vector2 wanderSpeedLimits = new Vector2(3.0f, 1.0f);
    [SerializeField] Vector2 chaseSpeedLimits = new Vector2(5.0f, 1.0f);
    [SerializeField] float detectionAngle = 15f; // The max angle, in degrees, off of horizontal the player can be seen by this enemy
    [SerializeField] float detectionDistance = 5f; // The furthest distance the player can be seen by this enemy
    [SerializeField] float trackingDistance = 15f; // The furthest distance the player can be chased by this enemy
    [SerializeField][Range(0, 30)] float wanderDelay_UpperBound = 10f;
    [SerializeField][Range(0, 30)] float wanderDelay_LowerBound = 7f;
    float wanderTimer = 0f;
    public float killThreshold = .05f;
    [SerializeField] bool DEBUG_doDie = false;



    //add Floatpoints
    public GameObject floatPoint;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponentInChildren<ParticleSystem>();
        model = transform.Find("Model").gameObject;
        gc = GameObject.FindObjectOfType<GameController>();
        //gc = GameObject.FindAnyObjectByType<GameController>();
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
        
        // State Resolution
        int dir_Horizontal = 0;
        Vector2 newVelocity = Vector2.zero;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerOffset = player.transform.position - transform.position;
        float angleToPlayer = Mathf.Abs(Mathf.Asin(playerOffset.y / playerOffset.magnitude) * Mathf.Rad2Deg);
        switch (currentState) 
        {
            case EnemyState.WANDER: // ----------------------- WANDER STATE -----------------------
                // Update Horizontal Wander Timer
                if (wanderTimer <= 0)
                {
                    wanderDir_Vertical = UnityEngine.Random.Range(-1, 2);
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
                rb.velocity = newVelocity;

                // Check for Chase
                
                if (playerOffset.magnitude <= detectionDistance && 
                    angleToPlayer < detectionAngle && 
                    Mathf.Sign(playerOffset.x) * dir_Horizontal == 1) 
                {
                    currentState = EnemyState.CHASE;
                }
                break;

            case EnemyState.CHASE: // ----------------------- CHASE STATE -----------------------
                dir_Horizontal = (int)Mathf.Sign(player.transform.position.x - transform.position.x);
                int chaseDir_Vertical = (int)Mathf.Sign(player.transform.position.y + (Mathf.Sign(transform.localScale.y) * killThreshold * 2) - transform.position.y);

                deltaV = new Vector2(dir_Horizontal, chaseDir_Vertical) * // get the wander direction
                    (isGrounded ? groundAccelleration : airAccelleration) * // multiply it by the relevant acceleration
                    Time.deltaTime; // account for time

                // Horizontal
                if (Mathf.Abs(rb.velocity.x + deltaV.x) < chaseSpeedLimits.x || (int)Mathf.Sign(rb.velocity.x * deltaV.x) == -1)
                    newVelocity += Vector2.right * (rb.velocity.x + deltaV.x);
                else
                    newVelocity += Vector2.right * dir_Horizontal * chaseSpeedLimits.x;

                // Vertical
                if (Mathf.Abs(rb.velocity.y + deltaV.y) < chaseSpeedLimits.y || (int)Mathf.Sign(rb.velocity.y * deltaV.y) == -1)
                    newVelocity += Vector2.up * (rb.velocity.y + deltaV.y);
                else
                    newVelocity += Vector2.up * chaseDir_Vertical * chaseSpeedLimits.y; // TODO:: need to consider vertical collisions.

                rb.velocity = newVelocity;

                // Check for Wander
                if (playerOffset.magnitude > trackingDistance) 
                {
                    currentState = EnemyState.WANDER;
                }
                break;
        }        
    }

    public void Die()
    {
        // Add floatpoints
        Instantiate(floatPoint, transform.position,Quaternion.identity);

        // Add Score to GameController
        gc.AddScore(scoreValue);

        // Spawn Egg
        GameObject newEgg = Instantiate(eggPrefab, transform.position, Quaternion.identity);
        newEgg.GetComponent<Rigidbody2D>().velocity = rb.velocity;

        // Cleanup
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if ((collision.gameObject.transform.position.y - transform.position.y) > killThreshold)
            {
                ps.transform.SetParent(transform.parent, true);
                ps.Play();

                Die();
            }
            else if ((collision.gameObject.transform.position.y - transform.position.y) < -killThreshold)
            {
                gc.KillPlayer();
            }
            else if ((int)Mathf.Sign(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x * rb.velocity.x) == -1)
            {
                rb.velocity = Vector2.left * rb.velocity + Vector2.up * rb.velocity;
            }
        }
        else if (collision.gameObject.tag == "Enemy" && 
            Mathf.Sign(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x * rb.velocity.x) == -1)
        {
            rb.velocity = Vector2.left * rb.velocity + Vector2.up * rb.velocity;
        }
    }
}

enum EnemyState 
{
    WANDER,
    CHASE
}
