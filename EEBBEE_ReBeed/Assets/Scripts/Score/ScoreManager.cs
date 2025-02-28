using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, ISubject<Score>
{
    private List<IObserver<Score>> _observers = new List<IObserver<Score>>();
    [SerializeField] private Score _currentScore;
    [SerializeField] private int _scoreMultiplier = 1;
    private Dictionary<string, int> _multipliersDictionary = new Dictionary<string, int>();
    [SerializeField] private PowerupObject _helperPowerup;

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
        _currentScore.ScoreCount += change * TotalMultiplier();
        NotifyObservers(_currentScore, ISubject<Score>.NotificationType.Changed);
    }

    private int TotalMultiplier()
    {
        int totalMultiplier = 1;
        foreach(KeyValuePair<string, int> multiplier in _multipliersDictionary)
        {
            totalMultiplier = totalMultiplier * multiplier.Value;
        }
        return totalMultiplier;
    }

    public void AddMultiplier(string key, int multiplier)
    {
        _multipliersDictionary.Add(key, multiplier);
    }

    public void RemoveMultiplier(string key)
    {
        _multipliersDictionary.Remove(key);
    }

    public void SetScore()
    {
        CheckHighScore();
        ScoreStorage.current.PreviousWinScore = _currentScore.ScoreCount;
        ScoreStorage.current.PreviousRunScore = _currentScore.ScoreCount;
        ScoreStorage.current.TotalScore += _currentScore.ScoreCount;
    }

    private void CheckHighScore()
    {
        if(ScoreStorage.current.HighScore < _currentScore.ScoreCount)
        {
            ScoreStorage.current.HighScore = _currentScore.ScoreCount;
        }
    }

    public void SetFailScore()
    {
        if(_helperPowerup.PowerupData.Active)
        {
            ScoreStorage.current.TotalScore +=  Mathf.FloorToInt(_currentScore.ScoreCount / 4);
        }
        ScoreStorage.current.PreviousRunScore = 0;
    }

    private void OnEnable()
    {
        _multipliersDictionary.Add("Base", _scoreMultiplier);
        //PlayerMovement.onPlayerWin += SetScore;
        //PlayerMovement.onPlayerDeath += SetFailScore;
    }

    private void OnDisable()
    {
        _multipliersDictionary.Clear();
        //PlayerMovement.onPlayerWin -= SetScore;
        //PlayerMovement.onPlayerDeath -= SetFailScore;
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
