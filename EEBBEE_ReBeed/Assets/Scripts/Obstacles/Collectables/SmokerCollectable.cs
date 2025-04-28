using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectables", menuName = "ScriptableObjects/Collectables/SmokerCollectable", order = 2)] //allows this to be made as a scriptable object in the editor
public class SmokerCollectable : CollectableData
{
    [SerializeField] private int _affectTime; //variable for the max time the effect can last

    //function triggers when a smoker collectable is collected
    public override void OnCollect(GameObject objectToAffect)
    {
        objectToAffect.GetComponentInParent<ObstacleBox>().UpdateObstacle(objectToAffect); //remove the smoker from the active collectables list
        SmokerAffect(); //trigger the smoker effect
        objectToAffect.SetActive(false); //deactivate the smoker object
    }

    //function to set and trigger the smoker effect
    private void SmokerAffect()
    {
        AffectManager.Instance.SetCurrentEffect(SmokerCoroutine()); //set the current effect to the smoker effect
        AffectManager.EndingEffect = EndEffect; //set the end effect to the smoker end effect
        AffectManager.Instance.StartEffect(this); //start the current effect
    }

    //coroutine controlling what happens while the smoker effect is active
    private IEnumerator SmokerCoroutine()
    {
        PlayerMovement.InvertControls(); //invert the players controls
        int timeSpent = 0; //variable for how long the effect has been active

        while(timeSpent < _affectTime) //while there is still time left for the effect
        {
            AffectManager.Instance.AdjustAffectTime(_affectTime - timeSpent, this); //let the Affect manager know time has passed
            timeSpent++; //increase the time passed by 1 second
            yield return new WaitForSeconds(1); //wait for 1 second
        }

        AffectManager.Instance.AdjustAffectTime(_affectTime - timeSpent, this); //let the Affect manager know time has passed
        EndEffect(); //end the effect as it's time has run out
    }

    //function to control what happens when the smoker effect ends
    private void EndEffect()
    {
        PlayerMovement.SetDefaultControls(); //set the player's controls back to normal
    }
}
