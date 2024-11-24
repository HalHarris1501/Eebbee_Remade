using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, ISubject<Score>
{
    private List<IObserver<Score>> _observers = new List<IObserver<Score>>();
    [SerializeField] private Score _currentScore;
    [SerializeField] private int _scoreMultiplier = 1;

    //Singleton pattern
    #region Singleton
    private static ScoreManager _instance;
    public static ScoreManager Instance
    {
        get //making sure that a Score manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreManager>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("ScoreManager");
                _instance = go.AddComponent<ScoreManager>();
            }
            return _instance;
        }
    }
    #endregion

    public void NotifyObservers(Score type, ISubject<Score>.NotificationType notificationType)
    {
        foreach (var Observer in _observers)
        {
            Observer.ItemAltered(_currentScore, 0);
        }
    }

    public void RegisterObserver(IObserver<Score> o)
    {
        if (_observers.Count > 0)
        {
            if (_observers.Contains(o)) return; //check that o doesn't already exist in the list to avoid duplicates
        }
        _observers.Add(o);
    }

    public void RemoveObserver(IObserver<Score> o)
    {
        _observers.Remove(o);
    }

    public void AlterScore(int change)
    {
        _currentScore.ScoreCount += change * _scoreMultiplier;
        NotifyObservers(_currentScore, ISubject<Score>.NotificationType.Changed);
    }

    public void SetMultiplier(int multiplier)
    {
        _scoreMultiplier = multiplier;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
