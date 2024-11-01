using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBox : ParallaxMover
{
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
}
