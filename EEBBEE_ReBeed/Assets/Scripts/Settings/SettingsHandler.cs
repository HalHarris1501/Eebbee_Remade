using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    //Singleton pattern
    #region Singleton
    private static SettingsHandler _instance;
    public static SettingsHandler Instance
    {
        get //making sure that a game manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SettingsHandler>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("SaveManager");
                _instance = go.AddComponent<SettingsHandler>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    [Header("Settings Data")]
    [SerializeField] private SettingsSO _settingsSO;

    [Header("UI Object References")]
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Toggle _sfxToggle;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Toggle _obstacleVisibilityToggle;

    public void SaveSettings()
    {
        _settingsSO.SettingsData.MusicOn = _musicToggle.isOn;
        _settingsSO.SettingsData.MusicVolume = _musicVolumeSlider.value;
        _settingsSO.SettingsData.SFXOn = _sfxToggle.isOn;
        _settingsSO.SettingsData.SFXVolume = _sfxVolumeSlider.value;
        _settingsSO.SettingsData.IncreasedVisibility = _obstacleVisibilityToggle.isOn;
        SaveManager.Instance.SaveSettingsData();
        AudioManager.Instance.UpdateSettings();
    }

    public void LoadSettings()
    {
        _musicToggle.isOn = _settingsSO.SettingsData.MusicOn;
        _musicVolumeSlider.value = _settingsSO.SettingsData.MusicVolume;
        _sfxToggle.isOn = _settingsSO.SettingsData.SFXOn;
        _sfxVolumeSlider.value = _settingsSO.SettingsData.SFXVolume;
        _obstacleVisibilityToggle.isOn = _settingsSO.SettingsData.IncreasedVisibility;
    }
}
