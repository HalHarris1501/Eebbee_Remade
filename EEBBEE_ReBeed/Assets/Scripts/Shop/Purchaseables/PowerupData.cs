using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PowerupType
{
    BeeHelper,
    NectarDoubler,
    Helmet,
}

[System.Serializable]
public class PowerupData
{
    public PowerupType PowerupType;
    public bool Active;
}
