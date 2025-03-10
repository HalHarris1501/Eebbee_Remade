using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    private static SaveData _current;
    public static SaveData current
    {
        get
        {
            if (_current == null)
            {
                _current = new SaveData();
            }
            return _current;
        }
        set
        {
            if (value != null)
            {
                _current = value;
            }
        }
    }

    public PlayerData PlayerData;

    public int PreviousWinScore = 0;
    public int TotalScore = 0;
    public int HighScore = 0;
    public int PreviousRunScore = 0;

    public List<PowerupData> Powerups;

    public List<PlayerSkinData> Skins;

    public SettingsData settingsData;
}
