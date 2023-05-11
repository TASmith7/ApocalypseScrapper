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
    public VoiceLine[] killBossVoiceLines;
    public VoiceLine[] finishWithSRankVoiceLines;
    public VoiceLine[] finishWithARankVoiceLines;
    public VoiceLine[] finishWithBRankVoiceLines;
    public VoiceLine[] finishWithCRankVoiceLines;
    public VoiceLine[] finishWithDRankVoiceLines;
    public VoiceLine[] finishWithFRankVoiceLines;
    public VoiceLine[] storeInfoVoiceLines;
    public VoiceLine[] bonusSpendItVoiceLines;
    public VoiceLine[] floorPassVoiceLines;
    public VoiceLine[] floorFailVoiceLines;

}
