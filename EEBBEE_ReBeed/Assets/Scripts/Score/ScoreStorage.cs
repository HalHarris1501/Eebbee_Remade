using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreStorage
{
    private static ScoreStorage _current;
    public static ScoreStorage current
    {
        get
        {
            if (_current == null)
            {
                _current = new ScoreStorage();
            }
            return _current;
        }
        set
        {
            if (value != null)
            {
                _current = value;
            }
        }
    }

    public int PreviousWinScore = 0;
    public int TotalScore = 0;
    public int HighScore = 0;
    public int PreviousRunScore = 0;
}
