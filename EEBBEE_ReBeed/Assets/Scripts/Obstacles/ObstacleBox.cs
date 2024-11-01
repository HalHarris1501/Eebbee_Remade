using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBox : ParallaxMover
{
    public List<SaveableObjectInfo> CurrentObstacle;

    public override void ResetterCollision()
    {
        base.ResetterCollision();
        CheckIfObstacle();
    }

    private void CheckIfObstacle()
    {
        if (this.gameObject.layer == 0)
        {
            ObstacleManager.Instance.LoadObstacle();
        }
    }

    public void UpdateObstacle(GameObject objectToRemove)
    {
        GetComponentInParent<ObstacleManager>().UpdateObstacle(objectToRemove, CurrentObstacle);
    }
}
