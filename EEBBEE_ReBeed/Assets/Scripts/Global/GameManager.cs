using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ISubject<Direction>
{
    private List<IObserver<Direction>> _observers = new List<IObserver<Direction>>();
    private Direction _direction = Direction.Forward;

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
        _direction = Direction.Backword;
        NotifyObservers(_direction, ISubject<Direction>.NotificationType.Changed);
    }
}

public enum Direction
{
    Forward,
    Backword
}
