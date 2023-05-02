using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitDetection : MonoBehaviour
{
    [SerializeField] float FXtime;
    [SerializeField] ParticleSystem[] bloodEffect;
    [SerializeField] Renderer model;


    bool playerInRange;
    void Start()
    {
        for (int i = 0;i < bloodEffect.Length; i++)
        { bloodEffect[i].Play(); }
        //bloodEffect.Initialize();
    }

    void Update()
    {
        if (!playerInRange)
        {
            for (int i = 0; i < bloodEffect.Length; i++)
            {
                bloodEffect[i].Stop();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerBullet"))
        {
            for (int i = 0; i < bloodEffect.Length; i++)
            {
                bloodEffect[i].Play();
            }
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PlayerBullet"))
        {
            StartCoroutine(WaitForFX());
        }
    }

    IEnumerator WaitForFX()
    {

        yield return new WaitForSeconds(FXtime);
        playerInRange= false;
    }
}
