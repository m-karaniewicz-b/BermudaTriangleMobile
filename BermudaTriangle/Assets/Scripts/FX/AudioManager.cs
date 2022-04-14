using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    public const float GLOBAL_SFX_COOLDOWN = 0.15f;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    public Sound[] sounds;
    
    private void Awake()
    {
        musicSource = new GameObject().AddComponent<AudioSource>();
        musicSource.transform.parent = transform;
        musicSource.volume = 0.5f;
        musicSource.spatialBlend = 0;
        musicSource.loop = true;

        sfxSource = new GameObject().AddComponent<AudioSource>();
        sfxSource.transform.parent = transform;
        sfxSource.volume = 0.5f;
        sfxSource.spatialBlend = 0;
    }

    //System.Single instead of float to receive value from UI UnityEvent
    public void SetSFXVolume(System.Single volume)
    {
        sfxSource.volume = volume;
    }

    public void SetMusicVolume(System.Single volume)
    {
        musicSource.volume = volume;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError($"No sound named \"{name}\".");
            return;
        }

        //Debug.Log($"Playing {name}");
        if (!s.isMusic)
        {
            if(Time.time - s.lastStartTime > GLOBAL_SFX_COOLDOWN)
            {
                sfxSource.PlayOneShot(s.clip, s.sfxVolume);
                s.lastStartTime = Time.time;
            }
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }


    }
}
