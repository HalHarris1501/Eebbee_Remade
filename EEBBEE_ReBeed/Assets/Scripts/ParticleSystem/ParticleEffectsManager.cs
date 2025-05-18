using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectsManager : MonoBehaviour, IObserver<Score>
{
    [Header("Variables")]
    private int _currentScore = 0;

    [Header("ParticleSystemReferences")]
    [SerializeField] private ParticleSystem _nectarSystem;
    [SerializeField] private ParticleSystem _deathSystem;

    //Singleton pattern
    #region Singleton
    private static ParticleEffectsManager _instance;
    public static ParticleEffectsManager Instance
    {
        get //making sure that a Affect manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ParticleEffectsManager>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("ParticleEffectsManager");
                _instance = go.AddComponent<ParticleEffectsManager>();
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
        ScoreManager.Instance.RegisterObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {

    }

    private void UpNectarCount()
    {
        ParticleSystem.EmissionModule nectarSystemEmission = _nectarSystem.emission;
        nectarSystemEmission.rateOverTime = Mathf.FloorToInt(_currentScore/5);
    }

    private void StopNectar()
    {
        ParticleSystem.EmissionModule nectarSystemEmission = _nectarSystem.emission;
        nectarSystemEmission.rateOverTime = 0;
    }

    public void TriggerDeathExplosion()
    {
        _deathSystem.Play();
    }

    public void ItemAltered(Score score, int count)
    {
        _currentScore = score.ScoreCount;
        UpNectarCount();
    }

    public void ItemRemoved(Score type)
    {
        throw new System.NotImplementedException();
    }

    public void NewItemAdded(Score type)
    {
        throw new System.NotImplementedException();
    }
}
