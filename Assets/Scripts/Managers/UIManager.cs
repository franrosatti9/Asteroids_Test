using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private PauseMenuController pauseMenu;
    [SerializeField] private FinishGameScreen finishGameScreen;
    [SerializeField] private TextMeshProUGUI countdownText;

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

    private void Start()
    {
        UpdateHighscore();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGamePaused += ShowPauseMenu;
        GameManager.Instance.OnGameResumed += HidePauseMenu;
        GameManager.Instance.OnScoreUpdated += UpdateGameScore;
        GameManager.Instance.OnGameStart += HideCountdown;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGamePaused -= ShowPauseMenu;
        GameManager.Instance.OnGameResumed -= HidePauseMenu;
        GameManager.Instance.OnScoreUpdated -= UpdateGameScore;
        GameManager.Instance.OnGameStart -= HideCountdown;
    }

    void ShowPauseMenu()
    {
        pauseMenu.ShowPauseScreen();
    }
    
    private void HidePauseMenu()
    {
        pauseMenu.HidePauseScreen();
    }
    
    
    void UpdateGameScore(int scoreValue)
    {
        scoreText.text = $"Score: {scoreValue}";
    }

    void UpdateHighscore()
    {
        highscoreText.text = $"Highscore: {GameManager.Instance.Highscore}";
    }

    public void UpdateCountdown(int time)
    {
        countdownText.text = $"Starting in {time}!"; 
    }

    void HideCountdown()
    {
        countdownText.gameObject.SetActive(false);
    }

    public void FinishGameScreen(int currentScore, int highscore, bool newHighscore)
    {
        finishGameScreen.Setup(currentScore, highscore, newHighscore);
        finishGameScreen.ShowScreen();
    }
}
