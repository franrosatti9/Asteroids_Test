using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int currentScore = 0;
    public event Action OnGameStart;
    public event Action<int> OnScoreUpdated;
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
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void AddScore(int score)
    {
        currentScore += score;
        
        OnScoreUpdated?.Invoke(currentScore);
    }

    public void FinishGame()
    {
        // highscore = currentScore:
    }
    
    
}
