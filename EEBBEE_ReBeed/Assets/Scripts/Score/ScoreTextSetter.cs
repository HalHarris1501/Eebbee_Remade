using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextSetter : MonoBehaviour, IObserver<SaveManager>
{
    [Header("Values")]
    [SerializeField] private int _previousWinScore = 0;
    [SerializeField] private int _totalScore = 0;
    [SerializeField] private int _highScore = 0;
    [SerializeField] private int _previousRunScore = 0;

    [Header("Text")]
    [SerializeField] private TMP_Text _previousScoreText;
    [SerializeField] private TMP_Text _totalScoreText;
    [SerializeField] private TMP_Text _shopTotalScoreText;
    [SerializeField] private TMP_Text _highScoreText;
    [SerializeField] private TMP_Text _leaderboardMenuHighScoreText;
    [SerializeField] private TMP_Text _previousRunScoreText;
    private void Awake()
    {
        SaveManager.Instance.RegisterObserver(this);
        UpdateUI();
    }

    public void UpdateUI()
    {
        _previousWinScore = ScoreStorage.current.PreviousWinScore;
        _previousScoreText.text = _previousWinScore.ToString();

        _totalScore = ScoreStorage.current.TotalScore;
        _totalScoreText.text = _totalScore.ToString();
        _shopTotalScoreText.text = _totalScore.ToString();

        _highScore = ScoreStorage.current.HighScore;
        _highScoreText.text = _highScore.ToString();
        _leaderboardMenuHighScoreText.text = _highScore.ToString();

        _previousRunScore = ScoreStorage.current.PreviousRunScore;
        _previousRunScoreText.text = _previousRunScore.ToString();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewItemAdded(SaveManager type)
    {
        throw new System.NotImplementedException();
    }

    public void ItemRemoved(SaveManager type)
    {
        throw new System.NotImplementedException();
    }

    public void ItemAltered(SaveManager type, int count)
    {
        UpdateUI();
        //Debug.Log("Updating UI with save data");
    }
}
