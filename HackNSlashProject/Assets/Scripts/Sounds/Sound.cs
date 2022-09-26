using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip[] clips;
    public AudioMixerGroup mixer;

    [Range(0f, 1f)]
    public float volume;
    [Range(-3f, 3f)]
    public float pitch;
    [Range(0f, 1f)]
    public float spacialBlend = 1f;
    [Space]
    public float maxSoundDistance = 10;
    [Space]
    public bool loop;
    public bool playOnStart;
    public bool playRandomClip;

    [HideInInspector]
    public AudioSource source;
}
