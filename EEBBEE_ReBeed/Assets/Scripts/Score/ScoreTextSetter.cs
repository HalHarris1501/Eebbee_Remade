using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextSetter : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private int _previousScore = 0;
    [SerializeField] private int _totalScore = 0;
    [SerializeField] private ScoreStorage _scoreStorage;

    [Header("Text")]
    [SerializeField] private TMP_Text _previousScoreText;
    [SerializeField] private TMP_Text _totalScoreText;

    private void Awake()
    {
        _previousScore = _scoreStorage.PreviousScore;
        _previousScoreText.text = _previousScore.ToString();

        _totalScore = _scoreStorage.TotalScore;
        _totalScoreText.text = _totalScore.ToString();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
