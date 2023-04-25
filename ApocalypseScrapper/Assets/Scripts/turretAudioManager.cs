using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretAudioManager : MonoBehaviour
{
    public static turretAudioManager instance;

    [Header("----- Sources -----")]
    public AudioSource turretShotAudioSource;
    public AudioSource turretDamageAudioSource;

    [Header("----- Clips -----")]
    public AudioClip turretShotAudio;
    public AudioClip turretDamageAudio;

    [Header("----- Volumes -----")]
    [Range(0, 1)] public float turretShotVolume;
    [Range(0, 1)] public float turretDamageVolume;
    
    [Header("----- Spatial Blend -----")]
    [Range(0, 1)] public float turretShotSpatialBlend;
    [Range(0, 1)] public float turretDamageSpatialBlend;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        turretShotAudioSource = gameObject.AddComponent<AudioSource>();
        turretDamageAudioSource = gameObject.AddComponent<AudioSource>();

        turretShotAudioSource.clip = turretShotAudio;
        turretDamageAudioSource.clip = turretDamageAudio;

        turretShotAudioSource.spatialBlend = turretShotSpatialBlend;
        turretDamageAudioSource.spatialBlend = turretDamageSpatialBlend;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
