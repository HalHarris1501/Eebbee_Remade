using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectables", menuName = "ScriptableObjects/Collectables/SmokerCollectable", order = 2)]
public class SmokerCollectable : CollectableData
{
    public float AffectTime;
    public override void OnCollect(GameObject objectToAffect)
    {


        objectToAffect.SetActive(false);
    }

    private void SmokerAffect()
    {
        if(AffectTime > 0)
        {
            AffectTime -= Time.deltaTime;
        }
    }
}
