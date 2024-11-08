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
        sfxVolPercent.text = (int)(value * 100) + "%";
        AudioManager.Instance.SetSFXVolume(value);
    }

    private void HandleMusicVolumeChange(float value)
    {
        musicVolPercent.text = (int)(value * 100) + "%";
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
        sfxVolPercent.text = AudioManager.Instance.GetSFXVolumePercentage() + "%";
        sfxVolumeSlider.value = AudioManager.Instance.GetSFXVolumeNormalized();
        
        musicVolPercent.text = AudioManager.Instance.GetMusicVolumePercentage() + "%";
        musicVolumeSlider.value = AudioManager.Instance.GetMusicVolumeNormalized();;
    }
   
}
