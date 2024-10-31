using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class UIManager : MonoBehaviour, IObserver<Score>
{
    [SerializeField] private TMP_Text _scoreText;

    public void ItemAltered(Score type, int count)
    {
        _scoreText.text = type.ScoreCount.ToString();
    }

    public void ItemRemoved(Score type)
    {
        throw new System.NotImplementedException();
    }

    public void NewItemAdded(Score type)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        ScoreManager.Instance.RegisterObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
