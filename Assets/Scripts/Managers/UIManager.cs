using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private PauseMenuController pauseMenu;

    public static UIManager Instance { get; private set;}
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGamePaused += ShowPauseMenu;
        GameManager.Instance.OnGameResumed += HidePauseMenue;
        GameManager.Instance.OnScoreUpdated += UpdateGameScore;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnScoreUpdated -= UpdateGameScore;
    }

    void ShowPauseMenu()
    {
        pauseMenu.ShowPauseScreen();
    }
    
    private void HidePauseMenue()
    {
        pauseMenu.HidePauseScreen();
    }
    
    
    void UpdateGameScore(int scoreValue)
    {
        scoreText.text = $"Score: {scoreValue}";
    }
}
