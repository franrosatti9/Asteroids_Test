using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Options options;
    private Canvas menuCanvas;

    private void Awake()
    {
        menuCanvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        options.OnCloseOptions += ShowMenu;
    }
    
    private void OnDisable()
    {
        options.OnCloseOptions -= ShowMenu;
    }

    public void OnPressPlay()
    {
        //GameManager.Instance.SetState(GameState.Waiting); // Think a better way to do this
        SceneManager.LoadScene(1);
    }

    public void OnPressOptions()
    {
        menuCanvas.enabled = false;
        options.ShowScreen();
    }

    public void OnPressQuit()
    {
        Application.Quit();
    }
    
    public void ShowMenu()
    {
        menuCanvas.enabled = true;
    }
}
