using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool gameplayScene = false; // !!!FOR TESTING
    private int currentScore = 0;
    
    private GameState lastState;
    private GameState currentState;
    public event Action OnGameStart;
    public event Action OnGamePaused;
    public event Action OnGameResumed;
    public event Action<int> OnScoreUpdated;

    public event Action<GameState> OnGameStateChanged;
    public static GameManager Instance { get; private set;}

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

    private void Start()
    {
        currentState = GameState.Menu;
        if(gameplayScene) StartGame();
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();
        SetState(GameState.Playing);
    }

    public void SetState(GameState state)
    {
        if(currentState == state) return;
        lastState =  currentState;
        currentState = state;

        OnGameStateChanged?.Invoke(currentState);
        
        switch (state)
        {
            case GameState.Menu:
                Time.timeScale = 1f;
                break;
            case GameState.Waiting:
                StartCoroutine(StartGameCoroutine());
                Time.timeScale = 0f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                OnGamePaused?.Invoke();
                Time.timeScale = 0;
                break;
            case GameState.Finished:
                // UIManager.Instance.ShowEndScreen();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    public void ResumeGame()
    {
        if (currentState != GameState.Paused) return; // Only resume from pause state
        
        OnGameResumed?.Invoke();
        SetState(lastState); // Go back to Waiting or Playing state
    }

    public void PauseGame()
    {
        SetState(GameState.Paused);
    }

    public void TogglePause()
    {
        if (currentState == GameState.Paused) ResumeGame();
        else if (currentState is GameState.Playing or GameState.Waiting) PauseGame();
    }

    public void AddScore(int score)
    {
        currentScore += score;
        
        OnScoreUpdated?.Invoke(currentScore);
    }

    public void FinishGame()
    {
        SetState(GameState.Finished);
    }

    IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        // UIManager.Instance.UpdateCountdown
        
        StartGame();
    }
    
    
}

public enum GameState
{
    Menu,
    Waiting,
    Playing,
    Paused,
    Finished
}
