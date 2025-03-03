using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour, IObserver<SaveManager>
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

    [Header("ScoreTextManager")]
    [SerializeField] private ScoreTextSetter scoreTextSetter;

    [Header("Purchaseable Items References")]
    [SerializeField] private List<SkinObject> _skinsData;
    [SerializeField] private List<PowerupObject> _powerupsData;
    [SerializeField] private List<ShopItemPrefab> _shopItems;

    [Header("UI Objects References")]
    [SerializeField] private ShopItemPrefab _purchableItemPrefab;
    [SerializeField] private GameObject _powerupsGridLayout;
    [SerializeField] private GameObject _skinsGridLayout;
    [SerializeField] private GameObject _notificationWindow;
    [SerializeField] private TMP_Text _notificationText;

    [Header("Ad reward variables")]
    [SerializeField] private int _adPointsReward;

    public void PurchaseItem(SkinObject skinToBuy, ShopItemPrefab buttonPressed)
    {
        if (!_skinsData.Contains(skinToBuy))
        {
            Debug.LogError(skinToBuy.SkinData.SkinName + " skin not in list");
            return;
        }
        if(skinToBuy.Price > ScoreStorage.current.TotalScore)
        {
            Debug.LogError(skinToBuy.SkinData.SkinName + " is too expensive. Current Points: " + ScoreStorage.current.TotalScore);
            return;
        }

        ScoreStorage.current.TotalScore -= skinToBuy.Price;
        skinToBuy.SkinData.Owned = true;
        buttonPressed.UpdateUI(skinToBuy);
        scoreTextSetter.UpdateUI();

        SaveManager.Instance.SaveSkinData(skinToBuy.SkinData);
        SaveManager.Instance.SaveScoreData();
    }

    public void PurchaseItem(PowerupObject powerupToBuy, ShopItemPrefab buttonPressed)
    {
        if (!_powerupsData.Contains(powerupToBuy))
        {
            Debug.LogError(powerupToBuy.PowerupData.PowerupType + " powerup not in list");
            return;
        }
        if (powerupToBuy.Price > ScoreStorage.current.TotalScore)
        {
            Debug.LogError(powerupToBuy.PowerupData.PowerupType + " is too expensive. Current Points: " + ScoreStorage.current.TotalScore);
            return;
        }

        ScoreStorage.current.TotalScore -= powerupToBuy.Price;
        powerupToBuy.PowerupData.Active = true;
        buttonPressed.UpdateUI(powerupToBuy);
        scoreTextSetter.UpdateUI();

        SaveManager.Instance.SavePowerupData(powerupToBuy.PowerupData);
        SaveManager.Instance.SaveScoreData();
    }

    public void GiveAdReward()
    {
        ScoreStorage.current.TotalScore += _adPointsReward;
        scoreTextSetter.UpdateUI();

        SaveManager.Instance.SaveScoreData();

        _notificationText.text = _adPointsReward + " points added!";
        _notificationWindow.SetActive(true);        
    }

    public void UpdateAllShopItems()
    {
        foreach(ShopItemPrefab shopItem in _shopItems)
        {
            shopItem.UpdateUI();
        }
    }

    private void Awake()
    {
        foreach(SkinObject skin in _skinsData)
        {
            ShopItemPrefab newItem = Instantiate(_purchableItemPrefab);
            newItem.gameObject.transform.SetParent(_skinsGridLayout.transform);
            newItem.SetSkin(skin);
            _shopItems.Add(newItem);
        }

        foreach(PowerupObject powerup in _powerupsData)
        {
            ShopItemPrefab newItem = Instantiate(_purchableItemPrefab);
            newItem.gameObject.transform.SetParent(_powerupsGridLayout.transform);
            newItem.SetPowerup(powerup);
            _shopItems.Add(newItem);
        }
        SaveManager.Instance.RegisterObserver(this);
        UpdateAllShopItems();
    }

    public void SetPlayerSkin(SkinObject skinToSet)
    {
        PlayerData.current.CurrentSkinData = skinToSet.SkinData;
        UpdateAllShopItems();
        SaveManager.Instance.SavePlayerData();
    }

    public void NewItemAdded(SaveManager type)
    {
        throw new System.NotImplementedException();
    }

    public void ItemRemoved(SaveManager type)
    {
        throw new System.NotImplementedException();
    }

    public void ItemAltered(SaveManager type, int count)
    {
        Debug.Log("Updating shop items with loaded data");
        UpdateAllShopItems();
    }
}
