using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenusUIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _menus;

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
}
