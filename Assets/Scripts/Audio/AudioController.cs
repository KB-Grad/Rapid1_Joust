using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("------ Audio Source ------")] 
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    [Header ("------ Audio Clip ------")]
    public AudioClip gravitySwitch;
    public AudioClip enemyCollide;
    public AudioClip enemySpawn;
   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
