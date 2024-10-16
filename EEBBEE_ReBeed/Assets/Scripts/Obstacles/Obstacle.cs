using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObstacle", menuName = "New Obstacle")]
public class Obstacle : ScriptableObject
{
    [SerializeField]
    public List<SaveableObjectInfo> ObjectList = new List<SaveableObjectInfo>();

    public void ClearObjectList()
    {
        ObjectList.Clear();
    }

    public void AddObjectInfo(SaveableObject saveableObject)
    {
        SaveableObjectInfo saveableObjectInfo = new SaveableObjectInfo(saveableObject);
        ObjectList.Add(saveableObjectInfo);
    }
}
