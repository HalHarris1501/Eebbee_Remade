using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemPrefab : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private SkinObject _skin;
    [SerializeField] private PowerupObject _powerup;

    [Header("Child References")]
    [SerializeField] private Button _purchaseButton;
    [SerializeField] private Image _itemSprite;

    public void SetSkin(SkinObject skin)
    {
        _skin = skin;
        SkinSetUP();
    }

    public void SetPowerup(PowerupObject powerup)
    {
        _powerup = powerup;
        PowerUpSetUp();
    }

    public void UpdateUI(SkinObject skin)
    {
        if(_skin.SkinData.Owned)
        {
            _purchaseButton.interactable = false;
            _purchaseButton.GetComponentInChildren<TMP_Text>().text = "Owned!";
        }
    }

    public void UpdateUI(PowerupObject powerup)
    {
        if (_powerup.PowerupData.Active)
        {
            _purchaseButton.interactable = false;
            _purchaseButton.GetComponentInChildren<TMP_Text>().text = "Active!";
        }
    }

    private void PowerUpSetUp()
    {
        _purchaseButton.GetComponentInChildren<TMP_Text>().text = _powerup.Price + " Nectar";
        _purchaseButton.onClick.AddListener(delegate { ShopManager.Instance.PurchaseItem(_powerup, this);  });
        _itemSprite.sprite = _powerup.sprite;
        UpdateUI(_powerup);
    }

    private void SkinSetUP()
    {
        _purchaseButton.GetComponentInChildren<TMP_Text>().text = _skin.Price + " Nectar";
        _purchaseButton.onClick.AddListener(delegate { ShopManager.Instance.PurchaseItem(_skin, this); });
        _itemSprite.sprite = _skin.Skin;
        UpdateUI(_skin);
    }
}
