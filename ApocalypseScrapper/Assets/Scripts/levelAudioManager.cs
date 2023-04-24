using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelAudioManager : MonoBehaviour
{
    public static levelAudioManager instance;

    // audio sources
    [Header("----- Sources -----")]
    public AudioSource musicAudioSource;
    public AudioSource whiteNoiseAudioSource;

    // audio clips (the actual sound)
    [Header("----- Clips -----")]
    public AudioClip musicAudio;
    public AudioClip whiteNoiseAudio;


    [Header("----- Volume -----")]
    [Range(0f, 1.0f)][SerializeField] float musicVolume;
    [Range(0f, 1.0f)][SerializeField] float whiteNoiseVolume;

    [Header("----- Pitch -----")]
    [Range(0f, 3.0f)][SerializeField] float musicPitch;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // creating all audio sources for our level
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        whiteNoiseAudioSource = gameObject.AddComponent<AudioSource>();

        // assigning each audio sources clip (the actual sound that it makes)
        musicAudioSource.clip = musicAudio;
        whiteNoiseAudioSource.clip = whiteNoiseAudio;

        // setting each audio sources components
        musicAudioSource.volume = musicVolume;
        musicAudioSource.pitch = musicPitch;
        musicAudioSource.playOnAwake = true;
        musicAudioSource.loop = true;

        whiteNoiseAudioSource.volume = whiteNoiseVolume;
        whiteNoiseAudioSource.playOnAwake = true;
        whiteNoiseAudioSource.loop = true;

        // playing sounds that should play on awake - for some reason they don't play unless I call their play method
        // even though I set their play on awake to true
        whiteNoiseAudioSource.Play();
        musicAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseAllAudio()
    {
        musicAudioSource.Pause();
        whiteNoiseAudioSource.Pause();
    }

    public void UnpauseAllAudio()
    {
        musicAudioSource.Play();
        whiteNoiseAudioSource.Play();
    }
}
