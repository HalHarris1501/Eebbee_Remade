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

    public PlayerSkinData CurrentSkinData;
}
