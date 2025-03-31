using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    private static PlayerData _current;
    public static PlayerData current
    {
        get
        {
            if (_current == null)
            {
                _current = new PlayerData();
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

    private static PlayerSkinData _currentSkin;

    public PlayerSkinData CurrentSkinData
    {
        get
        {
            if(_currentSkin == null)
            {
                PlayerSkinData playerSkinData = new PlayerSkinData();
                playerSkinData.SkinName = SkinName.Default;
                playerSkinData.Owned = true;
                _currentSkin = playerSkinData;
            }
            return _currentSkin;
        }
        set
        {
            if(value != null)
            {
                _currentSkin = value;
            }
        }
    }
}
