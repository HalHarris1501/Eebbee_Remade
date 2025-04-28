using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectables", menuName = "ScriptableObjects/Collectables/NectarCollectable", order = 1)] //allows this to be made as a scriptable object in the editor
public class NectarCollectable : CollectableData
{
    //function that triggers when player collects a nectar object, using a given object to affect
    public override void OnCollect(GameObject nectar)
    {
        ScoreManager.Instance.AlterScore(1); //increase the player's score by 1
        nectar.GetComponentInParent<ObstacleBox>().UpdateObstacle(nectar); //remove this collectable from the active collectables list
        nectar.SetActive(false); //deactivate this object
    }
}
