using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = s.mixer;

            //select first in array
            s.source.clip = s.clips[0];

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = s.spacialBlend;
            s.source.maxDistance = s.maxSoundDistance;

            s.source.loop = s.loop;
        }     
    }

    private void Start()
    {
        foreach (var sound in sounds)
        {
            if(sound.playOnStart)
                PlaySound(sound.name);
        }
    }

    public void PlaySound(string name)
    {
        //Sound s = Array.Find(sounds, sound => sound.name == name);

        foreach (var s in sounds)
        {
            if (s.name != name)
                continue;

            if (s.clips.Length == 0)
            {
                Debug.LogWarning("No AudioClip Specified for" + name);
                continue;
            }

            if (s.playRandomClip)
            {
                int randomIndex = Random.Range(0, s.clips.Length);
                Debug.Log(randomIndex);
                s.source.clip = s.clips[randomIndex];
            }
            s.source.Play();
        }
    }
}
