using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ISubject<Direction>
{
    private List<IObserver<Direction>> _observers = new List<IObserver<Direction>>();
    private Direction _direction = Direction.Stop;

    [Header("Object Referencese")]
    [SerializeField] private List<PowerupObject> _powerups;
    [SerializeField] private List<SkinObject> _skins;
    [SerializeField] private PlayerMovement _player;
    [SerializeField] private SeedStorage _seedStorage;
    
    [Header("Invincibilty variables")]
    [SerializeField] private bool _invincible;
    [SerializeField] private float _invincibleTime = 1f;

    [Header("Bee Visuals References")]
    [SerializeField] private GameObject _helperBee;
    [SerializeField] private GameObject _nectarDoubler;
    [SerializeField] private GameObject _helmet;
    [SerializeField] private GameObject _beeSprite;
    [SerializeField] private Animator _beeAnimator;

    //Singleton pattern
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get //making sure that a game manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                _instance = go.AddComponent<GameManager>();
            }
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        if (_instance == null) //if there's no instance of the Money manager, make this the GameManager, ortherwise delete this to avoid duplicates
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if(GetPowerup(PowerupType.Bee_Helper).PowerupData.Active)
        {
            _helperBee.SetActive(true);
        }
        else
        {
            _helperBee.SetActive(false);
        }

        if(GetPowerup(PowerupType.Nectar_Doubler).PowerupData.Active)
        {
            ScoreManager.Instance.AddMultiplier("NectarDoubler", 2);
            _nectarDoubler.SetActive(true);
        }
        else
        {
            _nectarDoubler.SetActive(false);
        }

        if (GetPowerup(PowerupType.Helmet).PowerupData.Active)
        {
            _helmet.SetActive(true);
        }
        else
        {
            _helmet.SetActive(false);
        }

        try
        {
            _beeSprite.GetComponent<SpriteRenderer>().sprite = FindSkin(PlayerData.current.CurrentSkinData).Skin;
        }
        catch 
        {
            Debug.Log("Player skin not set");
        }

        PlayerMovement.onPlayerDeath += ManageLose;
        PlayerMovement.onPlayerWin += ManageWin;
    }

    public void StartGame()
    {        
        _direction = Direction.Forward;
        NotifyObservers(_direction, ISubject<Direction>.NotificationType.Changed);
        _beeAnimator.SetTrigger("IsMovable");
    }

    private SkinObject FindSkin(PlayerSkinData data)
    {
        foreach(SkinObject skin in _skins)
        {
            if(skin.SkinData == data)
            {
                return skin;
            }
        }
        return null;
    }

    private void OnDisable()
    {
        PlayerMovement.onPlayerDeath -= ManageLose;
        PlayerMovement.onPlayerWin -= ManageWin;
    }

    public void NotifyObservers(Direction type, ISubject<Direction>.NotificationType notificationType)
    {
        foreach(var Observer in _observers)
        {
            Observer.ItemAltered(_direction, 0);
        }
    }

    public void RegisterObserver(IObserver<Direction> o)
    {
        if (_observers.Count > 0)
        {
            if (_observers.Contains(o)) return; //check that o doesn't already exist in the list to avoid duplicates
        }
        _observers.Add(o);
    }

    public void RemoveObserver(IObserver<Direction> o)
    {
        _observers.Remove(o);
    }

    public void GoBack()
    {
        _direction = Direction.Backward;
        NotifyObservers(_direction, ISubject<Direction>.NotificationType.Changed);
    }

    public void ManageWin() // function called when the game is won
    {
        Debug.Log("Bee Win");
        if (_seedStorage.SeedRandomised == true)
        {
            ScoreManager.Instance.SetScore();
        }
        GetPowerup(PowerupType.Nectar_Doubler).PowerupData.Active = false; //deactivate point doubler on win
        SaveManager.Instance.SaveScoreData();
        SceneSwapper.Instance.LoadSceneByName("Menu Scene");
    }

    public void ManageLose() // function called when the game is lost
    {
        if(_invincible)
        {
            return;
        }
        if(GetPowerup(PowerupType.Helmet).PowerupData.Active)
        {
            GetPowerup(PowerupType.Helmet).PowerupData.Active = false;
            _helmet.SetActive(false);
            StartCoroutine(InvincibleTimerCoroutine());
            return;
        }

        Debug.Log("Bee ded");
        _direction = Direction.Stop;
        NotifyObservers(_direction, ISubject<Direction>.NotificationType.Changed);
        DisableBee();
        ScoreManager.Instance.SetFailScore();
        foreach(PowerupObject powerup in _powerups)
        {
            powerup.PowerupData.Active = false;
        }
        SaveManager.Instance.SaveScoreData();
        _beeAnimator.SetTrigger("IsDead");        
    }

    private void EnableBee()
    {
        _player.SetColliderActive(true);
        _player.SetRigidbodyActive(true);
        _player.SetControlsActive(true);
    }

    private void DisableBee()
    {
        _player.SetColliderActive(false);
        _player.SetRigidbodyActive(false);
        _player.SetControlsActive(false);
    }

    public void LoadMenu()
    {
        SceneSwapper.Instance.LoadSceneByName("Menu Scene");
    }

    private PowerupObject GetPowerup(PowerupType powerupType)
    {
        foreach(PowerupObject powerup in _powerups)
        {
            if(powerup.PowerupData.PowerupType == powerupType)
            {
                return powerup;
            }
        }

        Debug.LogError("Powerup of type: " + powerupType + " not in list.");
        return null;
    }

    private IEnumerator InvincibleTimerCoroutine()
    {
        _invincible = true;
        _beeSprite.GetComponent<SpriteRenderer>().color = Color.black;

        yield return new WaitForSeconds(_invincibleTime);

        _beeSprite.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        _invincible = false;
    }
}

public enum Direction
{
    Forward,
    Backward,
    Stop
}
