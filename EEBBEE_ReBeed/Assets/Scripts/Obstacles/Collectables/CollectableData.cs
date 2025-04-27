using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for all collectable objects
public class CollectableData : ScriptableObject
{
    public virtual void OnCollect(GameObject objectToAffect) { } //overridable function that controls what happens when the object is collected
}
