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
    GameObject[] newEnemies = new GameObject[4];
    GameObject[] enemyParent = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        start();
    }

    // Update is called once per frame
    void Update()
    {
        scoreBoard();
        enemySpawn();
    }

    void start()
    {
        lives = 5;
        score = 0;
        rebornPos[0] = new Vector3(-1.3f, 0.14f, 0);
        rebornPos[1] = new Vector3(0.07f, 0.4f,0);
        rebornPos[2] = new Vector3(1.33f, -0.18f, 0);
        enemyParent[0] = GameObject.Find("EnemyParent1");
        enemyParent[1] = GameObject.Find("EnemyParent2");
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
        scoreText.text = score.ToString();
    }

    void reborn()
    {
        transform.position = rebornPos[Random.Range(0,3)];
    }

    public void AddScore(float newScore) 
    {
        score += newScore;
        print("up");
    }

    public void KillPlayer() 
    {
        lives--;
        print("down");
        healthBar[lives].gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
        if (lives > 0)
        {
            reborn();
        }
    }

    void enemySpawn()
    {
        if(GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            for(int i = 0; i < 4 ; i++)
            {
                newEnemies[i] = Instantiate(enemyParent[Random.Range(0, 2)], rebornPos[i], Quaternion.identity);
                newEnemies[i].gameObject.tag = "Enemy";
            }
        }
    } 

    /*
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
    */
}
