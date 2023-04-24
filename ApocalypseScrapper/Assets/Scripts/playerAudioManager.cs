using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAudioManager : MonoBehaviour
{
    public static playerAudioManager instance;

    public AudioSource jetpackAudioSource;
    public AudioSource gunAudioSource;

    public AudioClip jetpackThrustAudio;
    public AudioClip gunShotAudio;


    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        jetpackAudioSource = gameObject.AddComponent<AudioSource>();
        gunAudioSource = gameObject.AddComponent<AudioSource>();

        jetpackAudioSource.clip = jetpackThrustAudio;
        gunAudioSource.clip = gunShotAudio;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
