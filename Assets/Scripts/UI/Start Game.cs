using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartGame : MonoBehaviour
{
    public TMP_Text score;

    public void Start()
    {
        score.text = PlayerPrefs.GetFloat("Score").ToString("00000000");
    }

    public void StartMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void ReStartMenu()
    {
        SceneManager.LoadScene(0);
        print("Restart");
    }
}