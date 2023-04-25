using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crabAudioManager : MonoBehaviour
{

    public static crabAudioManager instance;
    [Header("-----Bite Sound-----")]
    public AudioSource crabBiteAudioSource;
    public AudioClip crabBiteClip;
    [Header("-----Walk Sound-----")]
    public AudioSource crabWalkAudioSource;
    public AudioClip[] crabWalkClips;
    [Header("-----Run Sound-----")]
    public AudioSource crabRunAudioSource;
    public AudioClip crabRunClip;

    [Header("-----Spit Sound-----")]
    public AudioSource crabSpitAudioSource;
    public AudioClip crabSpitClip;


    [Header("-----Sound Volume-----")]
    [Range(0f, 1.0f)][SerializeField] float crabBiteVolume;
    [Range(0f, 1.0f)][SerializeField] float crabRunVolume;
    [Range(0f, 1.0f)][SerializeField] float crabSpitVolume;
    [Range(0f, 1.0f)][SerializeField] float crabWalkVolume;

    [Header("-----Sound Pitch-----")]
    [Range(0f, 1.0f)][SerializeField] float crabBitePitch;
    [Range(0f, 1.0f)][SerializeField] float crabRunPitch;
    [Range(0f, 1.0f)][SerializeField] float crabSpitPitch;
    [Range(0f, 1.0f)][SerializeField] float crabWalkPitch;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //crab Bite Audio
        crabBiteAudioSource=gameObject.AddComponent<AudioSource>();
        crabBiteAudioSource.clip = crabBiteClip;
        crabBiteAudioSource.spatialBlend = 1;
        //crab Run Audio
        crabRunAudioSource = gameObject.AddComponent<AudioSource>();
        crabRunAudioSource.clip = crabRunClip;
        crabRunAudioSource.spatialBlend = 1;
        // crab Boss Spit Audio
        if (crabSpitAudioSource&&crabSpitClip)
        {
            crabRunAudioSource = gameObject.AddComponent<AudioSource>();
            crabRunAudioSource.clip = crabRunClip;
            crabRunAudioSource.spatialBlend = 1;
        }
        //Crab Walk Audio
        crabWalkAudioSource = gameObject.AddComponent<AudioSource>();
        
        crabWalkAudioSource.spatialBlend = 1;
        //setting bite components
        crabBiteAudioSource.playOnAwake = false;
        crabBiteAudioSource.loop = true;
        crabBiteAudioSource.volume = crabBiteVolume;
        crabBiteAudioSource.pitch = crabBitePitch;
        //setting run components
        crabBiteAudioSource.playOnAwake = false;
        crabBiteAudioSource.loop = true;
        crabBiteAudioSource.volume = crabBiteVolume;
        crabBiteAudioSource.pitch = crabBitePitch;
        //setting Spit components
        if (crabSpitAudioSource && crabSpitClip)
        {
            crabBiteAudioSource.playOnAwake = false;
            crabBiteAudioSource.loop = true;
            crabBiteAudioSource.volume = crabBiteVolume;
            crabBiteAudioSource.pitch = crabBitePitch;
        }
        //setting run components
        crabWalkAudioSource.playOnAwake = false;
        crabWalkAudioSource.loop = true;
        crabWalkAudioSource.volume = crabWalkVolume;
        crabWalkAudioSource.pitch = crabWalkPitch;

    }

    
}
