using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //Singleton pattern
    #region Singleton
    private static ShopManager _instance;
    public static ShopManager Instance
    {
        get //making sure that a game manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ShopManager>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("ShopManager");
                _instance = go.AddComponent<ShopManager>();
            }
            return _instance;
        }
    }
    #endregion

    [Header("Purchaseable Items References")]
    [SerializeField] private List<SkinObject> _skinsData;
    [SerializeField] private List<PowerupObject> _powerupsData;

    [Header("Score Reference")]
    [SerializeField] private ScoreStorage _scoreStorage;

    [Header("UI Objects References")]
    [SerializeField] private ShopItemPrefab _purchableItemPrefab;
    [SerializeField] private GameObject _powerupsGridLayout;
    [SerializeField] private GameObject _skinsGridLayout;

    public void PurchaseItem(SkinObject skinToBuy)
    {
        if (!_skinsData.Contains(skinToBuy))
        {
            Debug.LogError(skinToBuy.SkinData.SkinName + " skin not in list");
            return;
        }
        if(skinToBuy.Price > _scoreStorage.TotalScore)
        {
            Debug.LogError(skinToBuy.SkinData.SkinName + " is too expensive. Current Points: " + _scoreStorage.TotalScore);
            return;
        }

        _scoreStorage.TotalScore -= skinToBuy.Price;
        skinToBuy.SkinData.Owned = true;
    }

    public void PurchaseItem(PowerupObject powerupToBuy)
    {
        if (!_powerupsData.Contains(powerupToBuy))
        {
            Debug.LogError(powerupToBuy.PowerupData.PowerupType + " powerup not in list");
            return;
        }
        if (powerupToBuy.Price > _scoreStorage.TotalScore)
        {
            Debug.LogError(powerupToBuy.PowerupData.PowerupType + " is too expensive. Current Points: " + _scoreStorage.TotalScore);
            return;
        }

        _scoreStorage.TotalScore -= powerupToBuy.Price;
        powerupToBuy.PowerupData.Active = true;
    }

    private void Awake()
    {
        foreach(SkinObject skin in _skinsData)
        {
            ShopItemPrefab newItem = Instantiate(_purchableItemPrefab);
            newItem.gameObject.transform.SetParent(_skinsGridLayout.transform);
            newItem.SetSkin(skin);
        }

        foreach(PowerupObject powerup in _powerupsData)
        {
            ShopItemPrefab newItem = Instantiate(_purchableItemPrefab);
            newItem.gameObject.transform.SetParent(_powerupsGridLayout.transform);
            newItem.SetPowerup(powerup);
        }
    }
}
