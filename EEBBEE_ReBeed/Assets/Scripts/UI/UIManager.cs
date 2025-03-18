using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class UIManager : MonoBehaviour, IObserver<Score>, IObserver<CollectableData>
{
    [Header("Score")]
    [SerializeField] private TMP_Text _scoreText;

    [Header("Affect")]
    [SerializeField] private GameObject _affectDisplay;
    [SerializeField] private TMP_Text _affectTitleText;
    [SerializeField] private TMP_Text _effectTimeText;

    [Header("MobileControls")]
    [SerializeField] private GameObject _mobileUI;

    public void ItemAltered(Score type, int count)
    {
        _scoreText.text = type.ScoreCount.ToString();
    }

    public void ItemAltered(CollectableData type, int count)
    {
        if (count > 0)
        {
            _affectDisplay.SetActive(true);
            string affectTitleText = type.ToString();
            affectTitleText = affectTitleText.Substring(0, CheckForSpace(affectTitleText));
            _affectTitleText.text = affectTitleText + " time remaining:";

            _effectTimeText.text = count.ToString();
        }
        else
        {
            _affectDisplay.SetActive(false);
        }
    }

    private int CheckForSpace(string textToCheck)
    {
        int stringLength = 0;
        foreach(char currentChar in textToCheck)
        {
            if(currentChar == ' ')
            {
                return stringLength;
            }
            stringLength++;
        }
        return stringLength;
    }    

    public void ItemRemoved(Score type)
    {
        throw new System.NotImplementedException();
    }

    public void ItemRemoved(CollectableData type)
    {
        throw new System.NotImplementedException();
    }

    public void NewItemAdded(Score type)
    {
        throw new System.NotImplementedException();
    }

    public void NewItemAdded(CollectableData type)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        ScoreManager.Instance.RegisterObserver(this);
        AffectManager.Instance.RegisterObserver(this);
        _affectDisplay.SetActive(false);
        CheckIfMobile();
    }

    private void CheckIfMobile()
    {
        if (UnityEngine.Device.SystemInfo.deviceType == DeviceType.Handheld)
        {
            Debug.Log("Handheld");
            _mobileUI.SetActive(true);
        }
        else if (UnityEngine.Device.SystemInfo.deviceType == DeviceType.Desktop)
        {
            Debug.Log("Desktop");
            _mobileUI.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
