using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer _renderer;
    public Sprite CursorAnime;
    Animator Animator;

    private void Awake()
    {
        Cursor.visible = false;
    }
    void Start()
    {
        //Cursor.visible = false;
        _renderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        _renderer.sprite = CursorAnime;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;
        if (Input.GetMouseButtonDown(0))
        {
            Animator.SetBool("Click", true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Animator.SetBool("Click", false);
        }


    }
}
