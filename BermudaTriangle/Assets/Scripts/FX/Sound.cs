using UnityEngine.Audio;
using UnityEngine;
using System;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0, 1)]
    public float sfxVolume = 1;

    public bool isMusic = false;

    [HideInInspector]
    public float lastStartTime;
}