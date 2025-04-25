using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to store the SaveableObject game objects to be created at runtime
[System.Serializable] //allows the SaveableObjectPrefab to be serialized, and allows them to be view in the Editor
public class SaveableObjectPrefab
{
    public ObjectType Type; //variable to store the type of object
    public GameObject Prefab; //variable to store the gameobject for the SaveableObject

    public SaveableObjectPrefab(ObjectType type) //constructor for SaveableObjectPrefab usingh a given type of object
    {
        this.Type = type; //store the object type
    }
}
