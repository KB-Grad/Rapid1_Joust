using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using DG.Tweening;

public class Particle_Enemy : MonoBehaviour
{
    GameObject MyEnemy;
    GameObject model, down;
    new ParticleSystem particleSystem;
    bool died = false;
    // Start is called before the first frame update
    void Start()
    {
        MyEnemy = transform.parent.gameObject;
        model = MyEnemy.GetComponent<Transform>().Find("Model").gameObject;
        down = MyEnemy.GetComponent<Transform>().Find("Down").gameObject;

        particleSystem = MyEnemy.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particleSystem!= null) {
            if (!particleSystem.isPlaying && died)
            {
                DestroyImmediate(MyEnemy);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            particleSystem.Play();
            died = true;
            Destroy(model);
            Destroy(down);
            transform.localScale = Vector3.zero;
        }
    }


}