using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParent : MonoBehaviour
{
    Vector3 pos;
    private void Start()
    {
        pos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = pos;
    }
}
