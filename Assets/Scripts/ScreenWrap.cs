using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ScreenWrap : MonoBehaviour
{

    private Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {

   



        myRigidbody = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Camera.main.GetComponent<PixelPerfectCamera>())
        {
            PixelPerfectCamera pp = Camera.main.GetComponent<PixelPerfectCamera>();
            // Bounds of the pixel perfect camera
            float rightBound = (pp.refResolutionX / pp.assetsPPU) / 2f;
            float leftBound = -rightBound;

            if (transform.position.x <= leftBound && myRigidbody.velocity.x < 0)
            {
                transform.position = new Vector2(rightBound, transform.position.y);
            }
            else if (transform.position.x >= rightBound && myRigidbody.velocity.x > 0)
            {
                transform.position = new Vector2(leftBound, transform.position.y);
            }
        }
        else 
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            float rightOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
            float leftOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;

            if (screenPos.x <= 0 && myRigidbody.velocity.x < 0)
            {
                transform.position = new Vector2(rightOfScreen, transform.position.y);
            }
            else if (screenPos.x >= Screen.width && myRigidbody.velocity.x > 0)
            {
                transform.position = new Vector2(leftOfScreen, transform.position.y);
            }
        }
    }
}
