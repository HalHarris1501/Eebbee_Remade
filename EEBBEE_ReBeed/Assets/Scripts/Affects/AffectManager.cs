using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectManager : MonoBehaviour, ISubject<CollectableData>
{
    private IEnumerator _currentEffect;
    private int _effectTimeRemaining;
    private List<IObserver<CollectableData>> _observers = new List<IObserver<CollectableData>>();

    //Singleton pattern
    #region Singleton
    private static AffectManager _instance;
    public static AffectManager Instance
    {
        get //making sure that a game manager always exists
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
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentEffect(IEnumerator newEffect)
    {
        if(_currentEffect != null)
        {
            StopCoroutine(_currentEffect);
        }
        _currentEffect = newEffect;
    }

    public void StartEffect(CollectableData type)
    {
        if(_currentEffect == null)
        {
            Debug.Log("No Affect set");
            return;
        }
        StartCoroutine(_currentEffect);
        NotifyObservers(type, ISubject<CollectableData>.NotificationType.Changed);
    }

    public void AdjustAffectTime(int timeChange, CollectableData type)
    {
        _effectTimeRemaining = timeChange;
        NotifyObservers(type, ISubject<CollectableData>.NotificationType.Changed);
    }

    public void RegisterObserver(IObserver<CollectableData> o)
    {
        if (_observers.Count > 0)
        {
            if (_observers.Contains(o)) return; //check that o doesn't already exist in the list to avoid duplicates
        }
        _observers.Add(o);
    }

    public void RemoveObserver(IObserver<CollectableData> o)
    {
        throw new System.NotImplementedException();
    }

    public void NotifyObservers(CollectableData type, ISubject<CollectableData>.NotificationType notificationType)
    {
        foreach (var Observer in _observers)
        {
            Observer.ItemAltered(type, _effectTimeRemaining);
        }
    }
}
