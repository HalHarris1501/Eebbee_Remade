using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _names;
    [SerializeField] private List<TextMeshProUGUI> _scores;

    private string _publicLeaderboardKey = "f3c74bed869f2691e755fba4335f4fbd4844e40a41fa6c99c460f3445f4cf952";

    private void Start()
    {
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        Leaderboards.EEBBEE.GetEntries(((msg) =>
        {
            int loopLength = (msg.Length < _names.Count) ? msg.Length : _names.Count;
            for (int i = 0; i < loopLength; i++)
            {
                _names[i].text = msg[i].Username;
                _scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        Leaderboards.EEBBEE.UploadNewEntry(username, score, ((msg) =>
        {
            GetLeaderboard();
        }));
    }
}
