using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    #region Singleton pattern

    private static MusicManager instance;

    public static MusicManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion
    public AudioSource _audioSource;
    public AudioClip _audio;
    public AudioClip _audio1;
    public AudioClip _audio2;

    public void OnAudio(AudioClip audio)
    {
        _audioSource.PlayOneShot(audio);
    }
    
}