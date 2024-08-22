using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [SerializeField] int lives;
    [SerializeField] int score;
    [SerializeField] int enemyValue;
    [SerializeField] GameObject scoreText;
    Vector3[] rebornPos = new Vector3[4];
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
        rebornPos[0] = new Vector3(2, 3, 0);
        rebornPos[1] = new Vector3(-2, 3,0);
        rebornPos[2] = new Vector3(2, -1, 0);
        rebornPos[3] = new Vector3(-2, -1, 0);
        rb = this.GetComponent<Rigidbody2D>();
    }

    void scoreBoard()
    {
        scoreText.GetComponent<Text>().text = "score:" + score.ToString();
    }
    void reborn()
    {
        //Animate here
        transform.position = rebornPos[Random.Range(0,4)];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //score
        if (other.tag == "Up")
        {
            score += enemyValue;
        }
        //player spawn

        if(other.tag == "Down")
        {
            lives--;
            if (lives > 0)
            {
                reborn();
            }
        }
    }
}
