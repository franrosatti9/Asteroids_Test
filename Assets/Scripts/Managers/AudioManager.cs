using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    
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
}

public enum SFXType
{
    DamageAsteroid,
    DestroyAsteroid,
    Death,
    Shoot,
    HighScore
}
