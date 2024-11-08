using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool gameplayScene = false; // !!!FOR TESTING
    [SerializeField] private float difficultyIncreaseRate = 15f;
    private float _difficultyTimer;
    private int _currentScore = 0;
    private int _currentDifficulty = 0;
    private int _highscore = 0;
    
    private GameState lastState;
    private GameState currentState;
    public event Action OnGameStart;
    public event Action OnGamePaused;
    public event Action OnGameResumed;
    public event Action<int> OnScoreUpdated;

    public event Action<int> OnDifficultyIncreased;

    public event Action<GameState> OnGameStateChanged;
    
    public int Highscore => _highscore;
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

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += HandleSceneChanged;
    }

    private void Update()
    {
        if (currentState != GameState.Playing) return;

        if (_difficultyTimer > 0f)
        {
            _difficultyTimer -= Time.deltaTime;
        }
        else
        {
            _currentDifficulty++;
            OnDifficultyIncreased?.Invoke(_currentDifficulty);
            
            _difficultyTimer = difficultyIncreaseRate;
        }
    }

    private void HandleSceneChanged(Scene unloaded, Scene loaded)
    {
        if (loaded.buildIndex == 0)
        {
            StopAllCoroutines();
            SetState(GameState.Menu);
        }
        else if (loaded.buildIndex == 1)
        {
            SetState(GameState.Waiting);
        }
    }

    private void Start()
    {
        currentState = GameState.Menu;
        if(gameplayScene) StartGame();
    }

    public void StartGame()
    {
        _difficultyTimer = difficultyIncreaseRate;
        _currentDifficulty = 0;
        _currentScore = 0;
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
                if(lastState != GameState.Paused) StartCoroutine(StartGameCoroutine()); // Don't restart Countdown if unpaused
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                OnGamePaused?.Invoke();
                Debug.Log("Pause");
                Time.timeScale = 0;
                break;
            case GameState.Finished:
                Time.timeScale = 0;

                bool highscore = CheckNewHighscore();
                
                UIManager.Instance.FinishGameScreen(_currentScore, _highscore, highscore);
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
        _currentScore += score;
        
        OnScoreUpdated?.Invoke(_currentScore);
    }

    bool CheckNewHighscore()
    {
        bool newHighscore = _currentScore > _highscore;
        if (newHighscore)
        {
            _highscore = _currentScore;
            AudioManager.Instance.PlaySFX(SFXType.HighScore);
        }

        return newHighscore;
    }

    public void FinishGame()
    {
        SetState(GameState.Finished);
    }
    
    IEnumerator StartGameCoroutine()
    {
        UIManager.Instance.UpdateCountdown(3);
        yield return new WaitForSeconds(1f);
        UIManager.Instance.UpdateCountdown(2);
        yield return new WaitForSeconds(1f);
        UIManager.Instance.UpdateCountdown(1);
        yield return new WaitForSeconds(1f);
        
        StartGame();
    }
    
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetState(GameState.Waiting);
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
