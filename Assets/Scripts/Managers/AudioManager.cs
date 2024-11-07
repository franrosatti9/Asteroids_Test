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
    
    [SerializeField] SoundEffectSO[] soundEffects;
    
    Dictionary<SFXType, SoundEffectSO> _sfxDictionary = new Dictionary<SFXType, SoundEffectSO>();

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
        
        SetupSFXDictionary();
    }

    private void Start()
    {
        PlayMenuMusic();
    }

    #region Enable/Disable
    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameMusic;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= HandleGameMusic;
    }
    #endregion

    private void HandleGameMusic(GameState newState)
    {
        if(newState == GameState.Playing) PlayGameplayMusic();
        else if(newState is GameState.Menu or GameState.Waiting) PlayMenuMusic();
    }

    void SetupSFXDictionary()
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            if (!_sfxDictionary.TryAdd(soundEffects[i].Type, soundEffects[i]))
            {
                Debug.LogWarning("Duplicated SFX Type in Audio Manager array!");
            }
        }
    }

    #region Play Clip methods
    void PlayMenuMusic()
    {
        if (musicAudioSource.clip == menuMusic) return; // Don't override music
        musicAudioSource.clip = menuMusic;
        musicAudioSource.Play();
    }

    void PlayGameplayMusic()
    {
        if(musicAudioSource.clip == gameplayMusic) return; // Don't override music
        musicAudioSource.clip = gameplayMusic;
        musicAudioSource.Play();
    }
    public void PlaySFX(SFXType type)
    {
        sfxAudioSource.pitch = 1f;
        sfxAudioSource.PlayOneShot(_sfxDictionary[type].GetRandomClip());
    }
    
    public void PlaySFXRandomPitch(SFXType type)
    {
        sfxAudioSource.pitch = Random.Range(0.9f, 1.1f);
        sfxAudioSource.PlayOneShot(_sfxDictionary[type].GetRandomClip());
    }
    #endregion

    #region Volume Settings
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
    
    #endregion
}

public enum SFXType
{
    DamageAsteroid,
    DestroyAsteroid,
    Death,
    Shoot,
    HighScore
}
