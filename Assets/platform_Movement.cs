using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_Movement : MonoBehaviour
{

    public Vector3 startPos;
    public Vector3 endPos;
    Rigidbody2D rb;
    public ContactFilter2D contactFilter;
    public float speed;
    ContactPoint2D[] contactPoint = new ContactPoint2D[10];
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    public void FollowObjects()
    {
        //get object on platform
        int count = rb.GetContacts(contactFilter, contactPoint);
        for (int i = 0; i < count; i++)
        {
            contactPoint[i].rigidbody.velocity += new Vector2(transform.position.x < endPos.x ? speed : -speed,0);

        }


    }

    private void LateUpdate()
    {
        FollowObjects();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
        if (transform.position == endPos)
        {
            Vector3 temp = endPos;
            this.endPos = this.startPos;
            this.startPos = temp;
        }
    }
}
