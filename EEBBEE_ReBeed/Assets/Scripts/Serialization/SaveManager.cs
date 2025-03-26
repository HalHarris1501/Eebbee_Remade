using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour, ISubject<SaveManager>
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

    [Header("Observers")]
    private List<IObserver<SaveManager>> _observers = new List<IObserver<SaveManager>>();

    [Header("Save Fields")]
    [SerializeField] private List<SkinObject> _skins;
    [SerializeField] private List<PowerupObject> _powerups;
    [SerializeField] private SettingsSO _settingsObject;

    [Header("Default Save Data")]
    [SerializeField] private SaveData _default;

    // Start is called before the first frame update
    void Start()
    {
        OnLoad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetData()
    {
        SaveData.current = _default;
        Save();
        OnLoad();
    }

    public void SaveSkinData(PlayerSkinData skinData)
    {
        if (SaveData.current.Skins == null || SaveData.current.Skins.Count <= 0)
        {
            SaveData.current.Skins = new List<PlayerSkinData>();
            goto Add;
        }
        foreach (PlayerSkinData data in SaveData.current.Skins)
        {
            if (data.SkinName == skinData.SkinName)//check if powerup data has already been saved
            {
                SaveData.current.Skins.Remove(data);//if it has delete and save new data
                SaveData.current.Skins.Add(skinData);
                return;
            }
        }
        Add:
        SaveData.current.Skins.Add(skinData);//if not in, add save data
        //Debug.Log("Saving Skins data");
        Save();
    }

    public void SavePowerupData(PowerupData powerupData)
    {
        if(SaveData.current.Powerups == null || SaveData.current.Powerups.Count <= 0)
        {
            SaveData.current.Powerups = new List<PowerupData>();
            goto Add;
        }
        foreach(PowerupData data in SaveData.current.Powerups)
        {
            if(data.PowerupType == powerupData.PowerupType)//check if powerup data has already been saved
            {
                SaveData.current.Powerups.Remove(data);//if it has delete and save new data
                SaveData.current.Powerups.Add(powerupData);
                return;
            }
        }
        Add:
        SaveData.current.Powerups.Add(powerupData);//if not in, add save data#
        //Debug.Log("Saving Powerup data");
        Save();
    }

    public void SavePlayerData()
    {
        SaveData.current.PlayerData = PlayerData.current;
        //Debug.Log("Saving Player data");
        Save();
    }

    public void SaveScoreData()
    {
        SaveData.current.PreviousWinScore = ScoreStorage.current.PreviousWinScore;
        SaveData.current.PreviousRunScore = ScoreStorage.current.PreviousRunScore;
        SaveData.current.TotalScore = ScoreStorage.current.TotalScore;
        SaveData.current.HighScore = ScoreStorage.current.HighScore;

        //Debug.Log("Saving score data");
        Save();
    }

    public void SaveSettingsData()
    {
        SaveData.current.SettingsData = _settingsObject.SettingsData;
        Debug.Log("Saving settings data");
        Save();
    }

    public void Save()
    {
        SerializationManager.Save("MainSave", SaveData.current);
    }

    private void OnLoad()
    {
        SaveData.current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/MainSave.save");
        if(SaveData.current == null)
        {
            SaveData.current = _default;
        }


        PlayerData.current = SaveData.current.PlayerData;
        //Debug.Log("Player data loaded and set");

        ScoreStorage.current.PreviousWinScore = SaveData.current.PreviousWinScore;
        ScoreStorage.current.PreviousRunScore = SaveData.current.PreviousRunScore;
        ScoreStorage.current.TotalScore = SaveData.current.TotalScore;
        ScoreStorage.current.HighScore = SaveData.current.HighScore;
        //Debug.Log("Score data loaded and set");

        if (SaveData.current.SettingsData != null && _settingsObject.SettingsData != null)
        {
            _settingsObject.SettingsData = SaveData.current.SettingsData;
            Debug.Log("loaded settings data");
        }

        if (SaveData.current.Powerups == null  || SaveData.current.Powerups.Count <= 0)
        {
            //Debug.Log("No powerup save data");
            goto Skins;
        }
        foreach(PowerupData data in SaveData.current.Powerups)
        {
            FindPowerUp(data).PowerupData = data;
        }
        //Debug.Log("Powerups data loaded and set");

        Skins:
        if(SaveData.current.Skins == null || SaveData.current.Skins.Count <= 0)
        {
            //Debug.Log("No Skin save data");
            goto end;
        }
        foreach(PlayerSkinData data in SaveData.current.Skins)
        {
            FindSkinObject(data).SkinData = data;
        }
        //Debug.Log("Skins data loaded and set");

        end:
        NotifyObservers(this, ISubject<SaveManager>.NotificationType.Changed);
    }

    private PowerupObject FindPowerUp(PowerupData data)
    {
        foreach(PowerupObject powerup in _powerups)
        {
            if (powerup.PowerupData.PowerupType == data.PowerupType)
            {
                return powerup;
            }
        }
        return null;
    }

    private SkinObject FindSkinObject(PlayerSkinData data)
    {
        foreach(SkinObject skin in _skins)
        {
            if(skin.SkinData.SkinName == data.SkinName)
            {
                return skin;
            }
        }
        return null;
    }

    public void RegisterObserver(IObserver<SaveManager> o)
    {
        _observers.Add(o);
        //Debug.Log("Observer added");
    }

    public void RemoveObserver(IObserver<SaveManager> o)
    {
        _observers.Remove(o);
    }

    public void NotifyObservers(SaveManager type, ISubject<SaveManager>.NotificationType notificationType)
    {
        //Debug.Log("Notifiying observers");
        foreach(IObserver<SaveManager> observer in _observers)
        {
            observer.ItemAltered(this, 0);
        }
    }
}
