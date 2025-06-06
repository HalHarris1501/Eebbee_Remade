using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Powerups", menuName = "ScriptableObjects/Powerups", order = 6)]
public class PowerupObject : ScriptableObject
{
    public PowerupData PowerupData;
    public Sprite Sprite;
    public string Info;
    public int Price;
}
