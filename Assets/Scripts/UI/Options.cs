using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI sfxVolPercent;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TextMeshProUGUI musicVolPercent;

    public event Action OnCloseOptions;

    private void OnEnable()
    {
        UpdateValues();
        
        sfxVolumeSlider.onValueChanged.AddListener(HandleSFXVolumeChange);
        musicVolumeSlider.onValueChanged.AddListener(HandleMusicVolumeChange);
    }
    
    private void OnDisable()
    {
        sfxVolumeSlider.onValueChanged.RemoveListener(HandleSFXVolumeChange);
        musicVolumeSlider.onValueChanged.RemoveListener(HandleMusicVolumeChange);
    }
    
    private void HandleSFXVolumeChange(float value)
    {
        sfxVolPercent.text = value.ToString() + "%";
        AudioManager.Instance.SetSFXVolume(value);
    }

    private void HandleMusicVolumeChange(float value)
    {
        musicVolPercent.text = value.ToString() + "%";
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void ShowScreen()
    {
        gameObject.SetActive(true);
    }

    public void HideScreen()
    {
        OnCloseOptions?.Invoke();
        gameObject.SetActive(false);
    }

    void UpdateValues()
    {
        int sfxVol = (int) AudioManager.Instance.GetSFXVolume();
        int musicVol = (int) AudioManager.Instance.GetMusicVolume();
        
        sfxVolPercent.text = sfxVol + "%";
        sfxVolumeSlider.value = sfxVol;
        
        musicVolPercent.text = musicVol + "%";
        musicVolumeSlider.value = musicVol;
    }
   
}
