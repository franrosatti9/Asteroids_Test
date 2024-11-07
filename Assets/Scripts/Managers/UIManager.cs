using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private void OnEnable()
    {
        GameManager.Instance.OnScoreUpdated += UpdateGameScore;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnScoreUpdated -= UpdateGameScore;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateGameScore(int scoreValue)
    {
        scoreText.text = $"Score: {scoreValue}";
    }
}
