using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveableObjectPrefab
{
    public ObjectType Type;
    public GameObject Prefab;

    public SaveableObjectPrefab(ObjectType type)
    {
        this.Type = type;
    }
}
