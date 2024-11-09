using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectables", menuName = "ScriptableObjects/Collectables/NectarCollectable", order = 1)]
public class NectarCollectable : CollectableData
{
    public override void OnCollect(GameObject nectar)
    {
        ScoreManager.Instance.AlterScore(1);
        nectar.GetComponentInParent<ObstacleBox>().UpdateObstacle(nectar);
        nectar.SetActive(false);
    }
}
