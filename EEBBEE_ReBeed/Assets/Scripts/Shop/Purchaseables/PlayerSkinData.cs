using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SkinName
{
    Default,
    Inverted,
    Bee,
    RGBee,
}

[System.Serializable]
public class PlayerSkinData
{
    public SkinName SkinName;
    public bool Owned;
    public int Price;
}
