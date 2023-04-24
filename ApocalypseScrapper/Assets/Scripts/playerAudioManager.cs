using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAudioManager : MonoBehaviour
{
    public static playerAudioManager instance;

    // audio sources
    [Header("----- Sources -----")]
    public AudioSource jetpackAudioSource;
    public AudioSource gunAudioSource;
    public AudioSource salvagingAudioSource;
    public AudioSource objectSalvagedAudioSource;
    public AudioSource jetpackPowerDownAudioSource;

    // audio clips (the actual sound)
    [Header("----- Clips -----")]
    public AudioClip jetpackThrustAudio;
    public AudioClip gunShotAudio;
    public AudioClip salvagingAudio;
    public AudioClip objectSalvagedAudio;
    public AudioClip jetpackPowerDownAudio;

    [Header("----- Volume -----")]
    [Range(0f, 1.0f)][SerializeField] float jetpackThrustVolume;
    [Range(0f, 1.0f)][SerializeField] float gunAudioVolume;
    [Range(0f, 1.0f)][SerializeField] float salvagingAudioVolume;
    [Range(0f, 1.0f)][SerializeField] float objectSalvagedVolume;
    [Range(0f, 1.0f)][SerializeField] float jetpackPowerDownVolume;

    [Header("----- Pitch -----")]
    [Range(0f, 3.0f)][SerializeField] float jetpackThrustPitch;
    [Range(0f, 3.0f)][SerializeField] float gunAudioPitch;
    [Range(0f, 3.0f)][SerializeField] float salvagingAudioPitch;
    [Range(0f, 3.0f)][SerializeField] float objectSalvagedPitch;
    [Range(0f, 3.0f)][SerializeField] float jetpackPowerDownPitch;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // creating all audio sources for our player
        jetpackAudioSource = gameObject.AddComponent<AudioSource>();
        gunAudioSource = gameObject.AddComponent<AudioSource>();
        salvagingAudioSource = gameObject.AddComponent<AudioSource>();
        objectSalvagedAudioSource = gameObject.AddComponent<AudioSource>();
        jetpackPowerDownAudioSource = gameObject.AddComponent<AudioSource>();

        // assigning each audio sources clip (the actual sound that it makes)
        jetpackAudioSource.clip = jetpackThrustAudio;
        gunAudioSource.clip = gunShotAudio;
        salvagingAudioSource.clip = salvagingAudio;
        objectSalvagedAudioSource.clip = objectSalvagedAudio;
        jetpackPowerDownAudioSource.clip = jetpackPowerDownAudio;

        // setting each audio sources components
        jetpackAudioSource.playOnAwake = false;
        jetpackAudioSource.loop = true;
        jetpackAudioSource.volume = jetpackThrustVolume;
        jetpackAudioSource.pitch = jetpackThrustPitch;

        gunAudioSource.playOnAwake = false;
        gunAudioSource.volume = gunAudioVolume;
        gunAudioSource.pitch = gunAudioPitch;

        salvagingAudioSource.playOnAwake = false;
        salvagingAudioSource.loop = true;
        salvagingAudioSource.volume = salvagingAudioVolume;
        salvagingAudioSource.pitch = salvagingAudioPitch;

        objectSalvagedAudioSource.playOnAwake = false;
        objectSalvagedAudioSource.loop = false;
        objectSalvagedAudioSource.volume = objectSalvagedVolume;
        objectSalvagedAudioSource.pitch = objectSalvagedPitch;

        jetpackPowerDownAudioSource.playOnAwake = false;
        jetpackPowerDownAudioSource.loop = false;
        jetpackPowerDownAudioSource.volume = jetpackPowerDownVolume;
        jetpackPowerDownAudioSource.pitch = jetpackPowerDownPitch;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
