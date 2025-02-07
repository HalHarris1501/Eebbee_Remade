using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ISubject<Direction>
{
    private List<IObserver<Direction>> _observers = new List<IObserver<Direction>>();
    private Direction _direction = Direction.Forward;
    [SerializeField] private List<PowerupObject> _powerups;

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
        PlayerMovement.onPlayerDeath += ManageLose;
        PlayerMovement.onPlayerWin += ManageWin;
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
        DeactivateDoubler();
        SceneSwapper.Instance.LoadSceneByName("Menu Scene");
    }

    public void ManageLose() // function called when the game is lost
    {
        if(GetPowerup(PowerupType.Helmet).PowerupData.Active)
        {
            GetPowerup(PowerupType.Helmet).PowerupData.Active = false;
            return;
        }

        Debug.Log("Bee ded");
        foreach(PowerupObject powerup in _powerups)
        {
            powerup.PowerupData.Active = false;
        }
        SceneSwapper.Instance.LoadSceneByName("Menu Scene");
    }

    private void DeactivateDoubler() //function to deactivate the points double on a win
    {
        foreach(PowerupObject powerup in _powerups)
        {
            if(powerup.PowerupData.PowerupType != PowerupType.NectarDoubler)
            {
                return;
            }
            powerup.PowerupData.Active = false;
        }
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
}

public enum Direction
{
    Forward,
    Backward
}
