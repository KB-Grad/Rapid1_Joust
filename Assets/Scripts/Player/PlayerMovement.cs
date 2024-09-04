using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Animator anim;
    private CustomInputs input = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D rb = null;

    public float moveSpeed = 0;
    public int rightClickCount = 1;
    public int leftClickCount = 1;
    public float[] speeds = {0, 5, 10, 15, 20};
    public float jumpforce = 10;
    private bool breaks = false;
    public float breakSpeed = 10;
    public bool isCollision = false;

    public float bounceForce = 5;

    public int gTimer = 0;
    private float timeElapsed = 0f;

    private Coroutine speedCoroutine;
    private Coroutine slowDownCoroutine;


    private void Awake()
    {
        input = new CustomInputs();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.started += OnMovementStarted;
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancled;
        //input.Player.Gravity.performed += OnGravityPerformed;
        input.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.started -= OnMovementStarted;
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancled;
        input.Player.Jump.performed -= OnJumpPerformed;
    }

    private void OnMovementStarted(InputAction.CallbackContext value)
    {
        anim.SetFloat("speed", moveSpeed);
        moveVector = value.ReadValue<Vector2>();

        if (!breaks)
        {

            if (moveVector.x > 0&&rightClickCount<speeds.Length)
            {
                rightClickCount++;
                if (rightClickCount == 1)
                {
                    breaks = true;

                    slowDownCoroutine = StartCoroutine(SlowDownCoroutine());
                    leftClickCount = 1;
                }
                else
                {
                    moveSpeed = speeds[rightClickCount - 1];
                    leftClickCount = 0;
                }
            }
            if (moveVector.x < 0&&leftClickCount<speeds.Length)
            {
                leftClickCount++;
                if (leftClickCount == 1)
                {
                    breaks = true;

                    slowDownCoroutine = StartCoroutine(SlowDownCoroutine());
                    rightClickCount = 1;
                }
                else
                {
                    moveSpeed = speeds[leftClickCount - 1];
                    rightClickCount = 0;
                   
                }
            }
        }

    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
        if (breaks)
        {
            if (speedCoroutine != null)
            {
                StopCoroutine(speedCoroutine);
                speedCoroutine = null;
            }
            StartCoroutine(waitForStop());
        }
        else
        {
            if (speedCoroutine != null)
            {
                StopCoroutine(speedCoroutine);
            }
            speedCoroutine = StartCoroutine(SpeedOverTime());
        }

    }


    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if (rb.gravityScale > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpforce), ForceMode2D.Impulse);
        }
        else if(rb.gravityScale < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, -jumpforce), ForceMode2D.Impulse);
        }

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
            moveSpeed = 0;
            breaks = false;
        }
    }
    private IEnumerator SlowDownCoroutine()
    {
        while (breaks)
        {
            slowSpeed();
            yield return null;
        }
    }
    private IEnumerator waitForStop()
    {
        while (moveSpeed > 0)
        {
            yield return null;
        }
        speedCoroutine = StartCoroutine(SpeedOverTime());
    }



    private IEnumerator SpeedOverTime()
    {

        breaks = false;
        moveSpeed = 0;
        while (!breaks)
        {
            if (moveVector.x > 0 && rightClickCount < speeds.Length)
            {

                rightClickCount++;
                if (rightClickCount == 1)
                {
                    rightClickCount++;
                    
                    moveSpeed = speeds[rightClickCount - 1];
                    leftClickCount = 1;


                }
                else
                {
                    moveSpeed = speeds[rightClickCount - 1];
                    leftClickCount = 0;
                }
            }
            else if (moveVector.x < 0 && leftClickCount < speeds.Length)
            {
                leftClickCount++;
                if (leftClickCount == 1)
                {
                    leftClickCount++;
                    moveSpeed= speeds[leftClickCount - 1];

                    rightClickCount = 1;

                }
                else
                {
                    moveSpeed = speeds[leftClickCount - 1];
                    rightClickCount = 0;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Change this to make it slide and not to stop
    private void OnMovementCancled(InputAction.CallbackContext value)
    {

        //moveVector = Vector2.zero; 
        if(speedCoroutine!=null)
        {
            StopCoroutine(speedCoroutine);
            speedCoroutine = null;
        }
    }

    private void FixedUpdate()
    {   
        if (!breaks)
        {
            Vector2 horizontalMovement = new Vector2(moveVector.x, 0);
            rb.velocity = new Vector2(horizontalMovement.x * moveSpeed, rb.velocity.y);
        }
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= 1f)
        {
            if (gTimer < 6)
            {
                gTimer++;
                timeElapsed = 0f;
            }
        }
    }

    private void OnGravityPerformed(InputAction.CallbackContext value)
    {
        rb.gravityScale = - rb.gravityScale;
        transform.Rotate(new Vector3(0, 0, 180));
    }

    private void gravitySwitch()
    {
        if(gTimer ==6)
        {
            rb.gravityScale = -rb.gravityScale;
            transform.Rotate(new Vector3(0, 0, 180));
            gTimer = 0;
        }
    }

    //Trying bouncy for the player where the player on collsion will start to move in oppsite direction at the same speed as collsion.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Box" || collision.gameObject.tag == "Ground")
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
        if (collision.gameObject.tag == "Enemy" && (collision.gameObject.name== "Enemy 1"|| collision.gameObject.name == "Enemy 1(Clone)"))
        {

            float killTreshold=collision.gameObject.GetComponent<EnemyMovement>().killThreshold;
            float colLocation = (transform.position.y - collision.gameObject.transform.position.y);
            if (colLocation > killTreshold)
            {
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
                if (rb.gravityScale > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(0, bounceForce), ForceMode2D.Impulse);

                }
                else if (rb.gravityScale < 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(0, -bounceForce), ForceMode2D.Impulse);
                }
            }
            if (colLocation < killTreshold && colLocation > -killTreshold)
            {
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
    }

    public void resetPlayer()
    {
        moveVector = Vector3.zero;
        moveSpeed = 0;
        leftClickCount = 1;
        rightClickCount = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "GSwitch")
        {
            //Debug.Log("g");
            gravitySwitch();
        }
    }
    
    private void gSitchWait()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
