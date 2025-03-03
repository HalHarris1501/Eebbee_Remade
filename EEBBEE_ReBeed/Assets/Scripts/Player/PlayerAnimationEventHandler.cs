using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    private void EndingAnimationFinished()
    {
        GameManager.Instance.LoadMenu();
    }
}
