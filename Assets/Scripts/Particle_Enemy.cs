using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class Particle_Enemy : MonoBehaviour
{
    public GameObject MyEnemy;
    public ParticleSystem ParticleSystem;
    private Material _material;
    // Start is called before the first frame update
    void Start()
    {
        _material = MyEnemy.GetComponent<Renderer>().material;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Die();
        }
    }

    private void Die()
    {
        //var s = DOTween.Sequence();
        //s.Append(_material.DOFloat(30, "_Strength", 0.2f));
        //s.AppendInterval(0.4f);
        //s.AppendCallback(() =>
        //{
            GameObject.Destroy(MyEnemy);
            ParticleSystem.Play();
            //s.Append(_material.DOFade(30, "white", 0.2f));

        //});


    }
}