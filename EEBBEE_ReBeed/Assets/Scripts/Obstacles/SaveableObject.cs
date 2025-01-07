using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableObject : MonoBehaviour
{
    public ObjectType ObjectType;
}

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
