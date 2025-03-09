using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Singleton pattern
    #region Singleton
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get //making sure that a game manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("AudioManager");
                _instance = go.AddComponent<AudioManager>();
            }
            return _instance;
        }
    }
    #endregion

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundEffectsSource;

    [Header("Audio Settings")]
    [SerializeField] private bool _musicEnabled = true;
    [SerializeField] private float _musicVolume;
    [SerializeField] private bool _sfxEnabled = true;
    [SerializeField] private float _sfxVolume;

    [Header("Audio clips")]
    [SerializeField] private List<AudioStorage> _audioClips;

    // Start is called before the first frame update
    void Start()
    {
        PlayMusic(AudioTag.MenuMusic);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlaySoundAffect(AudioTag tag, bool overrideSound)
    {
        if (!_sfxEnabled)
        {
            return;
        }

        if(_soundEffectsSource.isPlaying == true && overrideSound == false)
        {
            return;
        }

        if (FindClip(tag) != null)
        {
            _soundEffectsSource.clip = FindClip(tag);
            _soundEffectsSource.Play();
        }
        else
        {
            Debug.Log("No sound affect returned");
        }
    }

    public void PlayMusic(AudioTag tag)
    {
        if(!_musicEnabled)
        {
            return;
        }

        if (FindClip(tag) != null)
        {
            _musicSource.clip = FindClip(tag);
            _musicSource.Play();
        }
        else
        {
            Debug.Log("No sound affect returned");
        }
    }

    public AudioClip FindClip(AudioTag tag)
    {
        AudioClip soundToPlay = null;

        foreach(AudioStorage clip in _audioClips)
        {
            if(clip.Tag == tag)
            {
                soundToPlay = clip.Clip;
            }
        }

        return soundToPlay;
    }
}
