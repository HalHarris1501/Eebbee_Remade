using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableData : ScriptableObject
{
    public virtual void OnCollect(GameObject objectToAffect) { }
}
