using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Obstacles", menuName = "ScriptableObjects/Obstacles", order = 1)]
public class Obstacle : ScriptableObject
{
    public List<SaveableObjectInfo> ObjectList = new List<SaveableObjectInfo>();
    public List<Vector2> FreeSpace = new List<Vector2>();

    public void ClearObjectList()
    {
        ObjectList.Clear();
    }

    public void AddObjectInfo(SaveableObject saveableObject)
    {
        SaveableObjectInfo saveableObjectInfo = new SaveableObjectInfo(saveableObject);
        if(FreeSpace.Contains(saveableObjectInfo.Position))
        {
            FreeSpace.Remove(saveableObjectInfo.Position);
        }
        ObjectList.Add(saveableObjectInfo);
    }

    public void SetFreeSpace()
    {
        FreeSpace.Clear();
        for(int x = -13; x < 14; x++)
        {
            for(int y = -6; y < 7; y++)
            {
                FreeSpace.Add(new Vector2(x, y));
            }
        }
    }
}
