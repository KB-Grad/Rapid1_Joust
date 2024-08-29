using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{


    [SerializeField] int lives;
    [SerializeField] float score;
    int enemyValue = 1;
    [SerializeField] TMP_Text scoreText;
    Transform[] healthBar = new Transform[5];
    Vector3[] rebornPos = new Vector3[3];
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        start();
    }

    // Update is called once per frame
    void Update()
    {
        scoreBoard();
    }

    void start()
    {
        lives = 5;
        score = 0;
        rebornPos[0] = new Vector3(-1.3f, 0.14f, 0);
        rebornPos[1] = new Vector3(0.07f, 0.4f,0);
        rebornPos[2] = new Vector3(1.33f, -0.18f, 0);
        //rebornPos[3] = new Vector3(-2, -1, 0);
        rb = this.GetComponent<Rigidbody2D>();
        for (int i = 0; i < lives; i++)
        {
            healthBar[i] = GameObject.Find((i+1).ToString() + "_Lives").transform;
            healthBar[i].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void scoreBoard()
    {
        print("score:" + score);
        print("scoreText:" + scoreText);
        scoreText.text = score.ToString();
    }

    void reborn()
    {
        transform.position = rebornPos[Random.Range(0,3)];
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //score
        if (collision.collider.tag == "Up")
        {
            score += collision.gameObject.GetComponentInParent<EnemyMovement>().Die();
            print("up");
        }
        //player spawn

        if (collision.collider.tag == "Down")
        {
            lives--;
            print("down");
            healthBar[lives].gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
            if (lives > 0)
            {
                reborn();
            }
        }
    }



}
