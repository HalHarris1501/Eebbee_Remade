using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleInfo
{
    public ObstacleInfo(Obstacle obstacle)
    {
        foreach(SaveableObjectInfo saveableObject in obstacle.ObjectList)
        {
            ObjectList.Add(saveableObject);
        }

        foreach(Vector2 freespace in obstacle.FreeSpace)
        {
            FreeSpace.Add(freespace);
        }
    }

    public List<SaveableObjectInfo> ObjectList = new List<SaveableObjectInfo>();
    public List<Vector2> FreeSpace = new List<Vector2>();
}
