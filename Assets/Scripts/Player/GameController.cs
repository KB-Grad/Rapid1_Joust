using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [SerializeField] int lives;
    [SerializeField] int score;
    [SerializeField] int enemyValue;
    [SerializeField] GameObject scoreText;
    Transform[] healthBar = new Transform[5];
    Vector3[] rebornPos = new Vector3[3];
    Rigidbody2D rb;
    public GameObject RespawnPlatform;
    public float delayInSeconds;
    // Start is called before the first frame update
    void Start()
    {
        start();
    }

    // Update is called once per frame
    void Update()
    {
        //scoreBoard();
    }

    void start()
    {
        lives = 5;
        score = 0;
        rebornPos[0] = new Vector3(-0.249f, 0.102f, 0);
        rebornPos[1] = new Vector3(-0.572f, 1.207f, 0);
        rebornPos[2] = new Vector3(0.738f, 0.708f, 0);
        //rebornPos[3] = new Vector3(-2, -1, 0);
        rb = this.GetComponent<Rigidbody2D>();
        for (int i = 0; i < lives; i++)
        {
            healthBar[i] = GameObject.Find((i+1).ToString() + "_Lives").transform;
            healthBar[i].gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void reborn()
    {
        //Animate here
        transform.position = rebornPos[Random.Range(0,3)];
        GameObject node = Instantiate(RespawnPlatform, transform.position,Quaternion.identity);
        StartCoroutine(DelayToDeleteRespawn(delayInSeconds, node));


    }

    IEnumerator DelayToDeleteRespawn(float delayInSeconds, GameObject node)
    {
        yield return new WaitForSeconds(delayInSeconds);

        // delete respawn point
        GameObject.Destroy(node, 0.2f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //score
        if (collision.collider.tag == "Up")
        {
            score += enemyValue;
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
