using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveableObjectInfo
{
    public SaveableObjectInfo(SaveableObject saveableObject)
    {
        this.Type = saveableObject.ObjectType;

        this.Position = saveableObject.transform.position;
    }

    public ObjectType Type;

    public Vector3 Position;
}
