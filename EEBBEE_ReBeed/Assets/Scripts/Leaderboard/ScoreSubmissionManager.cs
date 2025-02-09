using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ScoreSubmissionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _inputScore;
    [SerializeField] private TMP_InputField _inputName;

    public UnityEvent<string, int> SubmitScoreEvent;
    public void SubmitScore()
    {
        SubmitScoreEvent.Invoke(_inputName.text, int.Parse(_inputScore.text));
    }
}
