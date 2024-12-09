using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectables", menuName = "ScriptableObjects/Collectables/SmokerCollectable", order = 2)]
public class SmokerCollectable : CollectableData
{
    [SerializeField] private int _affectTime;
    public override void OnCollect(GameObject objectToAffect)
    {
        objectToAffect.GetComponentInParent<ObstacleBox>().UpdateObstacle(objectToAffect);
        SmokerAffect();
        objectToAffect.SetActive(false);
    }

    private void SmokerAffect()
    {
        AffectManager.Instance.SetCurrentEffect(SmokerCoroutine());
        AffectManager.EndingEffect = EndEffect;
        AffectManager.Instance.StartEffect(this);
    }

    private IEnumerator SmokerCoroutine()
    {
        PlayerMovement.InvertControls();
        int timeSpent = 0;

        while(timeSpent < _affectTime)
        {
            AffectManager.Instance.AdjustAffectTime(_affectTime - timeSpent, this);
            timeSpent++;            
            yield return new WaitForSeconds(1);
        }

        AffectManager.Instance.AdjustAffectTime(_affectTime - timeSpent, this);
        EndEffect();
    }

    private void EndEffect()
    {
        PlayerMovement.SetDefaultControls();        
    }
}
