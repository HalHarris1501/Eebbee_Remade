using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skins", menuName = "ScriptableObjects/Skins", order = 5)]
public class SkinObject : ScriptableObject
{
    public PlayerSkinData SkinData;
    public Sprite Skin;
    public int Price;
    public List<GameObject> Accessories;
    public List<AudioStorage> SoundEffects;
}
