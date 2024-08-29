using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CustomInputs input = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb = null;
    public float moveSpeed = 0;
    private int rightClickCount = 1;
    private int leftClickCount = 1;
    public float[] speeds = {0, 5, 10, 15, 20};
    public float jumpforce = 10;
    private bool breaks = false;
    public float breakSpeed = 10;
    public bool isCollision = false;


    private void Awake()
    {
        input = new CustomInputs();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancled;
        //input.Player.Gravity.performed += OnGravityPerformed;
        input.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancled;
        input.Player.Jump.performed -= OnJumpPerformed;
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); 
        rb.AddForce(new Vector2(0, jumpforce), ForceMode2D.Impulse);
    }

    private void slowSpeed()
    {
        if (isCollision)
        {
            moveVector.x = -moveVector.x;
            //Debug.Log(moveVector);
            isCollision = false;
        }

        moveSpeed = Mathf.Clamp(moveSpeed - breakSpeed * Time.deltaTime, 0, 20);
        Vector2 horizontalMovement = new Vector2(-moveVector.x, 0);

        rb.velocity = new Vector2(horizontalMovement.x * moveSpeed, rb.velocity.y);
        //Debug.Log(rb.velocity);
        if (moveSpeed <= 0)
        {
            breaks = false;
        }
    }
    private IEnumerator SlowDownCoroutine()
    {
        breaks = true;
        while (breaks)
        {
            slowSpeed();
            yield return null; // Wait for the next frame
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        //this section handles the speed change
        moveVector = value.ReadValue<Vector2>();
        if (!breaks)
        {
            if (moveVector.x > 0 && rightClickCount <= speeds.Length - 1)
            {
                rightClickCount++;


                if (rightClickCount == 1)
                {
                    //moveSpeed = 0;
                    breaks = true;
                    StartCoroutine(SlowDownCoroutine());

                    rightClickCount = 1;
                    leftClickCount = 1;
                }
                if (rightClickCount > 1)
                {
                    moveSpeed = speeds[rightClickCount - 1];
                    leftClickCount = 0;
                }


            }
            if (moveVector.x < 0 && leftClickCount <= speeds.Length - 1)
            {

                leftClickCount++;
                if (leftClickCount == 1)
                {
                    //moveSpeed = 0;
                    breaks = true;
                    StartCoroutine(SlowDownCoroutine());
                    leftClickCount = 1;
                     rightClickCount = 1;
                }
                if (leftClickCount > 1)
                {
                    moveSpeed = speeds[leftClickCount - 1];
                    rightClickCount = 0;
                }

            }
        }

    }

    // Change this to make it slide and not to stop
    private void OnMovementCancled(InputAction.CallbackContext value)
    {
        
        //moveVector = Vector2.zero; 
    }

    private void FixedUpdate()
    {
        if (!breaks)
        {
            Vector2 horizontalMovement = new Vector2(moveVector.x, 0);
            rb.velocity = new Vector2(horizontalMovement.x * moveSpeed, rb.velocity.y);
        }

    }

    private void OnGravityPerformed(InputAction.CallbackContext value)
    {
        rb.gravityScale = - rb.gravityScale;
        transform.Rotate(new Vector3(0, 0, 180));
    }

    //Trying bouncy for the player where the player on collsion will start to move in oppsite direction at the same speed as collsion.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Box")
        {
            if(breaks)
            {
                isCollision = true;
            }

            if (rightClickCount > 1)
            {
                moveVector.x = -moveVector.x;
                //moveSpeed =-1*moveSpeed;
                leftClickCount = rightClickCount;
                rightClickCount = 0;
            }
            else if (leftClickCount > 1)
            {
                moveVector.x = -moveVector.x;
                //moveSpeed = -1 * moveSpeed;
                rightClickCount = leftClickCount;
                leftClickCount = 0;
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
