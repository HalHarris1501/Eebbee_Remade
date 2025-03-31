using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItemPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item Data")]
    [SerializeField] private SkinObject _skin;
    [SerializeField] private PowerupObject _powerup;

    [Header("Child References")]
    [SerializeField] private TMP_Text _nameText;
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

    public void UpdateUI()
    {
        if(_skin != null)
        {
            UpdateUI(_skin);
        }
        if(_powerup != null)
        {
            UpdateUI(_powerup);
        }
    }

    public void UpdateUI(SkinObject skin)
    {
        if (_skin.SkinData.Owned)
        {
            _purchaseButton.interactable = true;
            _purchaseButton.onClick.RemoveAllListeners();
            _purchaseButton.onClick.AddListener(delegate { ShopManager.Instance.SetPlayerSkin(_skin); });
            _purchaseButton.GetComponentInChildren<TMP_Text>(true).text = "Select";
        }
        else if (!_skin.SkinData.Owned)
        {
            _purchaseButton.interactable = true;
            _purchaseButton.onClick.RemoveAllListeners();
            _nameText.text = UnderscoreRemover(_skin.SkinData.SkinName.ToString());
            _purchaseButton.GetComponentInChildren<TMP_Text>(true).text = _skin.Price + " Nectar";
            _purchaseButton.onClick.AddListener(delegate { ShopManager.Instance.PurchaseItem(_skin, this); });
            _itemSprite.sprite = _skin.Skin;
        }

        if (PlayerData.current.CurrentSkinData.SkinName == _skin.SkinData.SkinName)
        {
            _purchaseButton.interactable = false;
            _purchaseButton.GetComponentInChildren<TMP_Text>(true).text = "Selected!";
        }
    }

    public void UpdateUI(PowerupObject powerup)
    {        
        if (_powerup.PowerupData.Active)
        {
            _purchaseButton.interactable = false;
            _purchaseButton.GetComponentInChildren<TMP_Text>().text = "Active!";
        }
        else
        {
            _purchaseButton.interactable = true;
            _nameText.text = UnderscoreRemover(_powerup.PowerupData.PowerupType.ToString());
            _purchaseButton.GetComponentInChildren<TMP_Text>(true).text = _powerup.Price + " Nectar";
            _purchaseButton.onClick.AddListener(delegate { ShopManager.Instance.PurchaseItem(_powerup, this); });
            _itemSprite.sprite = _powerup.Sprite;
        }
    }

    private void PowerUpSetUp()
    {
        _nameText.text = UnderscoreRemover(_powerup.PowerupData.PowerupType.ToString());
        _purchaseButton.GetComponentInChildren<TMP_Text>(true).text = _powerup.Price + " Nectar";
        _purchaseButton.onClick.AddListener(delegate { ShopManager.Instance.PurchaseItem(_powerup, this);  });
        _itemSprite.sprite = _powerup.Sprite;
        UpdateUI(_powerup);
    }

    private void SkinSetUP()
    {
        _nameText.text = UnderscoreRemover(_skin.SkinData.SkinName.ToString());
        _purchaseButton.GetComponentInChildren<TMP_Text>(true).text = _skin.Price + " Nectar";
        _purchaseButton.onClick.AddListener(delegate { ShopManager.Instance.PurchaseItem(_skin, this); });
        _itemSprite.sprite = _skin.Skin;
        UpdateUI(_skin);
    }

    private string UnderscoreRemover(string stringToCheck)
    {
        string newString = "";
        for (int i = 0; i < stringToCheck.Length; i++)
        {
            if(stringToCheck[i] == '_')
            {
                newString += ' ';
            }
            else
            {
                newString += stringToCheck[i];
            }            
        }
        return newString;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_powerup != null)
        {
            InfoBubbleController.Instance.PlaceInfoBubble(this.transform.position, _powerup.Info);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_powerup != null)
        {
            InfoBubbleController.Instance.HideInfoBubble();
        }
    }
}
