using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] int lives;
    [SerializeField] float score;
    int enemyValue = 1;
    [SerializeField] TMP_Text scoreText;
    Transform[] healthBar = new Transform[5];
    [SerializeField] Transform[] rebornPos;
    Rigidbody2D rb;
    GameObject[] newEnemies = new GameObject[4];
    GameObject[] enemyParent = new GameObject[2];
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
        scoreBoard();
    }

    void start()
    {
        lives = 5;
        score = 0;
        enemyParent[0] = GameObject.Find("EnemyParent1");
        enemyParent[1] = GameObject.Find("EnemyParent2");
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
        PlayerPrefs.SetFloat("Score", score);
    }

    void reborn()
    {
        transform.position = rebornPos[Random.Range(0,rebornPos.Length)].position;
        GetComponent<PlayerMovement>().resetPlayer();
        //add platform respawn
        // GameObject node = Instantiate(RespawnPlatform, transform.position, Quaternion.identity);
        // StartCoroutine(DelayToDeleteRespawn(delayInSeconds, node));
    }

    IEnumerator DelayToDeleteRespawn(float delayInSeconds, GameObject node)
    {
        yield return new WaitForSeconds(delayInSeconds);

        // delete respawn point
        GameObject.Destroy(node, 0.2f);
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
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
