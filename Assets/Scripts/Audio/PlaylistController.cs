using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaylistController : MonoBehaviour
{
    private AudioSource _musicplaylist;
    public AudioClip[] songs;
    public float volume;

    [SerializeField] private float trackTimer; 
    [SerializeField] private float songsPlayed;
    [SerializeField] private bool[] beenPlayed; 
    void Start()
    {
        _musicplaylist = GetComponent<AudioSource>();
        beenPlayed = new bool[songs.Length];

        if (!_musicplaylist.isPlaying)
        {
            ChangeSong(Random.Range(0, songs.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Sets volume
        _musicplaylist.volume = volume;

        if(_musicplaylist.isPlaying )
        {
            trackTimer += 1 * Time.deltaTime; 
        }

        //Checks if Playing to start
        if(!_musicplaylist.isPlaying || trackTimer >= _musicplaylist.clip.length || Input.GetKeyDown(KeyCode.P))
        {
            ChangeSong(Random.Range(0, songs.Length));
        }

        if (songsPlayed == songs.Length)
        {
            songsPlayed = 0;
            for (int i = 0; i < songs.Length; i++)
            {
                if(i== songs.Length)
                {
                    break;
                }
                else
                {
                    beenPlayed[i] = false;
                }
            }

        }
    }

    public void ChangeSong(int songPicked)
    {
        if (!beenPlayed[songPicked])
        {
            trackTimer = 0;
            songsPlayed++;
            beenPlayed[songPicked] = true;
            _musicplaylist.clip = songs[songPicked];
            _musicplaylist.Play();
        } else {
            _musicplaylist.Stop();         
        }
    }
}
