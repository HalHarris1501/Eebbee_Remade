using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Storage", menuName = "ScriptableObjects/Storage/ScoreStorage", order = 1)]
public class ScoreStorage : ScriptableObject
{
    public int PreviousWinScore = 0;
    public int TotalScore = 0;
    public int HighScore = 0;
    public int PreviousRunScore = 0;
}
