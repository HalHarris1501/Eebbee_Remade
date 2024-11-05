using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBox : ParallaxMover
{
    public int ObstacleNumber;    

    public override void ResetterCollision()
    {
        base.ResetterCollision();
        LoadObstacle();
    }

    private void LoadObstacle()
    {
        if (this.gameObject.layer == 0)
        {
            ObstacleManager.Instance.LoadObstacle();
        }
    }

    public void UpdateObstacle(GameObject objectToRemove)
    {
        GetComponentInParent<ObstacleManager>().UpdateObstacle(ObstacleNumber, objectToRemove.GetComponent<SaveableObject>());
    }
}
