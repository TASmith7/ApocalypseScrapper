using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelAudioManager : MonoBehaviour
{
    public static levelAudioManager instance;

    // audio sources
    [Header("----- Sources -----")]
    public AudioSource musicAudioSource;
    public AudioSource whiteNoiseAudioSource;
    public AudioSource pauseMenuAudioSource;
    public AudioSource voiceOverAudioSource;
    public AudioSource elevatorAudioSource;

    // audio clips (the actual sound)
    [Header("----- Clips -----")]
    public AudioClip lvl1MusicAudio;
    public AudioClip lvl2MusicAudio;
    public AudioClip lvl3MusicAudio;
    public AudioClip bossLvlMusicAudio;
    public AudioClip whiteNoiseAudio;
    public AudioClip pauseMenuAudio;
    public AudioClip VOIntro;
    public AudioClip VOPlayerDead;
    public AudioClip VOLvl2Intro;
    public AudioClip VOLvl3Intro;
    public AudioClip VOBossLvlIntro;
    public AudioClip VOFinishWithS;
    public AudioClip VOFinishWithA;
    public AudioClip VOFinishWithB;
    public AudioClip VOFinishWithC;
    public AudioClip VOFinishWithD;
    public AudioClip VOFinishWithF;
    public AudioClip VOKillBoss;
    public AudioClip VOFloorPass;
    public AudioClip VOFloorFail;
    public AudioClip VONextLevel;
    public AudioClip VOBonusSpendIt;
    public AudioClip VOStoreTutorial;
    public AudioClip elevatorUp;
    public AudioClip elevatorStop;


    [Header("----- Volume -----")]
    [Range(0f, 1.0f)][SerializeField] float musicVolume;
    [Range(0f, 1.0f)][SerializeField] float whiteNoiseVolume;
    [Range(0f, 1.0f)][SerializeField] float pauseMenuVolume;
    [Range(0f, 1.0f)][SerializeField] float voiceOverVolume;
    [Range(0f, 1.0f)][SerializeField] float elevatorVolume;

    [Header("----- Pitch -----")]
    [Range(0f, 3.0f)][SerializeField] float musicPitch;

    bool elevatorWasPlaying;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        // creating all audio sources for our level
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        whiteNoiseAudioSource = gameObject.AddComponent<AudioSource>();
        pauseMenuAudioSource = gameObject.AddComponent<AudioSource>();
        voiceOverAudioSource = gameObject.AddComponent<AudioSource>();
        if (gameManager.instance.currentScene == SceneManager.GetSceneByName("Lvl 1"))
        {
            elevatorAudioSource = gameObject.AddComponent<AudioSource>();
            elevatorAudioSource.volume = elevatorVolume;
        }
        
        // assigning each audio sources clip (the actual sound that it makes)
        whiteNoiseAudioSource.clip = whiteNoiseAudio;
        pauseMenuAudioSource.clip = pauseMenuAudio;

        // setting each audio sources components
        musicAudioSource.volume = gameManager.instance.musicVolume.value / 100;
        musicAudioSource.pitch = musicPitch;
        musicAudioSource.playOnAwake = true;
        musicAudioSource.loop = true;

        whiteNoiseAudioSource.volume = whiteNoiseVolume;
        whiteNoiseAudioSource.playOnAwake = true;
        whiteNoiseAudioSource.loop = true;

        pauseMenuAudioSource.volume = pauseMenuVolume;
        pauseMenuAudioSource.playOnAwake = false;

        // setting our music based on which scene we're in
        if (gameManager.instance.currentScene == SceneManager.GetSceneByName("Lvl 1"))
        {
            musicAudioSource.clip = lvl1MusicAudio;
        }
        else if (gameManager.instance.currentScene == SceneManager.GetSceneByName("Lvl 2"))
        {
            musicAudioSource.clip = lvl2MusicAudio;

            // if our voice over toggle is on
            if (gameManager.instance.voiceoversToggle.isOn)
            {
                voiceOverAudioSource.PlayOneShot(VOLvl2Intro);

                // if our subtitle toggle is on
                if (gameManager.instance.subtitlesToggle.isOn)
                {
                    StartCoroutine(gameManager.instance.StartSubtitles(subtitleManager.instance.lvl2IntroVoiceLines));
                }
            }
        }
        else if (gameManager.instance.currentScene == SceneManager.GetSceneByName("Lvl 3"))
        {
            musicAudioSource.clip = lvl3MusicAudio;

            // if our voice over toggle is on
            if (gameManager.instance.voiceoversToggle.isOn)
            {
                voiceOverAudioSource.PlayOneShot(VOLvl3Intro);

                // if our subtitle toggle is on
                if (gameManager.instance.subtitlesToggle.isOn)
                {
                    StartCoroutine(gameManager.instance.StartSubtitles(subtitleManager.instance.lvl3IntroVoiceLines));
                }
            }
        }
        else if (gameManager.instance.currentScene == SceneManager.GetSceneByName("Boss Lvl"))
        {
            musicAudioSource.clip = bossLvlMusicAudio;

            // if our voice over toggle is on
            if (gameManager.instance.voiceoversToggle.isOn)
            {
                voiceOverAudioSource.PlayOneShot(VOBossLvlIntro);

                // if our subtitle toggle is on
                if (gameManager.instance.subtitlesToggle.isOn)
                {
                    StartCoroutine(gameManager.instance.StartSubtitles(subtitleManager.instance.bossLvlIntroVoiceLines));
                }
            }
        }

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

        if(elevatorAudioSource.isPlaying)
        {
            elevatorWasPlaying = true;
            elevatorAudioSource.Pause();
        }
    }

    public void UnpauseAllAudio()
    {
        musicAudioSource.Play();
        whiteNoiseAudioSource.Play();

        if(elevatorWasPlaying)
        {
            elevatorAudioSource.UnPause();
        }
    }

   
}
