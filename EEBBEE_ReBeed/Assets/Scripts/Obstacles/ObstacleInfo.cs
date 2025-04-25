using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to save all information for obstacles
[System.Serializable] //allows the obstacles to be serialized, and allows them to be view in the Editor
public class ObstacleInfo
{
    public ObstacleInfo(Obstacle obstacle) //constructor for ObstacleInfo using a given obstacle
    {
        foreach(SaveableObjectInfo saveableObject in obstacle.ObjectList) //loop though all SaveableObjects in the obstacle
        {
            ObjectList.Add(saveableObject); //add SaveableObject info to the object list
        }

        foreach(Vector2 freespace in obstacle.FreeSpace) //loop through all free space positions in the Obstacle
        {
            FreeSpace.Add(freespace); //add freespace positions to the FreeSpace list
        }
    }

    public List<SaveableObjectInfo> ObjectList = new List<SaveableObjectInfo>(); //list for storing all info about all SaveableObjects in the obstacle
    public List<Vector2> FreeSpace = new List<Vector2>(); //list for storing all positions of free space in the obstacle
}
