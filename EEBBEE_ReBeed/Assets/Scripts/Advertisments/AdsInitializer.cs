using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    [SerializeField] RewardedAdsButton rewardedAdsButton;
    private string _gameId;

    //Singleton pattern
    #region Singleton
    private static AdsInitializer _instance;
    public static AdsInitializer Instance
    {
        get //making sure that a game manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AdsInitializer>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("AdsInitializer");
                _instance = go.AddComponent<AdsInitializer>();
            }
            return _instance;
        }
    }
    #endregion

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
            _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
