using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectables", menuName = "ScriptableObjects/Collectables/HoneyCollectable", order = 3)] //allows this to be made as a scriptable object in the editor
public class HoneyCollectable : CollectableData
{
    [SerializeField] private int _affectTime; //varaible for how long the effect lasts
    [SerializeField] private int _scoreMultiplier = 2; //variable for how much points collected are multiplied by
    [SerializeField] private string _honeyMultiplierKey = "HoneyMultiplier"; //variable to hold a string key for the collectable


    //function that triggers when player collects a Honey object, using a given object to affect
    public override void OnCollect(GameObject objectToAffect)
    {
        objectToAffect.GetComponentInParent<ObstacleBox>().UpdateObstacle(objectToAffect); //update the obstacle to remove this collectable from the list of active collectables
        HoneyAffect(); //function that sets and triggers the effect
        objectToAffect.SetActive(false); //disables the honey object
    }

    //function that sets and triggers the effect
    private void HoneyAffect()
    {
        AffectManager.Instance.SetCurrentEffect(HoneyCoroutine()); //set the current effect to the honey's coroutine effect
        AffectManager.EndingEffect = EndEffect; //set the end effect to the honey's end effect
        AffectManager.Instance.StartEffect(this); //start the current effect
    }

    //function controlling what happens while the honey effect is active
    private IEnumerator HoneyCoroutine()
    {
        Time.timeScale = 0.5f; //set the game speed to half the normal speed
        ScoreManager.Instance.AddMultiplier(_honeyMultiplierKey, _scoreMultiplier); //set the score multiplier so player gets double points
        int timeSpent = 0; //temp variable for how long affect has been active

        while(timeSpent < _affectTime) //if the time for the effect hasn't reached the maxmimum effect time
        {
            AffectManager.Instance.AdjustAffectTime(_affectTime - timeSpent, this); //adjust the effect time in the effect manager
            timeSpent++; //increase the time spent by a second
            yield return new WaitForSecondsRealtime(1); //wait for a second
        }

        AffectManager.Instance.AdjustAffectTime(_affectTime - timeSpent, this); //let the AffectManager know that the effect time has changed
        EndEffect(); //end the effect as the time has now run out
    }

    //function to control what happens at the end of the effect
    private void EndEffect()
    {
        Time.timeScale = 1f; //reset the game speed
        ScoreManager.Instance.RemoveMultiplier(_honeyMultiplierKey); //remove the honey multiplier from the score multipliers
    }
}
