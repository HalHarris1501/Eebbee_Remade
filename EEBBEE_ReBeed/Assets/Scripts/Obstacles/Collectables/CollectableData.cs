using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableData : ScriptableObject
{
    public Vector3 StartPosition;
    public virtual void OnCollect(GameObject objectToAffect) { }
}
