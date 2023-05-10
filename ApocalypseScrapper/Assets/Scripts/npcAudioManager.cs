using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcAudioManager : MonoBehaviour
{
    public static npcAudioManager instance;

    [Header("----- Audio Sources -----")]
    public AudioSource npcVoiceAudioSource;
    public AudioSource npcFootstepAudioSource;

    [Header("----- Audio Clips -----")]
    public AudioClip[] npcHelpMeLines;
    public AudioClip npcRescuedLines;
    public AudioClip[] npcFootstepAudio;

    [Header("----- Audio Settings -----")]
    [Range(0, 1)] public float npcVoiceAudioSpatialBlend;
    [Range(0, 1)] public float npcVoiceAudioVolume;
    [Range(0, 1)] public float npcFootstepSpatialBlend;
    [Range(0, 1)] public float npcFootstepVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // adding all of the necessary audio sources
        npcVoiceAudioSource = gameObject.AddComponent<AudioSource>();
        npcFootstepAudioSource = gameObject.AddComponent<AudioSource>();

        // audio settings
        npcVoiceAudioSource.spatialBlend = npcVoiceAudioSpatialBlend;
        npcFootstepAudioSource.spatialBlend = npcFootstepSpatialBlend;
    }

    public void PauseAllAudio()
    {
        npcFootstepAudioSource.Pause();
    }
}
