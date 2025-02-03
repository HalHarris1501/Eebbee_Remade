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

    [SerializeField] private List<SkinObject> _skinsData;
    [SerializeField] private List<PowerupData> _powerupsData;
    [SerializeField] private ScoreStorage _scoreStorage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public void PurchaseItem(PowerupData powerupToBuy)
    {
        if (!_powerupsData.Contains(powerupToBuy))
        {
            Debug.LogError(powerupToBuy.PowerupType + " skin not in list");
            return;
        }
        if (powerupToBuy.Price > _scoreStorage.TotalScore)
        {
            Debug.LogError(powerupToBuy.PowerupType + " is too expensive. Current Points: " + _scoreStorage.TotalScore);
            return;
        }

        _scoreStorage.TotalScore -= powerupToBuy.Price;
        powerupToBuy.Purchased = true;
    }
}
