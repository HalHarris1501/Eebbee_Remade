using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //Singleton pattern
    #region Singleton
    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get //making sure that a game manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveManager>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("SaveManager");
                _instance = go.AddComponent<SaveManager>();
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField] private List<SkinObject> _skins;
    [SerializeField] private List<PowerupObject> _powerups;
    [SerializeField] private ScoreStorage _scoreStorage;
    [SerializeField] private PlayerData _playerData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveSkinData(PlayerSkinData skinData)
    {
        if (SaveData.current.Skins.Count <= 0)
        {
            SaveData.current.Skins.Add(skinData);
        }
        else
        {
            foreach (PlayerSkinData data in SaveData.current.Skins)
            {
                if (data.SkinName == skinData.SkinName)//check if powerup data has already been saved
                {
                    SaveData.current.Skins.Remove(data);//if it has delete and save new data
                    SaveData.current.Skins.Add(skinData);
                    return;
                }
            }
            SaveData.current.Skins.Add(skinData);//if not in, add save data
        }
    }

    public void SavePowerupData(PowerupData powerupData)
    {
        if(SaveData.current.Powerups.Count <= 0)
        {
            SaveData.current.Powerups.Add(powerupData);
        }
        else
        {
            foreach(PowerupData data in SaveData.current.Powerups)
            {
                if(data.PowerupType == powerupData.PowerupType)//check if powerup data has already been saved
                {
                    SaveData.current.Powerups.Remove(data);//if it has delete and save new data
                    SaveData.current.Powerups.Add(powerupData);
                    return;
                }
            }
            SaveData.current.Powerups.Add(powerupData);//if not in, add save data
        }
    }

    public void SavePlayerData()
    {
        SaveData.current.PlayerData = _playerData;
    }

    public void SaveScoreData()
    {
        SaveData.current.ScoreStorage = _scoreStorage;
    }

    public void Save()
    {
        SerializationManager.Save("MainSave", SaveData.current);
    }

    private void OnLoad()
    {
        SaveData.current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/MainSave.save");
    }
}
