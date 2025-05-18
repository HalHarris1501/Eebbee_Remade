using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    private void StartingAnimationStarted()
    {
        AudioManager.Instance.PlayMusic(AudioTag.GameMusic);
        AudioManager.Instance.PlaySoundAffect(AudioTag.StartSound, true);
    }
    private void StartingAnimationFinished()
    {
        GameManager.Instance.StartGame();
    }
    private void WiningAnimationStarted()
    {
        AudioManager.Instance.PlaySoundAffect(AudioTag.WinSound, true);
    }
    private void DeathAnimationStarted()
    {
        AudioManager.Instance.PlaySoundAffect(AudioTag.DeathSound, true);
    }
    private void DeathExplosionTrigger()
    {
        ParticleEffectsManager.Instance.TriggerDeathExplosion();
    }
    private void EndingAnimationFinished()
    {
        AudioManager.Instance.PlayMusic(AudioTag.MenuMusic);
        GameManager.Instance.LoadMenu();
    }
}
