using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectables", menuName = "ScriptableObjects/Collectables/SmokerCollectable", order = 2)]
public class SmokerCollectable : CollectableData
{
    [SerializeField] private int _affectTime;
    public override void OnCollect(GameObject objectToAffect)
    {
        SmokerAffect();
        objectToAffect.SetActive(false);
    }

    private void SmokerAffect()
    {
        AffectManager.Instance.CurrentEffect = SmokerCoroutine();
        AffectManager.Instance.StartEffect();
    }

    private IEnumerator SmokerCoroutine()
    {
        PlayerMovement.InvertControls();
        int timeSpent = 0;

        while(timeSpent < _affectTime)
        {
            AffectManager.Instance.EffectTimeRemaining = _affectTime - timeSpent;
            timeSpent++;
            yield return new WaitForSeconds(1);
        }
        PlayerMovement.SetDefaultControls();
    }
}
