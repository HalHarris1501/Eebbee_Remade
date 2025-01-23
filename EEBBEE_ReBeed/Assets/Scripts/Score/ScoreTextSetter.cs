using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextSetter : MonoBehaviour
{
    [SerializeField] private int _previousScore = 0;
    [SerializeField] private TMP_Text _previousScoreText;
    [SerializeField] private ScoreStorage _scoreStorage;

    private void Awake()
    {
        _previousScore = _scoreStorage.PreviousScore;
        _previousScoreText.text = _previousScore.ToString();
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
