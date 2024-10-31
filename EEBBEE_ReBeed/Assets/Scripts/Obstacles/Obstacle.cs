using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObstacle", menuName = "New Obstacle")]
public class Obstacle : ScriptableObject
{
    [SerializeField] public List<SaveableObjectInfo> ObjectList = new List<SaveableObjectInfo>();
    [SerializeField] public List<Vector2> FreeSpace = new List<Vector2>();

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
