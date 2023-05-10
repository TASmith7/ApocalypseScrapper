using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subtitleManager : MonoBehaviour
{
    public static subtitleManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [System.Serializable]
    public struct VoiceLine
    {
        public string text;
        public float time;
    }

    public VoiceLine[] lvl1IntroVoiceLines;
    public VoiceLine[] lvl2IntroVoiceLines;
    public VoiceLine[] lvl3IntroVoiceLines;
    public VoiceLine[] bossLvlIntroVoiceLines;
    public VoiceLine[] playerDeathVoiceLines;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
