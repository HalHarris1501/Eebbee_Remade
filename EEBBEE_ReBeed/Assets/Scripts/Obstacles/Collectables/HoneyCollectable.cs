using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectables", menuName = "ScriptableObjects/Collectables/HoneyCollectable", order = 3)]
public class HoneyCollectable : CollectableData
{
    [SerializeField] private int _affectTime;
    [SerializeField] private int _scoreMultiplier = 2;
    // Start is called before the first frame update
    public override void OnCollect(GameObject objectToAffect)
    {
        HoneyAffect();
        objectToAffect.SetActive(false);
    }

    private void HoneyAffect()
    {
        AffectManager.Instance.SetCurrentEffect(HoneyCoroutine());
        AffectManager.Instance.StartEffect(this);
    }

    private IEnumerator HoneyCoroutine()
    {
        Time.timeScale = 0.5f;
        ScoreManager.Instance.SetMultiplier(_scoreMultiplier);
        int timeSpent = 0;

        while(timeSpent < _affectTime)
        {
            AffectManager.Instance.AdjustAffectTime(_affectTime - timeSpent, this);
            timeSpent++;
            yield return new WaitForSeconds(1);
        }

        Time.timeScale = 1f;
        ScoreManager.Instance.SetMultiplier(1);
    }
}
