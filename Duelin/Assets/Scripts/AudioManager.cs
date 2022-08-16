using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    void Awake(){
        if(instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Play(string name, float volume){
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        s.source.clip = s.clip;
        s.source.volume = volume;
        s.source.loop = s.Loop;
        Debug.LogError($"[AudioManager] Playing {s.name} at volume {volume}");
        s.source.Play();
    }
}
