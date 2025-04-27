using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to contain and move obstacles
public class ObstacleBox : ParallaxMover //inherits from parallax mover
{
    public int ObstacleNumber; //variable to locally store the obstacle number held in this box

    //funtion that overrides the parallax mover base collision handling function
    public override void ResetterCollision()
    {
        base.ResetterCollision(); //keep base collision handling
        LoadObstacle(); //load a new obstacle
    }

    //function to load a new obstacle
    private void LoadObstacle()
    {
        if (this.gameObject.layer == 0) //check the gameobject is on te correct layer (to ensure other objects don't accidentally spawn obstacles), then spawn a new obstacle using the obstacle manager
        {
            ObstacleManager.Instance.LoadObstacle();
        }
    }
     
    //function to update the current obstacle with the object that needs to be removed from it
    public void UpdateObstacle(GameObject objectToRemove)
    {
        GetComponentInParent<ObstacleManager>().UpdateObstacle(ObstacleNumber, objectToRemove.GetComponent<SaveableObject>()); //call function in Obstacle manager to remove the object
    }
}
