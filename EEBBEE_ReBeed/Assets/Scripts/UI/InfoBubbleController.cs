using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoBubbleController : MonoBehaviour
{
    //Singleton pattern
    #region Singleton
    private static InfoBubbleController _instance;
    public static InfoBubbleController Instance
    {
        get //making sure that a game manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InfoBubbleController>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("InfoBubbleController");
                _instance = go.AddComponent<InfoBubbleController>();
            }
            return _instance;
        }
    }
    #endregion

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

    [SerializeField] private GameObject _infoBubble;
    [SerializeField] private TextMeshProUGUI _infoBubbleText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceInfoBubble(Vector3 position, string message)
    {
        _infoBubble.SetActive(true);
        _infoBubble.transform.position = position;
        _infoBubbleText.text = message;
    }

    public void HideInfoBubble()
    {
        _infoBubble.SetActive(false);
    }
}
