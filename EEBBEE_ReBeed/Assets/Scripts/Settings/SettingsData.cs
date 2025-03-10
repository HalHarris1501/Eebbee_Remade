using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    public bool MusicOn = true;
    [Range(0f, 1f)] public float MusicVolume = 1f;
    public bool SFXOn = true;
    [Range(0f, 1f)] public float SFXVolume = 1f;
}
