using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, ISubject<Score>
{
    private List<IObserver<Score>> _observers = new List<IObserver<Score>>();
    private Score _currentScore;

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
        _currentScore.ScoreCount += change;
        NotifyObservers(_currentScore, ISubject<Score>.NotificationType.Changed);
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
