using System.Collections;
using System.Collections.Generic;
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
        input.Player.Gravity.performed += OnGravityPerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {

        moveVector = value.ReadValue<Vector2>();
        if (moveVector.x > 0 && rightClickCount <= speeds.Length-1 )
        {
            rightClickCount++;
            if (rightClickCount == 1)
            {
                moveSpeed = 0;
                rightClickCount = 1;
                leftClickCount = 1;
            } 
            if (rightClickCount > 1)
            {
                moveSpeed = speeds[rightClickCount - 1];
                leftClickCount = 0;
            }   
            
            
        }
        if (moveVector.x < 0 && leftClickCount <= speeds.Length-1)
        {
            leftClickCount++;
            if (leftClickCount == 1)
            {
                moveSpeed = 0;
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

    // Change this to make it slide
    private void OnMovementCancled(InputAction.CallbackContext value)
    {
        
        //moveVector = Vector2.zero; 
    }

    private void FixedUpdate()
    {
        Vector2 horizontalMovement = new Vector2(moveVector.x, 0);
        rb.velocity = horizontalMovement * moveSpeed;
    }

    private void OnGravityPerformed(InputAction.CallbackContext value)
    {
        rb.gravityScale = - rb.gravityScale;
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
