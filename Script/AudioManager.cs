using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] music, sfx;
    public AudioSource _musicSource, _sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //PlayMusic("LoopMusic");
    }

    public void PlayMusic(string name)
    {
        Sound snd = Array.Find(music, s => s.Name == name);
        if(snd == null)
        {
            Debug.Log("Нет такого трека");
        }
        else
        {
            _musicSource.clip = snd.AudioClip;
            _musicSource.Play();
        }
    }

    public void StopMusic(string name)
    {
        Sound snd = Array.Find(music, s => s.Name == name);
        if (snd == null)
        {
            Debug.Log("Нет такого трека");
        }
        else
        {
            _musicSource.clip = snd.AudioClip;
            _musicSource.Stop();
        }
    }

    public void PlaySfx(string name)
    {
        Sound snd = Array.Find(sfx, s => s.Name == name);
        if (snd == null)
        {
            Debug.Log("Нет аудио эффекта");
        }
        else
        {
            _sfxSource.PlayOneShot(snd.AudioClip);
        }
    }

    public void MuteSound()
    {
        _musicSource.mute = !_musicSource.mute;
        _sfxSource.mute = !_sfxSource.mute;
    }
}

