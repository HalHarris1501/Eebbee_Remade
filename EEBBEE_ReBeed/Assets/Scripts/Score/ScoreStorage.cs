using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storage", menuName = "ScriptableObjects/Storage/ScoreStorage", order = 1)]
public class ScoreStorage : ScriptableObject
{
    public int PreviousScore = 0;
    public int TotalScore = 0;
    public int HighScore = 0;
}
