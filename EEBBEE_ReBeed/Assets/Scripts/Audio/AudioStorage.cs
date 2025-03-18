using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioStorage 
{
    public AudioTag Tag;
    public AudioClip Clip;
}

public enum AudioTag
{ 
    BuzzSound,
    MenuMusic,
    GameMusic,
    DeathSound,
    WinSound,
    StartSound,
    ButtonSound,
    SkinChangeSound,
    PurchaseSound,
    HelmetSound
}

