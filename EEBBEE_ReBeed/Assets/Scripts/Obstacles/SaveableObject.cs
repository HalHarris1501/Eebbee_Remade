using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableObject : MonoBehaviour
{
    public ObjectType ObjectType; //the enum for the type of object that this is
    public GameObject Background; //a gameobject that can be activated in settings, making all objects more visible
}

//enum for all different object types
public enum ObjectType
{
    Wall,
    URCorner,
    ULCorner,
    BRCorner,
    BLCorner,
    Flower,
    Smoker,
    Honey,
    Nectar,
    EndingNectar,
    Hive
}
