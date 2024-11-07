using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup musicMixerGroup; // Maybe only AudioMixer is necessary
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;
    
    [SerializeField] private AudioClip damageAsteroidClip;
    [SerializeField] private AudioClip destroyAsteroidClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip highscoreClip;
    
    Dictionary<SFXType, AudioClip> sfxClips = new Dictionary<SFXType, AudioClip>();

    public static AudioManager Instance { get; private set;}
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        /* TODO: Move to scriptable object that gets random sounds
        
        foreach(sfx in sfxSOList){
            Populate dictionary
            
            sfxDict[sfx.Type] = sfx;
            
            sfx.GetSFX
        }
        
        
        */

        sfxClips[SFXType.DamageAsteroid] = damageAsteroidClip;
        sfxClips[SFXType.DestroyAsteroid] = destroyAsteroidClip; 
        sfxClips[SFXType.Death] = deathClip;
        sfxClips[SFXType.Shoot] = shootClip;
        sfxClips[SFXType.HighScore] = highscoreClip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(SFXType type)
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(sfxClips[type]);
    }
    
    public void PlaySFXRandomPitch(SFXType type)
    {
        sfxAudioSource.pitch = Random.Range(0.9f, 1.1f);
        sfxAudioSource.PlayOneShot(sfxClips[type]);
    }

    public void SetMusicVolume(float volume)
    {
        volume = math.remap(0, 100, -80, 0, volume);
        musicMixerGroup.audioMixer.SetFloat("MusicVol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        volume = math.remap(0, 100, -80, 0, volume);
        musicMixerGroup.audioMixer.SetFloat("SFXVol", volume);
    }

    public float GetMusicVolume()
    {
        musicMixerGroup.audioMixer.GetFloat("MusicVol", out var vol);
        vol = math.remap(-80, 0, 0, 100, vol);
        return vol;
    }

    public float GetSFXVolume()
    {
        musicMixerGroup.audioMixer.GetFloat("SFXVol", out var vol);
        vol = math.remap(-80, 0, 0, 100, vol);
        return vol;
    }
}

public enum SFXType
{
    DamageAsteroid,
    DestroyAsteroid,
    Death,
    Shoot,
    HighScore
}
