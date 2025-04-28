using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to control the effects that can be given to the player
public class AffectManager : MonoBehaviour, ISubject<CollectableData> //lets observers know of changes to the effect currently active
{
    private IEnumerator _currentEffect; //Variable to hold the coroutine data for the effects
    public delegate void EndEffect(); //delegate function to forcibly end the current effect
    public static EndEffect EndingEffect; //variable to store the function for what happens when an effect ends
    private int _effectTimeRemaining; //variable for the time left until an effect ends
    private List<IObserver<CollectableData>> _observers = new List<IObserver<CollectableData>>(); //observer list for changes to effect

    //Singleton pattern
    #region Singleton
    private static AffectManager _instance;
    public static AffectManager Instance
    {
        get //making sure that a Affect manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AffectManager>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("AffectManager");
                _instance = go.AddComponent<AffectManager>();
            }
            return _instance;
        }
    }

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
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        //when the player wins or dies, the current effect is ended
        PlayerMovement.onPlayerDeath += ForceEndEffect;
        PlayerMovement.onPlayerWin += ForceEndEffect;
    }

    private void OnDisable()
    {
        //if the affect manager is disabled, removes the force end affect from the player events, to prevent accidental calling
        PlayerMovement.onPlayerDeath -= ForceEndEffect;
        PlayerMovement.onPlayerWin -= ForceEndEffect;
    }

    //function to set the current effect using a given IEnumerator
    public void SetCurrentEffect(IEnumerator newEffect)
    {
        ForceEndEffect(); //end the current effect        
        _currentEffect = newEffect; //set the new effect
    }

    //function to stop the current effect
    private void ForceEndEffect()
    {
        if (_currentEffect != null) //if there isn't a current effect, dont attempt to end effect
        {
            StopCoroutine(_currentEffect); //stop the coroutine for the current effect
            EndingEffect(); //trigger any ending effects
        }
    }

    //function to start the effect using a given collectable type
    public void StartEffect(CollectableData type)
    {
        if(_currentEffect == null) //if there isn't a current effect, dont attempt to start effect
        {
            Debug.Log("No Affect set");
            return;
        }
        StartCoroutine(_currentEffect); //start the current effect
        NotifyObservers(type, ISubject<CollectableData>.NotificationType.Changed); //let observers know that the effect has started
    }

    //function to adjust the amount of time remaining for the current effect using a given number to set the effect time to and the collectable type
    public void AdjustAffectTime(int timeChange, CollectableData type)
    {
        //this function is mostly used by UI objects to display the time remaining on the current effect
        _effectTimeRemaining = timeChange; //set the time remaining to the given time
        NotifyObservers(type, ISubject<CollectableData>.NotificationType.Changed); //let observers know that the effect has been altered
    }

    //function to register observers
    public void RegisterObserver(IObserver<CollectableData> o)
    {
        if (_observers.Count > 0) //check if the list has observers already
        {
            if (_observers.Contains(o)) return; //check that 'o' doesn't already exist in the list to avoid duplicates
        }
        _observers.Add(o); //if list is empty, add new observer
    }

    public void RemoveObserver(IObserver<CollectableData> o)
    {
        throw new System.NotImplementedException();
    }

    //function to notify observers of changes to the current effect
    public void NotifyObservers(CollectableData type, ISubject<CollectableData>.NotificationType notificationType)
    {
        foreach (var Observer in _observers) //loop through each observer
        {
            Observer.ItemAltered(type, _effectTimeRemaining); //let observer know that a change has been made and how much time is left on the effect
        }
    }
}
