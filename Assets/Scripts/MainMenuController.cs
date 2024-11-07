using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject optionsCanvas;
    private Canvas menuCanvas;

    private void OnEnable()
    {
        // optionsCanvas.OnCloseOptions += ShowMenu;
    }
    
    private void OnDisable()
    {
        // optionsCanvas.OnCloseOptions -= ShowMenu;
    }

    public void OnPressPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void OnPressOptions()
    {
        menuCanvas.enabled = false;
        optionsCanvas.SetActive(true);
    }

    public void ShowMenu()
    {
        menuCanvas.enabled = true;
    }

    public void OnPressQuit()
    {
        Application.Quit();
    }
}
