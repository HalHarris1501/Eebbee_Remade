using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenusUIManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject[] _menus;

    [Header("Confimation Menu")]
    [SerializeField] private GameObject _confirmationMenu;
    [SerializeField] private TMP_Text _confimationMenuText;
    [SerializeField] private string _resetDataMenuMessage;
    [SerializeField] private Button _confirmButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectMenu(GameObject chosenMenu)
    {
        foreach(GameObject menu in _menus)
        {
            menu.SetActive(false);
        }

        chosenMenu.SetActive(true);
    }

    public void ResetDataPressed()
    {
        _confirmationMenu.SetActive(true);
        _confimationMenuText.text = _resetDataMenuMessage;
        _confirmButton.onClick.AddListener(delegate { SaveManager.Instance.ResetData(); });
    }
}
