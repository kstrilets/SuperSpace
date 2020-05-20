using UnityEngine.Audio;
using System;
using UnityEngine;


// AudioManager with Music and SFX mixers

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    
    public Sound[] sounds;   // --- an array of music and sounds

    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;


    // checking if one instance of AudioManager exists
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // filling sounds array with audioclip, corresponding audiomixergroup output, volume level
        // pitch level, and cheking if its looping

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.output;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    // public method to play sound

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        s.source.Play();
    }


    // public method to stop sound

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

   // public method to set Music Mixer Group volume nonlinear 

    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }


    // public method to set Music Mixer Group volume nonlinear 

    public void SetSFXVolume(float volume)
    {
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }
}
