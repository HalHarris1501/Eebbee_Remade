using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SkinName
{
    Default,
    Inverse,
    Naked,
    Horizontal,
    Bee,
    eeB,
    Orange,
    Blue,
    Green,
    Pink,
    Red,
    RGBee,
}

[System.Serializable]
public class PlayerSkinData
{
    public SkinName SkinName;
    public bool Owned;
}
