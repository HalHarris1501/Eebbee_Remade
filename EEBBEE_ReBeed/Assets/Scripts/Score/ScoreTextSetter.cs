using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextSetter : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private int _previousScore = 0;
    [SerializeField] private int _totalScore = 0;
    [SerializeField] private int _highScore = 0;
    [SerializeField] private ScoreStorage _scoreStorage;

    [Header("Text")]
    [SerializeField] private TMP_Text _previousScoreText;
    [SerializeField] private TMP_Text _totalScoreText;
    [SerializeField] private TMP_Text _highScoreText;

    private void Awake()
    {
        _previousScore = _scoreStorage.PreviousScore;
        _previousScoreText.text = _previousScore.ToString();

        _totalScore = _scoreStorage.TotalScore;
        _totalScoreText.text = _totalScore.ToString();

        _highScore = _scoreStorage.HighScore;
        _highScoreText.text = _highScore.ToString();
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
