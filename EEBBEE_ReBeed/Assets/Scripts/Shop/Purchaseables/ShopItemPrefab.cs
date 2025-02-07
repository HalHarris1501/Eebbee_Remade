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

    private void PowerUpSetUp()
    {
        _purchaseButton.GetComponentInChildren<TMP_Text>().text = _powerup.Price + " Nectar";
        _purchaseButton.onClick.AddListener(delegate { ShopManager.Instance.PurchaseItem(_powerup);  });
        _itemSprite.sprite = _powerup.sprite;
    }

    private void SkinSetUP()
    {
        _purchaseButton.GetComponentInChildren<TMP_Text>().text = _skin.Price + " Nectar";
        _purchaseButton.onClick.AddListener(delegate { ShopManager.Instance.PurchaseItem(_skin); });
        _itemSprite.sprite = _skin.Skin;
    }
}
