using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to hold all information for the saveable objects
[System.Serializable] //allows the SaveableObjectInfo to be serialized, and allows them to be view in the Editor
public class SaveableObjectInfo
{
    public SaveableObjectInfo(SaveableObject saveableObject) //constructor for saveable object info using a given SaveableObject
    {
        this.Type = saveableObject.ObjectType; //store the type of saveable object

        this.Position = saveableObject.transform.position; //store the position of the saveable object
    }

    public ObjectType Type; //variable to store the type of object

    public Vector3 Position; //variable to store the objects position
}
